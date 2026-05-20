using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;
using Template.Api.Infrastructure.Data.Models;
using Template.Api.Infrastructure.Data.Queries;

namespace Template.Api.Infrastructure.Data;

public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public Dictionary<Guid, AggregateRoot> EventSourced { get; } = [];

    public DbSet<EventDocument> Events { get; set; }
    public DbSet<Car> Cars { get; set; }

    public IRepository<TAggregate, TId> GetRepository<TAggregate, TId>()
        where TAggregate : AggregateRoot
        where TId : struct
        => (IRepository<TAggregate, TId>)this;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var aggregate in EventSourced)
        {
            var dbSet = Events;
            var events = aggregate.Value.GetUncommittedEvents();
            var version = events.Version;

            foreach (var evt in events)
            {
                var eventDoc = new EventDocument
                {
                    AggregateId = aggregate.Key,
                    AggregateName = aggregate.Value.GetType().Name,
                    Data = EventSerializer.Serialize(evt),
                    Type = EventSerializer.GetTypeName(evt),
                    Version = version++,
                    CreatedOn = evt.OccurredOn
                };
                dbSet.Add(eventDoc);
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}

public partial class AppDbContext : IRepository<Car, CarId>
{
    public void Add(Car entity)
    {
        Set<Car>().Add(entity);
        EventSourced.TryAdd(entity.Id, entity);
    }

    public void Delete(Car entity)
        => Remove(entity);

    public async Task<Car?> TryFindAsync(CarId id, CancellationToken cancellationToken = default)
    {
        var aggregate = await Set<Car>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (aggregate == null)
            return null;

        EventSourced.TryAdd(id, aggregate);
        return aggregate;
    }
}

public partial class AppDbContext : IRepository<Reservation, ReservationId>
{
    public void Add(Reservation entity)
        => EventSourced.TryAdd(entity.Id, entity);

    public void Delete(Reservation entity)
    {
    }

    public async Task<Reservation?> TryFindAsync(ReservationId id, CancellationToken cancellationToken = default)
    {
        var events = await this.GetEventsAsync(id, cancellationToken);
        var aggregate = await events.GetAggregate<Reservation>();

        if (aggregate == null)
            return null;

        EventSourced.TryAdd(id, aggregate);
        return aggregate;
    }
}
