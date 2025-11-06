using Confluent.Kafka;
using CQRS.Core.Commnds;
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Post.Cmd.Api.Commands;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handler;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

builder.Services.AddSingleton<ICommandDispatcher>(sp =>
{
    using var scope = sp.CreateScope();
    var commandHandler = scope.ServiceProvider.GetRequiredService<ICommandHandler>();
    var dispatcher = new CommandDispatcher();
    
    var projectDefinedTypes = typeof(CommandHandler).Assembly.DefinedTypes;
    var baseType = typeof(BaseCommand);
    var concreteCommands =  projectDefinedTypes
        .Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t));

    foreach (var command in concreteCommands)
    {
        var handleMethod = typeof(ICommandHandler).GetMethod("HandleAsync", [command])!;
        var delegateType = typeof(Func<,>).MakeGenericType(command, typeof(Task));
        var handler = Delegate.CreateDelegate(delegateType, commandHandler, handleMethod);
        
        var registerMethod = typeof(CommandDispatcher).GetMethod(nameof(CommandDispatcher.RegisterHandler))!
            .MakeGenericMethod(command);
        registerMethod.Invoke(dispatcher, [handler]);
    }
    return dispatcher;
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.Run();