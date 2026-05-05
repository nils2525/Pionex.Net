using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Pionex.Net.Enums
{
    /// <summary>
    /// Market type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<MarketType>))]
    public enum MarketType
    {
        /// <summary>
        /// ["<c>SPOT</c>"] Spot
        /// </summary>
        [Map("SPOT")]
        Spot,
        /// <summary>
        /// ["<c>PERP</c>"] Perpetual futures
        /// </summary>
        [Map("PERP")]
        Perpetual
    }
}