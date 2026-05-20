using System.Diagnostics.CodeAnalysis;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

public readonly record struct LicensePlate(string Value) : IValueObject<LicensePlate>
{
    public string Value { get; init; } =
        string.IsNullOrWhiteSpace(Value)
            ? throw new FormatException("LicensePlate cannot be empty.")
            : Value.Trim().ToUpper();

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out LicensePlate result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = default;
            return false;
        }

        result = new LicensePlate(s);
        return true;
    }

    public string ToJsonValue()
    {
        return Value;
    }

    // Implicit operators
    public static implicit operator string(LicensePlate licensePlate)
        => licensePlate.Value;

    public static explicit operator LicensePlate(string value)
        => new(value);
}
