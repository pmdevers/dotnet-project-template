namespace Template.Api.Domain.Abstractions;

public interface IUnitOfWork
{
    IRepository<TAggregate, TId> GetRepository<TAggregate, TId>()
        where TAggregate : AggregateRoot
        where TId : struct;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

