using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Infrastructure.Data.Config;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.Property<int>("Id")
               .UseIdentityByDefaultColumn();

        builder.HasKey("Id");

        builder.Property(x => x.LicensePlate)
            .HasConversion(
                v => v.Value,
                v => LicensePlate.Create(v))
            .IsRequired();

        builder.Property(x => x.Name)
            .HasConversion(
                v => v.Value,
                v => NonEmptyString.Create(v))
            .IsRequired();

        builder.Property(x => x.Description)
            .HasConversion(
                v => v.Value,
                v => NonEmptyString.Create(v))
            .IsRequired();

        builder.Property(x => x.Brand)
            .HasConversion(
                v => v.Value,
                v => NonEmptyString.Create(v))
            .IsRequired();

        builder.Property(x => x.Model)
            .HasConversion(
                v => v.Value,
                v => NonEmptyString.Create(v))
            .IsRequired();

        builder.Property(x => x.Category)
            .HasConversion<ValueObjectConversion<Category>>()
            .IsRequired();

        builder.ComplexProperty(x => x.PricePerDay, money =>
        {
            money.Property(p => p.Value)
                .HasConversion(
                    v => v.Value,
                    v => new Amount(v))
                .HasColumnName("Price")
                .IsRequired();

            money.Property(p => p.Currency)
                .HasConversion(
                    v => v.Code,
                    v => new Currency(v))
                .HasColumnName("Currency")
                .IsRequired();
        });

        builder.OwnsOne(x => x.Specifications, spec =>
        {
            spec.Property(s => s.Seats)
                .HasColumnName("Seats")
                .IsRequired();
            spec.Property(s => s.Luggage)
                .HasColumnName("Luggage")
                .IsRequired();
            spec.Property(s => s.Transmission)
                .HasConversion<ValueObjectConversion<Transmission>>()
                .HasColumnName("Transmission")
                .IsRequired();
            spec.Property(s => s.FuelType)
                .HasConversion<ValueObjectConversion<FuelType>>()
                .HasColumnName("FuelType")
                .IsRequired();
            spec.Property(s => s.HorsePower)
                .HasColumnName("HorsePower")
                .IsRequired();
        });
    }
}

internal class ValueObjectConversion<T> : ValueConverter<T, string>
    where T : struct, IValueObject<T>
{
    public ValueObjectConversion() : base(
        v => v.ToString() ?? string.Empty,
        v => ToValueObject(v))
    {
    }
    private static T ToValueObject(string value)
        => T.TryParse(value, null, out var result) ? result : default!;
}

