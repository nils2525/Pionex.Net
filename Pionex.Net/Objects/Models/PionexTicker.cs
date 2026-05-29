using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace Pionex.Net.Objects.Models
{
    /// <summary>
    /// 24 hour ticker
    /// </summary>
    public record PionexTicker
    {
        /// <summary>
        /// ["<c>symbol</c>"] Symbol name
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>time</c>"] Ticker time
        /// </summary>
        [JsonPropertyName("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Time { get; set; }
        /// <summary>
        /// ["<c>open</c>"] Open price
        /// </summary>
        [JsonPropertyName("open")]
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? OpenPrice { get; set; }
        /// <summary>
        /// ["<c>close</c>"] Close price
        /// </summary>
        [JsonPropertyName("close")]
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? ClosePrice { get; set; }
        /// <summary>
        /// ["<c>high</c>"] High price
        /// </summary>
        [JsonPropertyName("high")]
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? HighPrice { get; set; }
        /// <summary>
        /// ["<c>low</c>"] Low price
        /// </summary>
        [JsonPropertyName("low")]
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? LowPrice { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] Base volume
        /// </summary>
        [JsonPropertyName("volume")]
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? Volume { get; set; }
        /// <summary>
        /// ["<c>amount</c>"] Quote volume
        /// </summary>
        [JsonPropertyName("amount")]
        [JsonConverter(typeof(DecimalConverter))]
        public decimal? QuoteVolume { get; set; }
        /// <summary>
        /// ["<c>count</c>"] Trade count
        /// </summary>
        [JsonPropertyName("count")]
        public long? Trades { get; set; }
    }
}
