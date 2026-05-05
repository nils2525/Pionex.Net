using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;
using Pionex.Net.Objects.Sockets;
using System;

namespace Pionex.Net.Objects.Sockets.Subscriptions
{
    internal class PionexPingSubscription : SystemSubscription
    {
        public PionexPingSubscription(ILogger logger) : base(logger, false)
        {
            MessageRouter = MessageRouter.CreateWithoutTopicFilter<PionexPing>("PING", HandlePing);
        }

        private CallResult? HandlePing(SocketConnection connection, DateTime time, string? originalData, PionexPing ping)
        {
            var id = ExchangeHelpers.NextId();
            _ = connection.SendAsync(id, new PionexSocketRequest
            {
                Operation = "PONG",
                Timestamp = new DateTimeOffset(ping.Timestamp).ToUnixTimeMilliseconds()
            }, 0);

            return CallResult.SuccessResult;
        }
    }
}