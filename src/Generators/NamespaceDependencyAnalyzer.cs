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
    private const string AggregateRootMetadataName = "Template.Api.Domain.Abstractions.AggregateRoot";

    public static readonly DiagnosticDescriptor Rule = new(
        id: "NS0001",
        title: "Forbidden namespace reference",
        messageFormat: "Type '{0}' in namespace '{1}' must not reference type '{2}' from restricted namespace '{3}'",
        category: "Architecture",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Enforces namespace dependency restrictions declared with [assembly: RestrictNamespaceReference].");

    public static readonly DiagnosticDescriptor PrimitivePropertyRule = new(
        id: "AR0001",
        title: "Primitive property on aggregate root",
        messageFormat: "Aggregate root '{0}' should not expose primitive property '{1}' of type '{2}'. Use a value object instead.",
        category: "Architecture",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Discourages primitive properties on AggregateRoot-derived types so domain concepts are modeled as value objects.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule, PrimitivePropertyRule];

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterCompilationStartAction(compilationCtx =>
        {
            var restrictions = ReadRestrictions(compilationCtx.Compilation);
            var aggregateRootSymbol = compilationCtx.Compilation.GetTypeByMetadataName(AggregateRootMetadataName);

            if (restrictions.Count == 0 && aggregateRootSymbol is null)
                return;

            if (restrictions.Count > 0)
            {
                compilationCtx.RegisterSyntaxNodeAction(
                    nodeCtx => AnalyzeNode(nodeCtx, restrictions),
                    SyntaxKind.IdentifierName,
                    SyntaxKind.GenericName);
            }

            if (aggregateRootSymbol is not null)
            {
                compilationCtx.RegisterSyntaxNodeAction(
                    nodeCtx => AnalyzeAggregateRootProperty(nodeCtx, aggregateRootSymbol),
                    SyntaxKind.PropertyDeclaration);
            }
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
    // AggregateRoot property analysis
    // -------------------------------------------------------------------------
    private static void AnalyzeAggregateRootProperty(
        SyntaxNodeAnalysisContext ctx,
        INamedTypeSymbol aggregateRootSymbol)
    {
        if (ctx.Node is not Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax propertyDeclaration)
            return;

        if (propertyDeclaration.Parent is not Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax typeDeclaration)
            return;

        if (ctx.SemanticModel.GetDeclaredSymbol(typeDeclaration, ctx.CancellationToken) is not INamedTypeSymbol containingType)
            return;

        if (!DerivesFrom(containingType, aggregateRootSymbol))
            return;

        if (ctx.SemanticModel.GetDeclaredSymbol(propertyDeclaration, ctx.CancellationToken) is not IPropertySymbol propertySymbol)
            return;

        if (propertySymbol.IsStatic)
            return;

        if (!IsPrimitiveLike(propertySymbol.Type))
            return;

        var diagnostic = Diagnostic.Create(
            PrimitivePropertyRule,
            propertyDeclaration.Identifier.GetLocation(),
            containingType.Name,
            propertySymbol.Name,
            propertySymbol.Type.ToDisplayString());

        ctx.ReportDiagnostic(diagnostic);
    }

    private static bool DerivesFrom(INamedTypeSymbol type, INamedTypeSymbol baseType)
    {
        for (var current = type.BaseType; current is not null; current = current.BaseType)
        {
            if (SymbolEqualityComparer.Default.Equals(current, baseType))
                return true;
        }

        return false;
    }

    private static bool IsPrimitiveLike(ITypeSymbol type) => type switch
    {
        { SpecialType: not SpecialType.None } special => IsSpecialPrimitive(special.SpecialType),
        INamedTypeSymbol named when named.ContainingNamespace?.ToDisplayString() == "System" && named.Name is "String" or "Guid" or "DateTime" or "DateOnly" or "TimeOnly" or "Decimal" => true,
        _ => false
    };

    private static bool IsSpecialPrimitive(SpecialType specialType) => specialType switch
    {
        SpecialType.System_Boolean or
        SpecialType.System_Char or
        SpecialType.System_SByte or
        SpecialType.System_Byte or
        SpecialType.System_Int16 or
        SpecialType.System_UInt16 or
        SpecialType.System_Int32 or
        SpecialType.System_UInt32 or
        SpecialType.System_Int64 or
        SpecialType.System_UInt64 or
        SpecialType.System_Single or
        SpecialType.System_Double or
        SpecialType.System_String => true,
        _ => false
    };

    // -------------------------------------------------------------------------
    // Per-node analysis
    // -------------------------------------------------------------------------
    private static void AnalyzeNode(
        SyntaxNodeAnalysisContext ctx,
        List<(string From, string To)> restrictions)
    {
        // Resolve the referenced symbol
        var symbolInfo = ctx.SemanticModel.GetSymbolInfo(ctx.Node, ctx.CancellationToken);

        if ((symbolInfo.Symbol ?? symbolInfo.CandidateSymbols.FirstOrDefault()) is not INamedTypeSymbol referencedSymbol)
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
