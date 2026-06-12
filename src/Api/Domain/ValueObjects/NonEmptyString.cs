using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectJsonConverter))]
public readonly record struct NonEmptyString(string Value) : IValueObject<NonEmptyString>
{
    public string Value { get; init; }
        = string.IsNullOrWhiteSpace(Value)
        ? throw new ArgumentException("Value cannot be empty.", nameof(Value))
        : Value;

    public static NonEmptyString Create(string value)
        => new(value);

    public override string ToString()
        => Value;

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out NonEmptyString result)
    {
        if (string.IsNullOrEmpty(s))
        {
            result = default;
            return false;
        }
        result = new NonEmptyString(s);
        return true;
    }

    public static implicit operator string(NonEmptyString value) => value.Value;
    public static implicit operator NonEmptyString(string value) => new(value);
}
