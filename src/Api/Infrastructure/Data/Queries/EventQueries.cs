using Microsoft.EntityFrameworkCore;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Infrastructure.Data.Queries;

public static class EventQueries
{
    public static async Task<T> GetAggregate<T>(this AppDbContext dbContext, string aggregateId, CancellationToken cancellationToken)
        where T : AggregateRoot, new()
    {
        var events = await dbContext.Events
            .AsNoTracking()
            .Where(x => x.AggregateId == aggregateId)
            .Select(x => new
            {
                x.Data,
                x.Type,
            }).ToListAsync(cancellationToken);

        var domainEvents = events
            .Select(e => EventSerializer.Deserialize(e.Data, e.Type))
            .ToArray();

        var eventCollection = EventCollection.From(domainEvents);
        return AggregateRoot.LoadFromHistory<T>(eventCollection);
    }
}
