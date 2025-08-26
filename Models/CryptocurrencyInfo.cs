namespace Tokero.Models
{
    public class CryptocurrencyInfo
    {
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal MarketCap { get; set; }
        public decimal PercentChange24h { get; set; }
        public decimal Volume24h { get; set; }
    }
}