using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Pionex.Net.Enums;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Clients.SpotApi
{
    /// <inheritdoc />
    internal class PionexRestClientSpotApiExchangeData : IPionexRestClientSpotApiExchangeData
    {
        private readonly PionexRestClientSpotApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal PionexRestClientSpotApiExchangeData(ILogger logger, PionexRestClientSpotApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Symbols

        /// <inheritdoc />
        public async Task<WebCallResult<PionexSymbol[]>> GetSymbolsAsync(IEnumerable<string>? symbols = null, MarketType? type = MarketType.Spot, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptionalCommaSeparated("symbols", symbols);
            parameters.AddOptionalEnum("type", type);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/common/symbols", PionexExchange.RateLimiter.PionexRestIp, 5, false);
            var result = await _baseClient.SendWrappedAsync<PionexSymbols>(request, parameters, ct).ConfigureAwait(false);
            return result.As<PionexSymbol[]>(result.Data?.Symbols);
        }

        #endregion

        #region Get Recent Trades

        /// <inheritdoc />
        public async Task<WebCallResult<PionexTrade[]>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/trades", PionexExchange.RateLimiter.PionexRestIp, 1, false);
            var result = await _baseClient.SendWrappedAsync<PionexTrades>(request, parameters, ct).ConfigureAwait(false);
            return result.As<PionexTrade[]>(result.Data?.Trades);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<WebCallResult<PionexOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
            parameters.AddOptional("limit", limit);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/depth", PionexExchange.RateLimiter.PionexRestIp, 1, false);
            return await _baseClient.SendWrappedAsync<PionexOrderBook>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<PionexTicker[]>> GetTickersAsync(string? symbol = null, MarketType? type = MarketType.Spot, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptionalEnum("type", type);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/tickers", PionexExchange.RateLimiter.PionexRestIp, 1, false);
            var result = await _baseClient.SendWrappedAsync<PionexTickers>(request, parameters, ct).ConfigureAwait(false);
            return result.As<PionexTicker[]>(result.Data?.Tickers);
        }

        #endregion

        #region Get Book Tickers

        /// <inheritdoc />
        public async Task<WebCallResult<PionexBookTicker[]>> GetBookTickersAsync(string? symbol = null, MarketType? type = MarketType.Spot, CancellationToken ct = default)
        {
            var parameters = new ParameterCollection();
            parameters.AddOptional("symbol", symbol);
            parameters.AddOptionalEnum("type", type);
            var request = _definitions.GetOrCreate(HttpMethod.Get, "/api/v1/market/bookTickers", PionexExchange.RateLimiter.PionexRestIp, 1, false);
            var result = await _baseClient.SendWrappedAsync<PionexBookTickers>(request, parameters, ct).ConfigureAwait(false);
            return result.As<PionexBookTicker[]>(result.Data?.Tickers);
        }

        #endregion
    }
}