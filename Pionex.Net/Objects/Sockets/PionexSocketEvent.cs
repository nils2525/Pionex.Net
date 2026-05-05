using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Text.Json.Serialization;

namespace Pionex.Net.Objects.Sockets
{
    /// <summary>
    /// Socket event
    /// </summary>
    public record PionexSocketEvent<T>
    {
        /// <summary>
        /// Topic
        /// </summary>
        [JsonPropertyName("topic")]
        public string Topic { get; set; } = string.Empty;
        /// <summary>
        /// Symbol name
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// Timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        public T Data { get; set; } = default!;
    }
}