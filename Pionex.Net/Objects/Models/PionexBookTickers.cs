using System.Text.Json.Serialization;

namespace Pionex.Net.Objects.Models
{
    /// <summary>
    /// Book tickers
    /// </summary>
    public record PionexBookTickers
    {
        /// <summary>
        /// ["<c>tickers</c>"] Book tickers
        /// </summary>
        [JsonPropertyName("tickers")]
        public PionexBookTicker[] Tickers { get; set; } = [];
    }
}