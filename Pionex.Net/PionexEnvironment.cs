using CryptoExchange.Net.Objects;
using Pionex.Net.Objects;

namespace Pionex.Net
{
    /// <summary>
    /// Pionex environments
    /// </summary>
    public class PionexEnvironment : TradeEnvironment
    {
        /// <summary>
        /// Rest Spot API address
        /// </summary>
        public string RestClientSpotAddress { get; }

        /// <summary>
        /// Socket Spot API address
        /// </summary>
        public string SocketClientSpotAddress { get; }

        internal PionexEnvironment(
            string name,
            string restSpotAddress,
            string streamSpotAddress) :
            base(name)
        {
            RestClientSpotAddress = restSpotAddress;
            SocketClientSpotAddress = streamSpotAddress;
        }

        /// <summary>
        /// ctor for DI, use <see cref="CreateCustom"/> for creating a custom environment
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public PionexEnvironment() : base(TradeEnvironmentNames.Live)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        { }

        /// <summary>
        /// Get the Pionex environment by name
        /// </summary>
        public static PionexEnvironment? GetEnvironmentByName(string? name)
            => name switch
            {
                TradeEnvironmentNames.Live => Live,
                "" => Live,
                null => Live,
                _ => default
            };

        /// <summary>
        /// Available environment names
        /// </summary>
        /// <returns></returns>
        public static string[] All => [Live.Name];

        /// <summary>
        /// Live environment
        /// </summary>
        public static PionexEnvironment Live { get; }
            = new PionexEnvironment(TradeEnvironmentNames.Live,
                                    PionexApiAddresses.Default.RestClientSpotAddress,
                                    PionexApiAddresses.Default.SocketClientSpotAddress);

        /// <summary>
        /// Create a custom environment
        /// </summary>
        public static PionexEnvironment CreateCustom(
            string name,
            string spotRestAddress,
            string spotSocketStreamAddress)
            => new PionexEnvironment(name, spotRestAddress, spotSocketStreamAddress);
    }
}