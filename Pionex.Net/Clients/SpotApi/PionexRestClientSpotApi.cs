using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using Pionex.Net.Clients.MessageHandlers;
using Pionex.Net.Interfaces.Clients.SpotApi;
using Pionex.Net.Objects.Internal;
using Pionex.Net.Objects.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Pionex.Net.Clients.SpotApi
{
    /// <inheritdoc cref="IPionexRestClientSpotApi" />
    internal partial class PionexRestClientSpotApi : RestApiClient<PionexEnvironment, PionexAuthenticationProvider, PionexCredentials>, IPionexRestClientSpotApi
    {
        #region fields
        protected override ErrorMapping ErrorMapping => PionexErrors.RestErrors;

        /// <inheritdoc />
        public new PionexRestOptions ClientOptions => (PionexRestOptions)base.ClientOptions;

        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler { get; } = new PionexRestMessageHandler(PionexErrors.RestErrors);
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IPionexRestClientSpotApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public string ExchangeName => "Pionex";
        #endregion

        #region constructor/destructor
        internal PionexRestClientSpotApi(PionexRestClient baseClient, ILogger logger, HttpClient? httpClient, PionexRestOptions options)
            : base(logger, httpClient, options.Environment.RestClientSpotAddress, options, options.SpotOptions)
        {
            ExchangeData = new PionexRestClientSpotApiExchangeData(logger, this);

            StandardRequestHeaders = new Dictionary<string, string>
            {
                { "User-Agent", "CryptoExchange.Net/" + baseClient.CryptoExchangeLibVersion }
            };
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(PionexExchange._serializerContext);

        /// <inheritdoc />
        protected override PionexAuthenticationProvider CreateAuthenticationProvider(PionexCredentials credentials)
            => new PionexAuthenticationProvider(credentials);

        internal async Task<WebCallResult<T>> SendWrappedAsync<T>(RequestDefinition definition, ParameterCollection? parameters, CancellationToken cancellationToken, int? weight = null) where T : class
        {
            var result = await base.SendAsync<PionexResult<T>>(BaseAddress, definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result.As<T>(result.Data?.Data);
        }

        /// <inheritdoc />
        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
            => Task.FromResult(new WebCallResult<DateTime>(null, null, null, null, null, null, null, null, null, null, null, ResultDataSource.Server, DateTime.UtcNow, null));

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => PionexExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}