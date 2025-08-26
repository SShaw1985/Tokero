using System.Text.Json.Serialization;

namespace Tokero.Models
{
    public class CoinMarketCapResponse
    {
        [JsonPropertyName("status")]
        public Status Status { get; set; } = new();

        [JsonPropertyName("data")]
        public Dictionary<string, CryptocurrencyData> Data { get; set; } = new();
    }

    public class CryptocurrencyListResponse
    {
        [JsonPropertyName("status")]
        public Status Status { get; set; } = new();

        [JsonPropertyName("data")]
        public List<CryptocurrencyListingData> Data { get; set; } = new();
    }

    public class Status
    {
        [JsonPropertyName("timestamp")]
        public string Timestamp { get; set; } = string.Empty;

        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("error_message")]
        public string? ErrorMessage { get; set; }

        [JsonPropertyName("elapsed")]
        public int Elapsed { get; set; }

        [JsonPropertyName("credit_count")]
        public int CreditCount { get; set; }

        [JsonPropertyName("notice")]
        public string? Notice { get; set; }
    }

    public class CryptocurrencyData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("quote")]
        public QuoteData Quote { get; set; } = new();
    }

    public class CryptocurrencyListingData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonPropertyName("quote")]
        public QuoteData Quote { get; set; } = new();
    }

    public class PlatformData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonPropertyName("token_address")]
        public string TokenAddress { get; set; } = string.Empty;
    }

    public class QuoteData
    {
        [JsonPropertyName("USD")]
        public USDData USD { get; set; } = new();
    }

    public class USDData
    {
        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("volume_24h")]
        public decimal Volume24h { get; set; }

        [JsonPropertyName("volume_change_24h")]
        public decimal VolumeChange24h { get; set; }

        [JsonPropertyName("percent_change_1h")]
        public decimal PercentChange1h { get; set; }

        [JsonPropertyName("percent_change_24h")]
        public decimal PercentChange24h { get; set; }

        [JsonPropertyName("percent_change_7d")]
        public decimal PercentChange7d { get; set; }

        [JsonPropertyName("percent_change_30d")]
        public decimal PercentChange30d { get; set; }

        [JsonPropertyName("percent_change_60d")]
        public decimal PercentChange60d { get; set; }

        [JsonPropertyName("percent_change_90d")]
        public decimal PercentChange90d { get; set; }

        [JsonPropertyName("market_cap")]
        public decimal MarketCap { get; set; }

        [JsonPropertyName("market_cap_dominance")]
        public decimal MarketCapDominance { get; set; }

        [JsonPropertyName("fully_diluted_market_cap")]
        public decimal FullyDilutedMarketCap { get; set; }

        [JsonPropertyName("tvl")]
        public decimal? Tvl { get; set; }

        [JsonPropertyName("last_updated")]
        public string LastUpdated { get; set; } = string.Empty;
    }
}