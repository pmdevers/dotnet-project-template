using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectJsonConverter))]
public readonly record struct Category : IValueObject<Category>
{
    public static Category Unknown => default;
    public static Category Standard => new(nameof(Standard), "Standard cars with basic features and good value for money.");
    public static Category Economy => new(nameof(Economy), "Affordable cars with basic features and good fuel efficiency.");
    public static Category Premium => new(nameof(Premium), "Premium cars with high-end features and performance.");
    public static Category Comfort => new(nameof(Comfort), "Comfort cars with enhanced features and comfort for a pleasant driving experience.");
    public static Category SUV => new(nameof(SUV), "Sport Utility Vehicles with spacious interiors, off-road capabilities, and versatile features for various needs.");
    public static Category Luxury => new(nameof(Luxury), "Luxury cars with top-tier features, superior comfort, and exceptional performance for discerning customers.");

    private Category(NonEmptyString name, NonEmptyString description)
    {
        Name = name;
        Description = description;
    }

    public NonEmptyString Name { get; }
    public NonEmptyString Description { get; }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out Category result)
    {
        result = s switch
        {
            nameof(Premium) => Premium,
            nameof(Standard) => Standard,
            nameof(Economy) => Economy,
            nameof(Comfort) => Comfort,
            nameof(SUV) => SUV,
            nameof(Luxury) => Luxury,
            _ => Unknown
        };
        return result != Unknown;
    }

    public override string ToString()
        => Name.Value;
}
