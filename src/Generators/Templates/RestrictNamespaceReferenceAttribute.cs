using System;

namespace {{ namespace }}
{
    /// <summary>
    /// Restricts types in <see cref="FromNamespace"/> from referencing types in <see cref="ToNamespace"/>.
    /// Apply multiple times to declare multiple restrictions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RestrictNamespaceReferenceAttribute : Attribute
    {
        /// <summary>The namespace whose types must not reference <see cref="ToNamespace"/>.</summary>
        public string FromNamespace { get; }

        /// <summary>The namespace that <see cref="FromNamespace"/> types are forbidden to reference.</summary>
        public string ToNamespace { get; }

        public RestrictNamespaceReferenceAttribute(string fromNamespace, string toNamespace)
        {
            FromNamespace = fromNamespace;
            ToNamespace = toNamespace;
        }
    }
}
