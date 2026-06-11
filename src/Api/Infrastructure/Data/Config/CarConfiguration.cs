using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
    }
}
