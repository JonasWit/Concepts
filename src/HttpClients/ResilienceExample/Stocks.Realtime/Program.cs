using Microsoft.Extensions.Http.Resilience;
using Polly;
using Stocks.Realtime.Api.Realtime;
using Stocks.Realtime.Stocks;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

builder.Services.AddSingleton<ActiveTickerManager>();
builder.Services.AddHostedService<StocksFeedUpdater>();

builder.Services.Configure<StockUpdateOptions>(builder.Configuration.GetSection("StockUpdateOptions"));

builder.Services.AddHttpClient<StocksApiClient>(client => client.BaseAddress = new Uri("http://stocks-api"))
    //.AddStandardResilienceHandler()
    //.Configure(options =>
    //{ 
    //    //...
    //})
    .AddResilienceHandler("custom", pipeline =>
    {
        pipeline.AddTimeout(TimeSpan.FromSeconds(5));

        pipeline.AddRetry(new HttpRetryStrategyOptions
        {
            MaxRetryAttempts = 3,
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            Delay = TimeSpan.FromMilliseconds(500)
        });

        pipeline.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
        {
            SamplingDuration = TimeSpan.FromSeconds(10),
            FailureRatio = 0.9,
            MinimumThroughput = 5,
            BreakDuration = TimeSpan.FromSeconds(5)
        });

        pipeline.AddTimeout(TimeSpan.FromSeconds(1));
    });

WebApplication app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<StocksFeedHub>("/stocks-feed");

app.Run();
