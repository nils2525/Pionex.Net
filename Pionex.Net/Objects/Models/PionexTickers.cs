using System.Text.Json.Serialization;

namespace Pionex.Net.Objects.Models
{
    /// <summary>
    /// Tickers
    /// </summary>
    public record PionexTickers
    {
        /// <summary>
        /// ["<c>tickers</c>"] Tickers
        /// </summary>
        [JsonPropertyName("tickers")]
        public PionexTicker[] Tickers { get; set; } = [];
    }
}