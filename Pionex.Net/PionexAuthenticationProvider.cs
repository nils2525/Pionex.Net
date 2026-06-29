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
            if (!requestConfig.RequestDefinition.Authenticated)
                return;

            var timestamp = GetMillisecondTimestampLong(apiClient);
            requestConfig.QueryParameters ??= new Parameters(PionexExchange._parameterSerializationSettings);
            requestConfig.QueryParameters["timestamp"] = timestamp;

            var orderedParameters = new Parameters(new ParameterSerializationSettings
            {
                Decimal = DecimalSerialization.String,
                Array = ArrayParametersSerialization.MultipleValues,
                Sort = true
            });
            foreach (var parameter in requestConfig.QueryParameters.OrderBy(p => p.Key))
                orderedParameters.Add(parameter.Key, parameter.Value);
            requestConfig.QueryParameters = orderedParameters;

            var queryString = requestConfig.GetQueryString(false);
            var pathUrl = string.IsNullOrEmpty(queryString) ? requestConfig.RequestDefinition.Path : requestConfig.RequestDefinition.Path + "?" + queryString;
            requestConfig.SetQueryString(queryString);

            var signString = requestConfig.RequestDefinition.Method.Method.ToUpperInvariant() + pathUrl;
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
