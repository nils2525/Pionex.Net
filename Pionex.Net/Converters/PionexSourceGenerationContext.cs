using Pionex.Net.Objects.Internal;
using Pionex.Net.Objects.Models;
using Pionex.Net.Objects.Sockets;
using System;
using System.Text.Json.Serialization;

namespace Pionex.Net.Converters
{
    [JsonSerializable(typeof(PionexResult<PionexSymbols>))]
    [JsonSerializable(typeof(PionexResult<PionexTickers>))]
    [JsonSerializable(typeof(PionexResult<PionexBookTickers>))]
    [JsonSerializable(typeof(PionexResult<PionexTrades>))]
    [JsonSerializable(typeof(PionexResult<PionexOrderBook>))]
    [JsonSerializable(typeof(PionexSocketEvent<PionexTrade[]>))]
    [JsonSerializable(typeof(PionexSocketError))]
    [JsonSerializable(typeof(PionexSocketRequest))]
    [JsonSerializable(typeof(PionexPing))]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(string[]))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(int?))]
    [JsonSerializable(typeof(long))]
    [JsonSerializable(typeof(long?))]
    [JsonSerializable(typeof(decimal))]
    [JsonSerializable(typeof(decimal?))]
    [JsonSerializable(typeof(DateTime))]
    [JsonSerializable(typeof(DateTime?))]
    internal partial class PionexSourceGenerationContext : JsonSerializerContext
    {
    }
}