using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using System.Collections.Generic;
using System.Linq;

namespace Pionex.Net
{
    internal class PionexAuthenticationProvider : AuthenticationProvider<PionexCredentials, PionexCredentials>
    {
        private static readonly IStringMessageSerializer _messageSerializer =
            new SystemTextJsonMessageSerializer(PionexExchange._serializerContext);

        public PionexAuthenticationProvider(PionexCredentials credentials) : base(credentials, credentials)
        {
        }

        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration requestConfig)
        {
            if (!requestConfig.Authenticated)
                return;

            var timestamp = GetMillisecondTimestampLong(apiClient);
            requestConfig.QueryParameters ??= new Dictionary<string, object>();
            requestConfig.QueryParameters["timestamp"] = timestamp;

            var orderedParameters = requestConfig.QueryParameters
                .OrderBy(p => p.Key)
                .ToDictionary(p => p.Key, p => p.Value);
            requestConfig.QueryParameters = orderedParameters;

            var queryString = requestConfig.GetQueryString(false);
            var pathUrl = string.IsNullOrEmpty(queryString) ? requestConfig.Path : requestConfig.Path + "?" + queryString;
            requestConfig.SetQueryString(queryString);

            var signString = requestConfig.Method.Method.ToUpperInvariant() + pathUrl;
            if (requestConfig.BodyParameters != null)
            {
                var body = GetSerializedBody(_messageSerializer, requestConfig.BodyParameters);
                signString += body;
                requestConfig.SetBodyContent(body);
            }

            var signature = SignHMACSHA256(signString, SignOutputType.Hex);

            requestConfig.Headers ??= new Dictionary<string, string>();
            requestConfig.Headers.Add("PIONEX-KEY", Key);
            requestConfig.Headers.Add("PIONEX-SIGNATURE", signature);
        }
    }
}