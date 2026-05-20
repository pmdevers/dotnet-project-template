using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;
using Template.Api.Infrastructure.Data.Models;
using Template.Api.Infrastructure.Data.Queries;

namespace Template.Api.Infrastructure.Data;

public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    private Dictionary<string, AggregateRoot> EventSourced { get; } = [];
    
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
        EventSourced.Clear();

        return base.SaveChangesAsync(cancellationToken);
    }
}

public partial class AppDbContext : IRepository<Car, CarId>
{
    public void Add(Car entity) 
        => Cars.Add(entity);

    public void Delete(Car entity) 
        => Cars.Remove(entity);

    public Task<Car?> TryFindAsync(CarId id, CancellationToken cancellationToken = default)
        => Cars.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}

public partial class AppDbContext : IRepository<Reservation, ReservationId>
{
    public void Add(Reservation entity)
    {
        EventSourced.Add(entity.Id.ToString(), entity);
    }

    public void Delete(Reservation entity)
    {
    }

    public async Task<Reservation?> TryFindAsync(ReservationId id, CancellationToken cancellationToken = default)
    {
        var events = await Events
           .AsNoTracking()
           .Where(x => x.AggregateId == id.ToString())
           .Select(x => new
           {
               x.Data,
               x.Type,
           }).ToListAsync(cancellationToken);

        var domainEvents = events
            .Select(e => EventSerializer.Deserialize(e.Data, e.Type))
            .ToArray();

        var eventCollection = EventCollection.From(domainEvents);
        return AggregateRoot.LoadFromHistory<Reservation>(eventCollection);
    }
}
