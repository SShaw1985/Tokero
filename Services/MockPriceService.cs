using Tokero.Interfaces;
using Tokero.Models;

namespace Tokero.Services
{
    public class MockPriceService : IPriceService
    {
        private readonly Random _random = new();

        private readonly Dictionary<string, decimal> _basePrices = new()
        {
            { "BTC", 30000m },
            { "ETH", 2000m },
            { "SOL", 80m },
            { "XRP", 0.6m },
            { "USDC", 1.0m },
            { "TOKERO", 0.016m },
            { "ADA", 0.5m },
            { "DOT", 7m },
            { "LINK", 15m },
            { "UNI", 8m },
            { "AVAX", 20m },
            { "MATIC", 0.8m },
            { "ATOM", 10m },
            { "NEAR", 3m },
            { "FTM", 0.3m },
            { "ALGO", 0.3m },
            { "VET", 0.02m },
            { "THETA", 1m },
            { "FIL", 5m },
            { "TRX", 0.05m },
            { "EOS", 1m },
            { "AAVE", 200m }
        };

        public MockPriceService ()
        {
        }

        public async Task<decimal> GetHistoricalPriceAsync (string symbol, DateTime date)
        {
            if(!_basePrices.TryGetValue(symbol.ToUpper(), out decimal basePrice))
            {
                basePrice = 100m;
            }

            int daysFromEpoch = (int)(date.Date - new DateTime(2020, 1, 1)).TotalDays;

            decimal trend = basePrice * (decimal)(Math.Pow(1.0001, daysFromEpoch));

            decimal seasonal = (decimal)Math.Sin(daysFromEpoch / 30.0) * (basePrice * 0.1m);

            var random = new Random(date.GetHashCode());
            decimal volatility = basePrice * (decimal)(random.NextDouble() - 0.5) * 0.05m;

            decimal cycle = (decimal)Math.Sin(daysFromEpoch / 365.0) * (basePrice * 0.3m);

            decimal price = Math.Max(0.01m, basePrice + trend + seasonal + volatility + cycle);

            return price;
        }

        public async Task<decimal> GetLatestPriceAsync (string symbol)
        {
            return await GetHistoricalPriceAsync(symbol, DateTime.Today);
        }

        public async Task<List<CryptocurrencyInfo>> GetAllCryptocurrenciesAsync (int limit = 100)
        {
            var mockCryptos = new List<CryptocurrencyInfo>
            {
                new CryptocurrencyInfo { Symbol = "BTC", Name = "Bitcoin", Price = 45000m, MarketCap = 850000000000m, PercentChange24h = 2.5m, Volume24h = 25000000000m },
                new CryptocurrencyInfo { Symbol = "ETH", Name = "Ethereum", Price = 3200m, MarketCap = 380000000000m, PercentChange24h = 1.8m, Volume24h = 18000000000m },
                new CryptocurrencyInfo { Symbol = "SOL", Name = "Solana", Price = 95m, MarketCap = 42000000000m, PercentChange24h = 5.2m, Volume24h = 2800000000m },
                new CryptocurrencyInfo { Symbol = "XRP", Name = "XRP", Price = 0.58m, MarketCap = 32000000000m, PercentChange24h = -1.2m, Volume24h = 1500000000m },
                new CryptocurrencyInfo { Symbol = "USDC", Name = "USD Coin", Price = 1.00m, MarketCap = 28000000000m, PercentChange24h = 0.0m, Volume24h = 8000000000m },
                new CryptocurrencyInfo { Symbol = "TOKERO", Name = "Tokero", Price = 0.016m, MarketCap = 16000000m, PercentChange24h = 12.5m, Volume24h = 500000m },
                new CryptocurrencyInfo { Symbol = "ADA", Name = "Cardano", Price = 0.52m, MarketCap = 18000000000m, PercentChange24h = 3.1m, Volume24h = 1200000000m },
                new CryptocurrencyInfo { Symbol = "DOT", Name = "Polkadot", Price = 7.2m, MarketCap = 9000000000m, PercentChange24h = 4.2m, Volume24h = 800000000m },
                new CryptocurrencyInfo { Symbol = "LINK", Name = "Chainlink", Price = 15.8m, MarketCap = 9000000000m, PercentChange24h = 2.8m, Volume24h = 700000000m },
                new CryptocurrencyInfo { Symbol = "UNI", Name = "Uniswap", Price = 8.5m, MarketCap = 5000000000m, PercentChange24h = 6.1m, Volume24h = 400000000m },
                new CryptocurrencyInfo { Symbol = "AVAX", Name = "Avalanche", Price = 22m, MarketCap = 8000000000m, PercentChange24h = 8.3m, Volume24h = 600000000m },
                new CryptocurrencyInfo { Symbol = "MATIC", Name = "Polygon", Price = 0.85m, MarketCap = 8000000000m, PercentChange24h = 5.7m, Volume24h = 500000000m },
                new CryptocurrencyInfo { Symbol = "ATOM", Name = "Cosmos", Price = 10.5m, MarketCap = 4000000000m, PercentChange24h = 3.9m, Volume24h = 300000000m },
                new CryptocurrencyInfo { Symbol = "NEAR", Name = "NEAR Protocol", Price = 3.2m, MarketCap = 3000000000m, PercentChange24h = 7.4m, Volume24h = 200000000m },
                new CryptocurrencyInfo { Symbol = "FTM", Name = "Fantom", Price = 0.35m, MarketCap = 1000000000m, PercentChange24h = 9.2m, Volume24h = 150000000m },
                new CryptocurrencyInfo { Symbol = "ALGO", Name = "Algorand", Price = 0.32m, MarketCap = 2000000000m, PercentChange24h = 4.8m, Volume24h = 180000000m },
                new CryptocurrencyInfo { Symbol = "VET", Name = "VeChain", Price = 0.025m, MarketCap = 2000000000m, PercentChange24h = 6.3m, Volume24h = 120000000m },
                new CryptocurrencyInfo { Symbol = "THETA", Name = "Theta Network", Price = 1.1m, MarketCap = 1000000000m, PercentChange24h = 8.7m, Volume24h = 80000000m },
                new CryptocurrencyInfo { Symbol = "FIL", Name = "Filecoin", Price = 5.2m, MarketCap = 2000000000m, PercentChange24h = 2.1m, Volume24h = 150000000m },
                new CryptocurrencyInfo { Symbol = "TRX", Name = "TRON", Price = 0.055m, MarketCap = 5000000000m, PercentChange24h = 1.9m, Volume24h = 400000000m },
                new CryptocurrencyInfo { Symbol = "EOS", Name = "EOS", Price = 1.05m, MarketCap = 1000000000m, PercentChange24h = 3.4m, Volume24h = 120000000m },
                new CryptocurrencyInfo { Symbol = "AAVE", Name = "Aave", Price = 210m, MarketCap = 3000000000m, PercentChange24h = 5.6m, Volume24h = 250000000m }
            };

            return mockCryptos.Take(limit).ToList();
        }
    }
}