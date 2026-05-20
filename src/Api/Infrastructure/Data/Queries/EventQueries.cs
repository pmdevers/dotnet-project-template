using Microsoft.EntityFrameworkCore;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Infrastructure.Data.Queries;

public static class EventQueries
{
    public static async Task<IEnumerable<DomainEvent>> GetEventsAsync(this AppDbContext dbContext, Guid aggregateId, CancellationToken cancellationToken)
    {
        var events = await dbContext.Events
            .AsNoTracking()
            .Where(x => x.AggregateId == aggregateId)
            .ToArrayAsync(cancellationToken);

        var domainEvents = events
            .Select(e => EventSerializer.Deserialize(e.Data, e.Type))
            .ToArray();

        return domainEvents;
    }

    public static async Task<T> GetAggregate<T>(this IEnumerable<DomainEvent> events)
        where T : AggregateRoot, new()
    {
        var eventCollection = EventCollection.From([.. events]);
        return AggregateRoot.LoadFromHistory<T>(eventCollection);
    }
}
