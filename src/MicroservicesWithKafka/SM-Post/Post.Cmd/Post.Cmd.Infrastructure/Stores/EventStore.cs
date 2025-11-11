using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Stores;

public class EventStore(
    IEventStoreRepository eventStoreRepository,
    IEventProducer eventProducer) : IEventStore
{
    public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream = await eventStoreRepository.FindByAggregateId(aggregateId);
        if (eventStream == null || eventStream.Count == 0)
            throw new AggregateNotFoundException("Incorrect post id provided");
        return [.. eventStream.OrderBy(e => e.Version).Select(e => e.EventData!)];
    }

    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await eventStoreRepository.FindByAggregateId(aggregateId);
        if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion) throw new ConcurrencyException();

        var version = expectedVersion;
        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel
            {
                TimeStamp = DateTime.UtcNow,
                AggregateIdentifier = aggregateId,
                AggregateType = nameof(PostAggregate),
                Version = version,
                EventType = eventType,
                EventData = @event
            };

            // TODO: wrap this in mongodb transaction
            await eventStoreRepository.SaveAsync(eventModel);
            var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC") ?? "post-events";
            await eventProducer.ProduceAsync(topic, @event);
        }
        
        // using var session = await mongoClient.StartSessionAsync();
        //
        // try
        // {
        //     await session.WithTransactionAsync(async (session, cancellationToken) =>
        //     {
        //         var version = expectedVersion;
        //         foreach (var @event in events)
        //         {
        //             version++;
        //             @event.Version = version;
        //             var eventType = @event.GetType().Name;
        //             var eventModel = new EventModel
        //             {
        //                 TimeStamp = DateTime.UtcNow,
        //                 AggregateIdentifier = aggregateId,
        //                 AggregateType = nameof(PostAggregate),
        //                 Version = version,
        //                 EventType = eventType,
        //                 EventData = @event
        //             };
        //
        //             await eventStoreRepository.SaveAsync(eventModel, session);
        //             var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC") ?? "post-events";
        //             await eventProducer.ProduceAsync(topic, @event);
        //         }
        //         
        //         return "completed";
        //     });
        // }
        // catch (Exception ex)
        // {
        //     // Log the exception or handle it as appropriate for your application
        //     throw new InvalidOperationException("Failed to save events in transaction", ex);
        // }
    }
}