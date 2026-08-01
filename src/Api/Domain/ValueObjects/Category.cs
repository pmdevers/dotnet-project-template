using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectJsonConverter))]
public readonly record struct Category : IValueObject<Category>
{
    public static readonly Category Unknown = default;
    public static readonly Category Standard = new(nameof(Standard), "Standard cars with basic features and good value for money.");
    public static readonly Category Economy = new(nameof(Economy), "Affordable cars with basic features and good fuel efficiency.");
    public static readonly Category Premium = new(nameof(Premium), "Premium cars with high-end features and performance.");
    public static readonly Category Comfort = new(nameof(Comfort), "Comfort cars with enhanced features and comfort for a pleasant driving experience.");
    public static readonly Category SUV = new(nameof(SUV), "Sport Utility Vehicles with spacious interiors, off-road capabilities, and versatile features for various needs.");
    public static readonly Category Luxury = new(nameof(Luxury), "Luxury cars with top-tier features, superior comfort, and exceptional performance for discerning customers.");

    public static IReadOnlyList<Category> All =>
    [
        Standard,
        Economy,
        Premium,
        Comfort,
        SUV,
        Luxury
    ];

    private static readonly FrozenDictionary<string, Category> Lookup = All.ToFrozenDictionary(x => x.Name.Value, StringComparer.OrdinalIgnoreCase);

    private Category(NonEmptyString name, NonEmptyString description)
    {
        Name = name;
        Description = description;
    }

    public NonEmptyString Name { get; }
    public NonEmptyString Description { get; }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out Category result)
    {
        if (s is not null && Lookup.TryGetValue(s, out result))
            return true;

        result = Unknown;

        return false;
    }

    public override string ToString()
        => Name.Value;
}
