using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default.Routing;

namespace Pionex.Net.Objects.Sockets
{
    internal class PionexQuery : Query<object>
    {
        public PionexQuery(PionexSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            ExpectsResponse = false;
            MessageRouter = MessageRouter.Create();
        }
    }
}