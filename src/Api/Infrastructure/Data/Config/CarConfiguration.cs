using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Infrastructure.Data.Config;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    private static ValueConverter<CarId, Guid> converter => 
        new(x => (Guid)x, x=> CarId.From(x));

    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.Property(x => x.Id)
            .HasConversion(converter)
            .ValueGeneratedNever()
            .IsRequired();

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LicensePlate)
            .HasConversion(
                v => v.ToJsonValue(),
                v => LicensePlate.Parse(v, null))
            .IsRequired();
    }
}
