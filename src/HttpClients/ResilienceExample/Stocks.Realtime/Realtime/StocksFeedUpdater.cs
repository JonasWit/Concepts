using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Stocks.Realtime.Stocks;

namespace Stocks.Realtime.Api.Realtime;

internal sealed class StocksFeedUpdater(
    ActiveTickerManager activeTickerManager,
    IServiceScopeFactory serviceScopeFactory,
    IHubContext<StocksFeedHub, IStockUpdateClient> hubContext,
    IOptions<StockUpdateOptions> options,
    ILogger<StocksFeedUpdater> logger)
    : BackgroundService
{
    private static readonly string[] DefaultTickers = ["MSFT"];

    private readonly Random _random = new();
    private readonly StockUpdateOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Demonstrate updating ticker prices.
        foreach (string ticker in DefaultTickers)
        {
            activeTickerManager.AddTicker(ticker);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            await UpdateStockPrices();

            await Task.Delay(_options.UpdateInterval, stoppingToken);
        }
    }

    private async Task UpdateStockPrices()
    {
        using IServiceScope scope = serviceScopeFactory.CreateScope();
        StocksApiClient client = scope.ServiceProvider.GetRequiredService<StocksApiClient>();

        foreach (string ticker in activeTickerManager.GetAllTickers())
        {
            var sw = Stopwatch.StartNew();
            try
            {
                StockPriceResponse? currentPrice = await client.GetAsync(ticker);
                sw.Stop();

                if (currentPrice == null)
                {
                    continue;
                }

                logger.LogInformation("200 - {Time} ms", sw.ElapsedMilliseconds);

                decimal newPrice = CalculateNewPrice(currentPrice);

                var update = new StockPriceUpdate(ticker, newPrice);

                await hubContext.Clients.Group(ticker).ReceiveStockPriceUpdate(update);
            }
            catch (Exception e)
            {
                sw.Stop();
                logger.LogError(
                    e,
                    "{Status} - {Time} ms - [{Type}]",
                    (int)((e as HttpRequestException)?.StatusCode ?? 0),
                    sw.ElapsedMilliseconds,
                    e.GetType());
            }
        }
    }

    private decimal CalculateNewPrice(StockPriceResponse currentPrice)
    {
        double change = _options.MaxPercentageChange;
        decimal priceFactor = (decimal)(_random.NextDouble() * change * 2 - change);
        decimal priceChange = currentPrice.Price * priceFactor;
        decimal newPrice = Math.Max(0, currentPrice.Price + priceChange);
        newPrice = Math.Round(newPrice, 2);
        return newPrice;
    }
}
