using CQRS.Core.Domain;

namespace CQRS.Core.Handlers;

public interface IEventSourcingHandler
{
    Task SaveAsync(AggregateRoot aggregateRoot);
    Task<T> GetByIdAsync<T>(Guid id);
}