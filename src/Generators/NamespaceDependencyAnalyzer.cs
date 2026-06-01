using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Template.Generators;

/// <summary>
/// Roslyn analyzer that enforces namespace dependency restrictions declared via
/// <c>[assembly: RestrictNamespaceReference("A", "B")]</c> attributes.
/// When a type inside namespace A (or a sub-namespace of A) references a type
/// from namespace B (or a sub-namespace of B) a diagnostic is reported.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class NamespaceDependencyAnalyzer : DiagnosticAnalyzer
{
    private const string AttributeShortName = "RestrictNamespaceReferenceAttribute";

    public static readonly DiagnosticDescriptor Rule = new(
        id: "NS0001",
        title: "Forbidden namespace reference",
        messageFormat: "Type '{0}' in namespace '{1}' must not reference type '{2}' from restricted namespace '{3}'",
        category: "Architecture",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Enforces namespace dependency restrictions declared with [assembly: RestrictNamespaceReference].");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(compilationCtx =>
        {
            var restrictions = ReadRestrictions(compilationCtx.Compilation);
            if (restrictions.Count == 0)
                return;

            compilationCtx.RegisterSyntaxNodeAction(
                nodeCtx => AnalyzeNode(nodeCtx, restrictions),
                SyntaxKind.IdentifierName,
                SyntaxKind.GenericName);
        });
    }

    // -------------------------------------------------------------------------
    // Read [assembly: RestrictNamespaceReference(from, to)] attributes
    // -------------------------------------------------------------------------
    private static List<(string From, string To)> ReadRestrictions(Compilation compilation)
    {
        var result = new List<(string, string)>();

        foreach (var attr in compilation.Assembly.GetAttributes())
        {
            var attrClass = attr.AttributeClass;
            if (attrClass is null)
                continue;

            if (attrClass.Name != AttributeShortName &&
                attrClass.Name != "RestrictNamespaceReference")
                continue;

            if (attr.ConstructorArguments.Length == 2 &&
                attr.ConstructorArguments[0].Value is string from &&
                attr.ConstructorArguments[1].Value is string to)
            {
                result.Add((from, to));
            }
        }

        return result;
    }

    // -------------------------------------------------------------------------
    // Per-node analysis
    // -------------------------------------------------------------------------
    private static void AnalyzeNode(
        SyntaxNodeAnalysisContext ctx,
        List<(string From, string To)> restrictions)
    {
        // Resolve the referenced symbol
        var symbolInfo = ctx.SemanticModel.GetSymbolInfo(ctx.Node, ctx.CancellationToken);
        var referencedSymbol = (symbolInfo.Symbol ?? symbolInfo.CandidateSymbols.FirstOrDefault())
            as INamedTypeSymbol;

        if (referencedSymbol is null)
            return;

        var referencedNs = referencedSymbol.ContainingNamespace?.ToDisplayString();
        if (string.IsNullOrEmpty(referencedNs))
            return;

        // Find the containing type of the usage site
        var containingType = ctx.ContainingSymbol;
        while (containingType is not null && containingType is not INamedTypeSymbol)
            containingType = containingType.ContainingSymbol;

        if (containingType is not INamedTypeSymbol containingNamedType)
            return;

        var containingNs = containingNamedType.ContainingNamespace?.ToDisplayString();
        if (string.IsNullOrEmpty(containingNs))
            return;

        // Check every declared restriction
        foreach (var (from, to) in restrictions)
        {
            if (IsInNamespace(containingNs, from) && IsInNamespace(referencedNs, to))
            {
                var diagnostic = Diagnostic.Create(
                    Rule,
                    ctx.Node.GetLocation(),
                    containingNamedType.Name,
                    containingNs,
                    referencedSymbol.Name,
                    referencedNs);

                ctx.ReportDiagnostic(diagnostic);
                return; // one diagnostic per node is enough
            }
        }
    }

    /// <summary>
    /// Returns true when <paramref name="ns"/> equals <paramref name="prefix"/>
    /// or is a direct sub-namespace of it (e.g. "A.B" is inside "A").
    /// </summary>
    private static bool IsInNamespace(string ns, string prefix) =>
        ns == prefix || ns.StartsWith(prefix + ".");
}
