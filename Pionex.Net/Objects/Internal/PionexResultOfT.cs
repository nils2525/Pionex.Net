using System.Text.Json.Serialization;

namespace Pionex.Net.Objects.Internal
{
    internal record PionexResult<T> : PionexResult
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}