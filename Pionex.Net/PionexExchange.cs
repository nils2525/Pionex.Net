using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.SharedApis;
using Pionex.Net.Converters;
using System;
using System.Text.Json;

namespace Pionex.Net
{
    /// <summary>
    /// Pionex exchange information and configuration
    /// </summary>
    public static class PionexExchange
    {
        /// <summary>
        /// Platform metadata
        /// </summary>
        public static PlatformInfo Metadata { get; } = new PlatformInfo(
                "Pionex",
                "Pionex",
                string.Empty,
                "https://www.pionex.com/",
                ["https://www.pionex.com/docs/api-docs/readme/change-log"],
                PlatformType.CryptoCurrencyExchange,
                CentralizationType.Centralized
                );

        internal static JsonSerializerOptions _serializerContext = SerializerOptions.WithConverters(JsonSerializerContextCache.GetOrCreate<PionexSourceGenerationContext>());

        /// <summary>
        /// Aliases for Pionex assets
        /// </summary>
        public static AssetAliasConfiguration AssetAliases { get; } = new AssetAliasConfiguration
        {
            Aliases = [
                new AssetAlias("USDT", SharedSymbol.UsdOrStable.ToUpperInvariant(), AliasType.OnlyToExchange)
            ]
        };

        /// <summary>
        /// Format a base and quote asset to a Pionex recognized symbol
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            baseAsset = AssetAliases.CommonToExchangeName(baseAsset.ToUpperInvariant());
            quoteAsset = AssetAliases.CommonToExchangeName(quoteAsset.ToUpperInvariant());

            return baseAsset + "_" + quoteAsset;
        }

        /// <summary>
        /// Rate limiter configuration for the Pionex API
        /// </summary>
        public static PionexRateLimiters RateLimiter { get; } = new PionexRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the Pionex API
    /// </summary>
    public class PionexRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent>? RateLimitTriggered;
        /// <summary>
        /// Event when the rate limit is updated. Note that it's only updated when a request is send, so there are no specific updates when the current usage is decaying.
        /// </summary>
        public event Action<RateLimitUpdateEvent>? RateLimitUpdated;

        internal PionexRateLimiters()
        {
            Initialize();
        }

        private void Initialize()
        {
            PionexRestIp = new RateLimitGate("Pionex IP")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Request), 10, TimeSpan.FromSeconds(1), RateLimitWindowType.Sliding));
            PionexSocket = new RateLimitGate("Pionex Socket")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new LimitItemTypeFilter(RateLimitItemType.Connection), 10, TimeSpan.FromMinutes(1), RateLimitWindowType.Sliding));

            PionexRestIp.RateLimitTriggered += x => RateLimitTriggered?.Invoke(x);
            PionexRestIp.RateLimitUpdated += x => RateLimitUpdated?.Invoke(x);
            PionexSocket.RateLimitTriggered += x => RateLimitTriggered?.Invoke(x);
            PionexSocket.RateLimitUpdated += x => RateLimitUpdated?.Invoke(x);
        }

        internal IRateLimitGate PionexSocket { get; private set; } = null!;
        internal IRateLimitGate PionexRestIp { get; private set; } = null!;
    }
}