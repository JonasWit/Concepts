using System.Diagnostics.CodeAnalysis;

namespace Stocks.Realtime.Api.Realtime;

[SuppressMessage("Design", "CA1515:Consider making public types internal", Justification = "Required to be public for SignalR TypedClientBuilder")]
public interface IStockUpdateClient
{
    Task ReceiveStockPriceUpdate(StockPriceUpdate update);
}
