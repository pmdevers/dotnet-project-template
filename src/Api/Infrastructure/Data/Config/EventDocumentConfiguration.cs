using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Template.Api.Infrastructure.Data.Models;

namespace Template.Api.Infrastructure.Data.Config;

public class EventDocumentConfiguration : IEntityTypeConfiguration<EventDocument>
{
    public void Configure(EntityTypeBuilder<EventDocument> builder)
    {
        builder.ToTable($"Events");

        builder.Property(x=>x.Id)
            .ValueGeneratedNever()
            .IsRequired();
        
        builder.HasKey("Id");

        // Create unique constraint on AggregateId and Version
        builder.HasIndex(e => new { e.AggregateId, e.Version })
            .IsUnique()
            .HasDatabaseName($"IX_AggregateId_Version");

        builder.Property(e => e.AggregateId).IsRequired();
        builder.Property(e => e.Version).IsRequired();
        builder.Property(e => e.Type).IsRequired();
        builder.Property(e => e.Data).IsRequired();
        builder.Property(e => e.CreatedOn).IsRequired();
    }
}
