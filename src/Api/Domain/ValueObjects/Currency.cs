using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectJsonConverter))]
public readonly record struct Currency(string Code) : IValueObject<Currency>
{
    public static readonly Currency Empty = default;
    public static readonly Currency Dollar = new("$");
    public static readonly Currency Euro = new("€");

    public static IReadOnlyList<Currency> All =>
    [
        Dollar,
        Euro
    ];

    private static readonly FrozenDictionary<string, Currency> Lookup = All.ToFrozenDictionary(x => x.Code, StringComparer.OrdinalIgnoreCase);

    public string Code { get; init; } =
        string.IsNullOrWhiteSpace(Code)
            ? throw Errors.CurrencyCodeCannotBeNullOrWhiteSpace(nameof(Code))
            : Code.Trim().ToUpperInvariant();

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out Currency result)
    {
        if (s is not null && Lookup.TryGetValue(s, out result))
            return true;

        result = Empty;
        return false;
    }

    public override string ToString() => Code;
}
