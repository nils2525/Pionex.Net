using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pionex.Net.Clients.SpotApi;
using Pionex.Net.Interfaces.Clients;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Options;
using System;
using System.Net.Http;

namespace Pionex.Net.Clients
{
    /// <inheritdoc cref="IPionexRestClient" />
    public class PionexRestClient : BaseRestClient<PionexEnvironment, PionexCredentials>, IPionexRestClient
    {
        #region Api clients

        /// <inheritdoc />
        public IPionexRestClientSpotApi SpotApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of the PionexRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public PionexRestClient(Action<PionexRestOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate)))
        {
        }

        /// <summary>
        /// Create a new instance of the PionexRestClient using provided options
        /// </summary>
        /// <param name="httpClient">Http client for this client</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public PionexRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, IOptions<PionexRestOptions> options) : base(loggerFactory, "Pionex")
        {
            Initialize(options.Value);

            SpotApi = AddApiClient(new PionexRestClientSpotApi(this, loggerFactory, httpClient, options.Value));
        }

        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<PionexRestOptions> optionsDelegate)
        {
            PionexRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }
    }
}
