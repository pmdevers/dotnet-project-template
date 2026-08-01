using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.ValueObjects;

[JsonConverter(typeof(ValueObjectJsonConverter))]
public readonly record struct ReservationDate(DateOnly Date, TimeProvider? timeProvider = null) : IValueObject<ReservationDate>
{
    public DateOnly Value { get; init; }
        = Date < DateOnly.FromDateTime((timeProvider ?? TimeProvider.System).GetUtcNow().Date)
        ? throw Errors.ReservationDateCannotBeInThePast()
        : Date;

    public static ReservationDate Today(TimeProvider? timeProvider = null)
    {
        var provider = timeProvider ?? TimeProvider.System;
        var today = provider.GetUtcNow().Date;
        return Create(DateOnly.FromDateTime(today), provider);
    }

    public static ReservationDate Create(DateOnly value, TimeProvider? timeProvider = null)
        => new(value, timeProvider);

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? formatProvider, [MaybeNullWhen(false)] out ReservationDate result)
    {
        if (DateOnly.TryParse(s, formatProvider, out var parsedDate))
        {
            result = Create(parsedDate);
            return true;
        }

        result = default;
        return false;
    }

    public override string ToString()
        => Value.ToString("yyyy-MM-dd");

    public ReservationDate AddDays(int days)
        => Create(Value.AddDays(days));

    public static implicit operator DateOnly(ReservationDate date) => date.Value;
    public static explicit operator ReservationDate(DateOnly value) => Create(value);
}
