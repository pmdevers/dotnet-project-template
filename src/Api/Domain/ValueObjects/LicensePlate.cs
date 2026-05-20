using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectConverter<LicensePlate>))]
public readonly record struct LicensePlate(string Value) : IValueObject<LicensePlate>
{
    public static readonly LicensePlate Empty = default;

    public string Value { get; init; } =
       Value?.Trim().ToUpper() ?? throw new ArgumentNullException(nameof(Value));

    public static LicensePlate Parse(string s, IFormatProvider? provider)
        => TryParse(s, provider, out var result) 
        ? result 
        : throw new FormatException($"Invalid license plate: '{s}'.");

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out LicensePlate result)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            result = Empty;
            return false;
        }

        result = new LicensePlate(s);
        return true;
    }

    public readonly string ToJsonValue()
    {
        return Value;
    }
}
