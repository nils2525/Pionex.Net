using Pionex.Net.Enums;
using System.Text.Json.Serialization;

namespace Pionex.Net.Objects.Models
{
    /// <summary>
    /// Symbol info
    /// </summary>
    public record PionexSymbol
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>name</c>"] Display name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>type</c>"] Market type
        /// </summary>
        [JsonPropertyName("type")]
        public MarketType Type { get; set; }
        /// <summary>
        /// ["<c>baseCurrency</c>"] Base asset
        /// </summary>
        [JsonPropertyName("baseCurrency")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quoteCurrency</c>"] Quote asset
        /// </summary>
        [JsonPropertyName("quoteCurrency")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>basePrecision</c>"] Base precision
        /// </summary>
        [JsonPropertyName("basePrecision")]
        public int BasePrecision { get; set; }
        /// <summary>
        /// ["<c>quotePrecision</c>"] Quote precision
        /// </summary>
        [JsonPropertyName("quotePrecision")]
        public int QuotePrecision { get; set; }
        /// <summary>
        /// ["<c>amountPrecision</c>"] Amount precision
        /// </summary>
        [JsonPropertyName("amountPrecision")]
        public int AmountPrecision { get; set; }
        /// <summary>
        /// ["<c>minNotional</c>"] Minimum notional value
        /// </summary>
        [JsonPropertyName("minNotional")]
        public decimal MinNotional { get; set; }
        /// <summary>
        /// ["<c>minAmount</c>"] Minimum quote amount
        /// </summary>
        [JsonPropertyName("minAmount")]
        public decimal MinAmount { get; set; }
        /// <summary>
        /// ["<c>minTradeSize</c>"] Minimum trade quantity
        /// </summary>
        [JsonPropertyName("minTradeSize")]
        public decimal MinTradeSize { get; set; }
        /// <summary>
        /// ["<c>maxTradeSize</c>"] Maximum trade quantity
        /// </summary>
        [JsonPropertyName("maxTradeSize")]
        public decimal MaxTradeSize { get; set; }
        /// <summary>
        /// ["<c>enable</c>"] Whether trading is enabled
        /// </summary>
        [JsonPropertyName("enable")]
        public bool Enabled { get; set; }
        /// <summary>
        /// ["<c>contractType</c>"] Contract type for futures symbols
        /// </summary>
        [JsonPropertyName("contractType")]
        public string? ContractType { get; set; }
        /// <summary>
        /// ["<c>baseStep</c>"] Base quantity step for futures symbols
        /// </summary>
        [JsonPropertyName("baseStep")]
        public decimal? BaseStep { get; set; }
        /// <summary>
        /// ["<c>quoteStep</c>"] Price step for futures symbols
        /// </summary>
        [JsonPropertyName("quoteStep")]
        public decimal? QuoteStep { get; set; }
        /// <summary>
        /// ["<c>minSizeLimit</c>"] Minimum order size for futures symbols
        /// </summary>
        [JsonPropertyName("minSizeLimit")]
        public decimal? MinSizeLimit { get; set; }
        /// <summary>
        /// ["<c>maxSizeLimit</c>"] Maximum order size for futures symbols
        /// </summary>
        [JsonPropertyName("maxSizeLimit")]
        public decimal? MaxSizeLimit { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Trading status for futures symbols
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
