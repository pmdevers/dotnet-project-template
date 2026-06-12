using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectJsonConverter))]
public readonly record struct LicensePlate(string Value) : IValueObject<LicensePlate>
{
    public string Value { get; init; }
        = string.IsNullOrWhiteSpace(Value)
        ? throw new ArgumentException("LicensePlate cannot be empty.")
        : Value.Trim().ToUpper();

    public static LicensePlate Create(string value) => new(value);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out LicensePlate result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = default;
            return false;
        }

        result = Create(s);
        return true;
    }

    public override string ToString()
        => Value;

    public static implicit operator string(LicensePlate licensePlate) => licensePlate.Value;
    public static explicit operator LicensePlate(string value) => Create(value);
}
