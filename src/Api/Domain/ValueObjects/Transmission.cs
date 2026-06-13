using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectJsonConverter))]
public readonly record struct Transmission : IValueObject<Transmission>
{
    public static Transmission Unknown => default;
    public static Transmission Manual => new(nameof(Manual), "Manual transmission type.");
    public static Transmission Automatic => new(nameof(Automatic), "Automatic transmission type.");
    private Transmission(NonEmptyString name, NonEmptyString description)
    {
        Name = name;
        Description = description;
    }
    public NonEmptyString Name { get; }
    public NonEmptyString Description { get; }
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out Transmission result)
    {
        result = s switch
        {
            nameof(Manual) => Manual,
            nameof(Automatic) => Automatic,
            _ => Unknown
        };
        return result != Unknown;
    }
    public override string ToString()
        => Name.Value;
}
