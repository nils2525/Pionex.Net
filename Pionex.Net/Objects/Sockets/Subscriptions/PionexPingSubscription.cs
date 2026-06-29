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
            MessageRouter = MessageRouter.CreateForEvent<PionexSocketOperation>(["PING", "CLOSE"], HandleSocketOperation);
        }

        private CallResult? HandleSocketOperation(SocketConnection connection, DateTime time, string? originalData, PionexSocketOperation message)
        {
            if (message.Operation == "PING")
            {
                var id = ExchangeHelpers.NextId();
                _ = connection.SendAsync(id, new PionexSocketRequest
                {
                    Operation = "PONG",
                    Timestamp = new DateTimeOffset(message.Timestamp).ToUnixTimeMilliseconds()
                }, 0);
            }
            else if (message.Operation == "CLOSE")
            {
                if (message.Note?.Contains("rate limit", StringComparison.OrdinalIgnoreCase) == true)
                    _logger.LogWarning("Server requested socket reconnect due to rate limiting");
                else
                    _logger.LogWarning("Server requested socket reconnect. Note: {Note}", message.Note);

                _ = connection.TriggerReconnectAsync();
            }

            return CallResult.Ok();
        }
    }
}
