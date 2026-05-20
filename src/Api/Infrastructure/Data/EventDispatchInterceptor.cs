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

        var aggregates = appDbContext.EventSourced.Values;

        var events = aggregates
            .SelectMany(a => a.GetUncommittedEvents())
            .ToArray();

        await eventbus.PublishAsync(events, cancellationToken);

        foreach (var aggregate in aggregates)
            aggregate.ClearUncommittedEvents();

        appDbContext.EventSourced.Clear();

        return await base.SavedChangesAsync(eventData, result, cancellationToken);

    }
}
