using CQRS.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Post.Query.Infrastructure.Consumers;

public class ConsumerHostedService : IHostedService
{
    private readonly ILogger<ConsumerHostedService> _logger;
    private readonly IServiceProvider _sp;

    public ConsumerHostedService(
        IServiceProvider sp,
        ILogger<ConsumerHostedService> logger)
    {
        _sp = sp;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting event consumer");
        using var scope = _sp.CreateScope();
        var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();
        var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
        Task.Run(() => eventConsumer.Consume(topic), cancellationToken);
        
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping event consumer");
        return Task.CompletedTask;
    }
}