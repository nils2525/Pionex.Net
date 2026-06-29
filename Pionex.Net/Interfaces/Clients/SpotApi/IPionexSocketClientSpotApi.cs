using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Pionex.Net.Objects.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Pionex Spot streams
    /// </summary>
    public interface IPionexSocketClientSpotApi : ISocketApiClient<PionexCredentials>, IDisposable
    {
        /// <summary>
        /// Subscribe to trade updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://www.pionex.com/docs/api-docs/trade-websocket/public-stream" /><br />
        /// Endpoint:<br />
        /// wss://ws.pionex.com/wsPub (topic: TRADE)
        /// </para>
        /// </summary>
        /// <param name="symbol">The symbol to subscribe</param>
        /// <param name="onMessage">The event handler for the received data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <returns>A stream subscription. This stream subscription can be used to be notified when the socket is disconnected/reconnected</returns>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<PionexTrade[]>> onMessage, CancellationToken ct = default);
    }
}