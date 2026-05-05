using System.Text.Json.Serialization;

namespace Pionex.Net.Objects.Models
{
    /// <summary>
    /// Symbols
    /// </summary>
    public record PionexSymbols
    {
        /// <summary>
        /// ["<c>symbols</c>"] Symbols
        /// </summary>
        [JsonPropertyName("symbols")]
        public PionexSymbol[] Symbols { get; set; } = [];
    }
}