using Microsoft.EntityFrameworkCore.Diagnostics;
using Template.Api.Domain.Abstractions;

namespace Template.Api.Infrastructure.Data;

public class EventDispatchInterceptor(IEventBus eventbus) : SaveChangesInterceptor
{
    // Called after SaveChangesAsync has completed successfully
    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result,
      CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is not AppDbContext appDbContext)
        {
            return await base.SavedChangesAsync(eventData, result, cancellationToken)
                .ConfigureAwait(false);
        }

        // Retrieve all tracked entities that have domain events
        var events = appDbContext.ChangeTracker.Entries<AggregateRoot>()
          .SelectMany(e => e.Entity.GetUncommittedEvents())
          .ToArray();

        var events2 = appDbContext.EventSourced.Values
            .SelectMany(e => e.GetUncommittedEvents())
            .ToArray();

        var allEvents = events.Concat(events2).ToArray();

        // Dispatch and clear domain events
        await eventbus.PublishAsync(allEvents, cancellationToken);

        appDbContext.EventSourced.Clear();

        return await base.SavedChangesAsync(eventData, result, cancellationToken);

    }
}
