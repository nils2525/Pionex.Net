using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace Pionex.Net.Interfaces.Clients.SpotApi
{
    /// <summary>
    /// Pionex Spot API endpoints
    /// </summary>
    public interface IPionexRestClientSpotApi : IRestApiClient<PionexCredentials>, IDisposable
    {
        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IPionexRestClientSpotApiExchangeData" />
        public IPionexRestClientSpotApiExchangeData ExchangeData { get; }
    }
}