using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handler;

public class EventSourcingHandler(
    IEventStore eventStore,
    IEventProducer eventProducer) : IEventSourcingHandler<PostAggregate>
{
    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommitedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommited();
    }

    public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
    {
        var aggregate = new PostAggregate();
        var events = await eventStore.GetEventsAsync(aggregateId);
        if (events.Count == 0) return aggregate;
        aggregate.ReplayEvents(events);
        aggregate.Version = events.Select(e => e.Version).Max();
        return aggregate;
    }

    public async Task RepublishEventsAsync()
    {
        var aggregateIds = await eventStore.GetAggregateIdsAsync();
        if (aggregateIds is null || aggregateIds.Count == 0)
        {
            return;
        }

        foreach (var aid in aggregateIds)
        {
            var aggregate = await GetByIdAsync(aid);
            if (aggregate is null || !aggregate.Active)
            {
                continue;
            }
            var events = await eventStore.GetEventsAsync(aid);
            foreach (var e in events)
            {
                var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
                if (string.IsNullOrWhiteSpace(topic))
                {
                    throw new ArgumentNullException(topic, "topic not found!");
                }
                await eventProducer.ProduceAsync(topic, e);
            }
        }
    }
}