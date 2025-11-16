using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Cmd.Infrastructure.Config;

namespace Post.Cmd.Infrastructure.Repositories;

public class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _eventStoreCollection;

    public EventStoreRepository(IOptions<MongoDbConfig> config)
    {
        var mongoClient = new MongoClient(config.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(config.Value.Database);

        _eventStoreCollection = mongoDatabase.GetCollection<EventModel>(config.Value.Collection);
    }

    public Task<List<EventModel>> FindAllAsync()
    {
        return _eventStoreCollection.Find(_ => true).ToListAsync();
    }

    public Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
    {
        return _eventStoreCollection.Find(e => e.AggregateIdentifier == aggregateId).ToListAsync();
    }

    public Task SaveAsync(EventModel @event)
    {
        return _eventStoreCollection.InsertOneAsync(@event);
    }
}