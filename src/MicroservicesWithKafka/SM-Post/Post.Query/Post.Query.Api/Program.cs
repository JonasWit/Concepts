using Confluent.Kafka;
using Confluent.Kafka.Admin;
using CQRS.Core.Consumers;
using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.Consumers;
using Post.Query.Infrastructure.DataAccess;
using Post.Query.Infrastructure.Handlers;
using Post.Query.Infrastructure.Repositories;
using Scalar.AspNetCore;
using EventHandler = Post.Query.Infrastructure.Handlers.EventHandler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = true;
});

Action<DbContextOptionsBuilder> configureDbContext = o =>
    o.UseLazyLoadingProxies()
        .UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));

builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton(new DatabaseContextFactory(configureDbContext));

// Create DB
var dataContext = builder.Services.BuildServiceProvider().GetRequiredService<DatabaseContext>();
dataContext.Database.EnsureCreated();

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IEventHandler, EventHandler>();
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();

await EnsureTopicExists(builder.Configuration);
builder.Services.AddHostedService<ConsumerHostedService>();

builder.Services.AddOpenApi();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.Run();
return;

static async Task EnsureTopicExists(IConfiguration configuration)
{
    var consumerConfig = new ConsumerConfig();
    configuration.GetSection(nameof(ConsumerConfig)).Bind(consumerConfig);
    
    var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC") ?? "SocialMediaPostEvents";
    
    var adminConfig = new AdminClientConfig
    {
        BootstrapServers = consumerConfig.BootstrapServers
    };

    using var adminClient = new AdminClientBuilder(adminConfig).Build();
    
    try
    {
        var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
        var topicExists = metadata.Topics.Any(t => t.Topic == topic && t.Error.Code == ErrorCode.NoError);
        
        if (!topicExists)
        {
            Console.WriteLine($"Creating Kafka topic: {topic}");
            
            var topicSpec = new TopicSpecification
            {
                Name = topic,
                NumPartitions = 3,
                ReplicationFactor = 1
            };
            
            await adminClient.CreateTopicsAsync([topicSpec]);
            Console.WriteLine($"Successfully created Kafka topic: {topic}");
        }
        else
        {
            Console.WriteLine($"Kafka topic already exists: {topic}");
        }
    }
    catch (CreateTopicsException ex)
    {
        // Topic might already exist, check the specific error
        foreach (var result in ex.Results)
        {
            Console.WriteLine(result.Error.Code == ErrorCode.TopicAlreadyExists
                ? $"Kafka topic already exists: {result.Topic}"
                : $"Failed to create topic {result.Topic}: {result.Error.Reason}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Could not verify/create Kafka topic: {ex.Message}");
        Console.WriteLine("The consumer will still attempt to subscribe, which may fail if topic doesn't exist.");
    }
}