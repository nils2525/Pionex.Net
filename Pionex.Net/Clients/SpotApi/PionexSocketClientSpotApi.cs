using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;
using Pionex.Net.Clients.MessageHandlers;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Models;
using Pionex.Net.Objects.Options;
using Pionex.Net.Objects.Sockets;
using Pionex.Net.Objects.Sockets.Subscriptions;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Clients.SpotApi
{
    /// <summary>
    /// Client providing access to the Pionex Spot websocket Api
    /// </summary>
    internal partial class PionexSocketClientSpotApi : SocketApiClient<PionexEnvironment, PionexAuthenticationProvider, PionexCredentials>, IPionexSocketClientSpotApi
    {
        #region fields
        private readonly PionexSocketClient _baseClient;
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal PionexSocketClientSpotApi(PionexSocketClient baseClient, ILoggerFactory? loggerFactory, PionexSocketOptions options) :
            base(loggerFactory, PionexExchange.Metadata.Id, options.Environment.SocketClientSpotAddress, options, options.SpotOptions)
        {
            _baseClient = baseClient;

            RateLimiter = PionexExchange.RateLimiter.PionexSocket;

            AddSystemSubscription(new PionexPingSubscription(_logger));
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PionexExchange._serializerContext);
        /// <inheritdoc />
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new PionexSocketMessageHandler();

        /// <inheritdoc />
        protected override bool HandleUnhandledMessage(SocketConnection connection, string typeIdentifier, ReadOnlySpan<byte> data)
        {
            if (typeIdentifier == "TRADE"
                && (connection.Status is SocketStatus.Closing or SocketStatus.Closed or SocketStatus.Disposed
                    || connection.Subscriptions.Any(s => s.Status is SubscriptionStatus.Closing or SubscriptionStatus.Closed)))
                return true;

            return base.HandleUnhandledMessage(connection, typeIdentifier, data);
        }

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => PionexExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

        /// <inheritdoc />
        protected override PionexAuthenticationProvider CreateAuthenticationProvider(PionexCredentials credentials)
            => new PionexAuthenticationProvider(credentials);

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(string symbol, Action<DataEvent<PionexTrade[]>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, PionexSocketEvent<PionexTrade[]>>((receiveTime, originalData, data) =>
            {
                if (data.Data == null || data.Data.Length == 0)
                    return;

                UpdateTimeOffset(data.Timestamp);

                onMessage(
                    new DataEvent<PionexTrade[]>(PionexExchange.Metadata.Id, data.Data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        .WithStreamId(data.Topic)
                        .WithSymbol(data.Symbol)
                        .WithDataTimestamp(data.Timestamp, GetTimeOffset())
                    );
            });

            var subscription = new PionexSubscription<PionexSocketEvent<PionexTrade[]>>(_logger, "TRADE", symbol, internalHandler, false);
            return await SubscribeAsync(BaseAddress, subscription, ct).ConfigureAwait(false);
        }
    }
}
