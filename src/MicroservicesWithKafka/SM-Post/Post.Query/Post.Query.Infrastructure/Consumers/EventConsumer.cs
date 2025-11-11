using System.Text.Json;
using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;

namespace Post.Query.Infrastructure.Consumers;

public class EventConsumer : IEventConsumer
{
    private readonly ConsumerConfig _config;
    private readonly IEventHandler _eventHandler;

    public EventConsumer(IOptions<ConsumerConfig> config, IEventHandler eventHandler)
    {
        _eventHandler = eventHandler;
        _config = config.Value;
    }

    public void Consume(string topic)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config)
            .SetKeyDeserializer(Deserializers.Utf8)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();

        consumer.Subscribe(topic);

        var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
        while (true)
        {
            var consumerResult = consumer.Consume();
            if (consumerResult?.Message == null) continue;
            var @event = JsonSerializer.Deserialize<BaseEvent>(consumerResult.Message.Value, options);
            var handlerMethod = _eventHandler.GetType().GetMethod("On", [@event.GetType()]);
            if (handlerMethod == null)
            {
                throw new ArgumentNullException($"The On method was not found in handler for {@event.GetType().Name}");
            }
            handlerMethod.Invoke(_eventHandler, [@event]);
            consumer.Commit(consumerResult);
        }
    }
}