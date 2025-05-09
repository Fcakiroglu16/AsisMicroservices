namespace Microservice1.API.Services
{
    public class StockServices(HttpClient httpClient, IHttpClientFactory httpClientFactory)
    {
        public async Task<string> GetStockDataAsync()
        {
            var response = await httpClient.GetAsync("api/stocks");
            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

            throw new Exception("Error fetching stock data");
        }
    }
}
