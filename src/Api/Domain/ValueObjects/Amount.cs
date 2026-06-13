using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectJsonConverter))]
public readonly record struct Amount(decimal value) : IValueObject<Amount>
{
    public static Amount Zero { get; } = default;

    public decimal Value { get; init; } =
        value < 0m
            ? throw new ArgumentOutOfRangeException(nameof(value), "Amount cannot be negative.")
            : value;

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out Amount result)
    {
        result = Zero;

        if (string.IsNullOrEmpty(s))
        {
            return true;
        }

        if (decimal.TryParse(s, NumberStyles.Any, formatProvider, out decimal value))
        {
            result = new Amount(value);
            return true;
        }

        return false;
    }

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);

    public static implicit operator Amount(decimal val) => new(val);

    /// <inheritdoc />
    public static explicit operator decimal(Amount val) => val.Value;

    /// <inheritdoc />
    public static explicit operator double(Amount val) => (double)val.Value;
}
