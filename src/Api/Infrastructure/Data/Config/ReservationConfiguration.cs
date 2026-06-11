using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Api.Domain.ValueObjects;
using Template.Api.Infrastructure.Data.Models;

namespace Template.Api.Infrastructure.Data.Config;

public class ReservationConfiguration : IEntityTypeConfiguration<ReservationModel>
{
    public void Configure(EntityTypeBuilder<ReservationModel> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.LicensePlate)
               .HasConversion(
                   v => v.Value,
                   v => LicensePlate.Create(v)
               );
    }
}
