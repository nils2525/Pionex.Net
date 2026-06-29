using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;
using Pionex.Net.Objects.Sockets;
using System;

namespace Pionex.Net.Objects.Sockets.Subscriptions
{
    /// <inheritdoc />
    internal class PionexSubscription<T> : Subscription
    {
        private readonly Action<DateTime, string?, T> _handler;
        private readonly string _symbol;
        private readonly string _topic;

        /// <summary>
        /// ctor
        /// </summary>
        public PionexSubscription(
            ILogger logger,
            string topic,
            string symbol,
            Action<DateTime, string?, T> handler,
            bool auth) : base(logger, auth)
        {
            _handler = handler;
            _topic = topic;
            _symbol = symbol;

            MessageRouter = MessageRouter.CreateForEvent<T>(topic, symbol, DoHandleMessage);
        }

        /// <inheritdoc />
        protected override Query? GetSubQuery(SocketConnection connection)
        {
            return new PionexQuery(new PionexSocketRequest
            {
                Operation = "SUBSCRIBE",
                Topic = _topic,
                Symbol = _symbol
            }, Authenticated);
        }

        /// <inheritdoc />
        protected override Query? GetUnsubQuery(SocketConnection connection)
        {
            return new PionexQuery(new PionexSocketRequest
            {
                Operation = "UNSUBSCRIBE",
                Topic = _topic,
                Symbol = _symbol
            }, Authenticated);
        }

        /// <inheritdoc />
        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, T message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            return CallResult.Ok();
        }
    }
}
