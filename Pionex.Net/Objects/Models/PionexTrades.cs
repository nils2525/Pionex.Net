using System.Text.Json.Serialization;

namespace Pionex.Net.Objects.Models
{
    /// <summary>
    /// Trades
    /// </summary>
    public record PionexTrades
    {
        /// <summary>
        /// ["<c>trades</c>"] Trades
        /// </summary>
        [JsonPropertyName("trades")]
        public PionexTrade[] Trades { get; set; } = [];
    }
}