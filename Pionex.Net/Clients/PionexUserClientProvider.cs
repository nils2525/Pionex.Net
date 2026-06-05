using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pionex.Net.Interfaces.Clients;
using Pionex.Net.Objects.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace Pionex.Net.Clients
{
    /// <inheritdoc />
    public class PionexUserClientProvider : IPionexUserClientProvider
    {
        private static readonly ConcurrentDictionary<string, IPionexRestClient> _restClients = new ConcurrentDictionary<string, IPionexRestClient>();
        private static readonly ConcurrentDictionary<string, IPionexSocketClient> _socketClients = new ConcurrentDictionary<string, IPionexSocketClient>();

        private readonly IOptions<PionexRestOptions> _restOptions;
        private readonly IOptions<PionexSocketOptions> _socketOptions;
        private readonly HttpClient _httpClient;
        private readonly ILoggerFactory? _loggerFactory;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public PionexUserClientProvider(Action<PionexOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public PionexUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<PionexRestOptions> restOptions,
            IOptions<PionexSocketOptions> socketOptions)
        {
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.Timeout = restOptions.Value.RequestTimeout;
            _loggerFactory = loggerFactory;
            _restOptions = restOptions;
            _socketOptions = socketOptions;
        }

        /// <inheritdoc />
        public void InitializeUserClient(string userIdentifier, PionexCredentials credentials, PionexEnvironment? environment = null)
        {
            CreateRestClient(userIdentifier, credentials, environment);
            CreateSocketClient(userIdentifier, credentials, environment);
        }

        /// <inheritdoc />
        public void ClearUserClients(string userIdentifier)
        {
            _restClients.TryRemove(userIdentifier, out _);
            _socketClients.TryRemove(userIdentifier, out _);
        }

        /// <inheritdoc />
        public IPionexRestClient GetRestClient(string userIdentifier, PionexCredentials? credentials = null, PionexEnvironment? environment = null)
        {
            if (!_restClients.TryGetValue(userIdentifier, out var client) || client.Disposed)
                client = CreateRestClient(userIdentifier, credentials, environment);

            return client;
        }

        /// <inheritdoc />
        public IPionexSocketClient GetSocketClient(string userIdentifier, PionexCredentials? credentials = null, PionexEnvironment? environment = null)
        {
            if (!_socketClients.TryGetValue(userIdentifier, out var client) || client.Disposed)
                client = CreateSocketClient(userIdentifier, credentials, environment);

            return client;
        }

        private IPionexRestClient CreateRestClient(string userIdentifier, PionexCredentials? credentials, PionexEnvironment? environment)
        {
            var clientRestOptions = SetRestEnvironment(environment);
            var client = new PionexRestClient(_httpClient, _loggerFactory, clientRestOptions);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _restClients[userIdentifier] = client;
            }
            return client;
        }

        private IPionexSocketClient CreateSocketClient(string userIdentifier, PionexCredentials? credentials, PionexEnvironment? environment)
        {
            var clientSocketOptions = SetSocketEnvironment(environment);
            var client = new PionexSocketClient(clientSocketOptions, _loggerFactory);
            if (credentials != null)
            {
                client.SetApiCredentials(credentials);
                _socketClients[userIdentifier] = client;
            }
            return client;
        }

        private IOptions<PionexRestOptions> SetRestEnvironment(PionexEnvironment? environment)
        {
            if (environment == null)
                return _restOptions;

            var newRestClientOptions = new PionexRestOptions();
            _restOptions.Value.Set(newRestClientOptions);
            newRestClientOptions.Environment = environment;
            return Options.Create(newRestClientOptions);
        }

        private IOptions<PionexSocketOptions> SetSocketEnvironment(PionexEnvironment? environment)
        {
            if (environment == null)
                return _socketOptions;

            var newSocketClientOptions = new PionexSocketOptions();
            _socketOptions.Value.Set(newSocketClientOptions);
            newSocketClientOptions.Environment = environment;
            return Options.Create(newSocketClientOptions);
        }

        private static T ApplyOptionsDelegate<T>(Action<T>? del) where T : new()
        {
            var opts = new T();
            del?.Invoke(opts);
            return opts;
        }
    }
}
