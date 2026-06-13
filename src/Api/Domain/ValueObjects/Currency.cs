using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectJsonConverter))]
public readonly record struct Currency(string Code) : IValueObject<Currency>
{
    public static Currency Empty { get; } = default;
    public static Currency Dollar { get; } = new("$");
    public static Currency Euro { get; } = new("€");

    public string Code { get; init; } =
        string.IsNullOrWhiteSpace(Code)
            ? throw new ArgumentNullException(nameof(Code))
            : Code.Trim().ToUpperInvariant();

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out Currency result)
    {
        result = Empty;

        if (string.IsNullOrEmpty(s))
        {
            return true;
        }

        var dollar = new[] { "$", "USD", "dols." };
        var euro = new[] { "€", "Euro", "EUR", "EURO" };

        result = s switch
        {
            var str when dollar.Contains(str) => Dollar,
            var str when euro.Contains(str) => Euro,
            _ => Empty
        };

        return result != Empty;
    }

    public override string ToString() => Code;
}
