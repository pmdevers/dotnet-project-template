using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Template.Generators;

/// <summary>
/// Emits the <c>RestrictNamespaceReferenceAttribute</c> into every consuming assembly
/// so developers can use it without adding an extra package reference.
/// </summary>
[Generator]
public sealed class NamespaceDependencyAttributeGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            var ns = typeof(NamespaceDependencyAttributeGenerator).Namespace.Split('.')[0];
            var source = TemplateHelper.LoadTemplate("RestrictNamespaceReferenceAttribute.cs");
            var rendered = TemplateHelper.RenderTemplate(source, new System.Collections.Generic.Dictionary<string, object>
            {
                { "namespace", ns }
            });
            ctx.AddSource("RestrictNamespaceReferenceAttribute.g.cs", SourceText.From(rendered, Encoding.UTF8));
        });
    }
}
