namespace Template.Api.Infrastructure.Data.Models;

public class EventDocument
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid AggregateId { get; set; } = default!;
    public string AggregateName { get; set; } = default!;
    public int Version { get; set; }
    public string Type { get; set; } = default!;
    public string Data { get; set; } = default!;
    public DateTimeOffset? CreatedOn { get; set; }
}
