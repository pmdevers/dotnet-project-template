using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectJsonConverter))]
public readonly record struct Money(Amount Value, Currency Currency) : IValueObject<Money>
{
    public static readonly Money Empty = default;

    public Amount Value { get; init; } = Value;

    public Currency Currency { get; init; } =
        Currency == Currency.Empty
            ? throw new ArgumentException("Currency cannot be empty.", nameof(Currency))
            : Currency;

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out Money result)
    {
        result = Empty;

        if (string.IsNullOrEmpty(s))
        {
            return true;
        }

        var parts = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length > 1)
        {
            var currency = Currency.TryParse(parts[0], formatProvider, out var parsedCurrency) ? parsedCurrency : Currency.Empty;
            var amount = Amount.TryParse(parts[1], formatProvider, out var parsedAmount) ? parsedAmount : Amount.Zero;
            result = new(amount, currency);
            return true;
        }

        return false;
    }

    public override string ToString() => $"{Currency} {Value}";
}
