using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectJsonConverter))]
public readonly record struct FuelType : IValueObject<FuelType>
{
    public static FuelType Unknown => default;
    public static FuelType Petrol => new(nameof(Petrol), "Petrol fuel type.");
    public static FuelType Diesel => new(nameof(Diesel), "Diesel fuel type.");
    public static FuelType Electric => new(nameof(Electric), "Electric fuel type.");
    public static FuelType Hybrid => new(nameof(Hybrid), "Hybrid fuel type combining petrol and electric power.");

    private FuelType(NonEmptyString name, NonEmptyString description)
    {
        Name = name;
        Description = description;
    }
    public NonEmptyString Name { get; }
    public NonEmptyString Description { get; }
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out FuelType result)
    {
        result = s switch
        {
            nameof(Petrol) => Petrol,
            nameof(Diesel) => Diesel,
            nameof(Electric) => Electric,
            nameof(Hybrid) => Hybrid,
            _ => Unknown
        };
        return result != Unknown;
    }
    public override string ToString()
        => Name.Value;
}
