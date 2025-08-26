using Tokero.Models;

namespace Tokero.Interfaces
{
    public interface IPriceService
    {
        Task<decimal> GetHistoricalPriceAsync (string symbol, DateTime date);

        Task<decimal> GetLatestPriceAsync (string symbol);

        Task<List<CryptocurrencyInfo>> GetAllCryptocurrenciesAsync (int limit = 100);
    }
}