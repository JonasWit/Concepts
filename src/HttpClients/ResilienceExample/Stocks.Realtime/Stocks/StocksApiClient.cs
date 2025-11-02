namespace Stocks.Realtime.Stocks;

internal sealed class StocksApiClient(HttpClient httpClient)
{
    public async Task<StockPriceResponse?> GetAsync(string ticker)
    {
        StockPriceResponse? response = await httpClient.GetFromJsonAsync<StockPriceResponse>($"api/stocks/{ticker}");

        return response;
    }
}
