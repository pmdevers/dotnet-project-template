using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectJsonConverter))]
public readonly record struct FuelType : IValueObject<FuelType>
{
    public static readonly FuelType Unknown = default;
    public static readonly FuelType Petrol = new(nameof(Petrol), "Petrol fuel type.");
    public static readonly FuelType Diesel = new(nameof(Diesel), "Diesel fuel type.");
    public static readonly FuelType Electric = new(nameof(Electric), "Electric fuel type.");
    public static readonly FuelType Hybrid = new(nameof(Hybrid), "Hybrid fuel type combining petrol and electric power.");

    public static IReadOnlyList<FuelType> All =>
    [
        Petrol,
        Diesel,
        Electric,
        Hybrid
    ];

    private static readonly FrozenDictionary<string, FuelType> Lookup = All.ToFrozenDictionary(x => x.Name.Value, StringComparer.OrdinalIgnoreCase);

    private FuelType(NonEmptyString name, NonEmptyString description)
    {
        Name = name;
        Description = description;
    }
    public NonEmptyString Name { get; }
    public NonEmptyString Description { get; }
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out FuelType result)
    {
        if (s is not null && Lookup.TryGetValue(s, out result))
            return true;

        result = Unknown;
        return false;
    }
    public override string ToString()
        => Name.Value;
}
