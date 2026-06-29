using CryptoExchange.Net.Objects;
using Pionex.Net.Enums;
using Pionex.Net.Objects.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Pionex Spot exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IPionexRestClientSpotApiExchangeData
    {
        /// <summary>
        /// Get trading pair information
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/common" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/common/symbols
        /// </para>
        /// </summary>
        /// <param name="symbols">["<c>symbols</c>"] Filter by symbols, for example `BTC_USDT,ETH_USDT`</param>
        /// <param name="type">["<c>type</c>"] Market type</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexSymbol[]>> GetSymbolsAsync(IEnumerable<string>? symbols = null, MarketType? type = MarketType.Spot, CancellationToken ct = default);

        /// <summary>
        /// Get recent trades
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/market" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/market/trades
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `BTC_USDT`</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 500</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexTrade[]>> GetRecentTradesAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get order book snapshot
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/market" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/market/depth
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] The symbol, for example `BTC_USDT`</param>
        /// <param name="limit">["<c>limit</c>"] Max number of rows, max 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default);

        /// <summary>
        /// Get 24-hour price change statistics
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/market" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/market/tickers
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol, for example `BTC_USDT`</param>
        /// <param name="type">["<c>type</c>"] Market type</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexTicker[]>> GetTickersAsync(string? symbol = null, MarketType? type = MarketType.Spot, CancellationToken ct = default);

        /// <summary>
        /// Get best bid/ask prices
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-api/market" /><br />
        /// Endpoint:<br />
        /// GET /api/v1/market/bookTickers
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>symbol</c>"] Filter by symbol, for example `BTC_USDT`</param>
        /// <param name="type">["<c>type</c>"] Market type</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<PionexBookTicker[]>> GetBookTickersAsync(string? symbol = null, MarketType? type = MarketType.Spot, CancellationToken ct = default);
    }
}