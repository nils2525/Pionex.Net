namespace Pionex.Net.Objects
{
    /// <summary>
    /// Api addresses
    /// </summary>
    public class PionexApiAddresses
    {
        /// <summary>
        /// The address used by the PionexRestClient for the Spot API
        /// </summary>
        public string RestClientSpotAddress { get; set; } = string.Empty;

        /// <summary>
        /// The address used by the PionexSocketClient for the websocket Spot API
        /// </summary>
        public string SocketClientSpotAddress { get; set; } = string.Empty;

        /// <summary>
        /// The default addresses to connect to the Pionex API
        /// </summary>
        public static PionexApiAddresses Default { get; } = new PionexApiAddresses
        {
            RestClientSpotAddress = "https://api.pionex.com",
            SocketClientSpotAddress = "wss://ws.pionex.com/wsPub"
        };
    }
}