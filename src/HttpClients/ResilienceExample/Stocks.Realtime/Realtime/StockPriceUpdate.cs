namespace Stocks.Realtime.Api.Realtime;

#pragma warning disable CA1515
public sealed record StockPriceUpdate(string Ticker, decimal Price);
#pragma warning restore CA1515
