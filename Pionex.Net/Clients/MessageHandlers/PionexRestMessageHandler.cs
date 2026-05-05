using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using Pionex.Net.Objects.Internal;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pionex.Net.Clients.MessageHandlers
{
    internal class PionexRestMessageHandler : JsonRestMessageHandler
    {
        private readonly ErrorMapping _errorMapping;

        public override JsonSerializerOptions Options { get; } = PionexExchange._serializerContext;

        public PionexRestMessageHandler(ErrorMapping errorMapping)
        {
            _errorMapping = errorMapping;
        }

        public override Error? CheckDeserializedResponse<T>(HttpResponseHeaders responseHeaders, T result)
        {
            if (result is not PionexResult pionexResult)
                return null;

            if (pionexResult.Result)
                return null;

            var code = pionexResult.Code ?? "0";
            return new ServerError(code, _errorMapping.GetErrorInfo(code, pionexResult.Message));
        }

        public override async ValueTask<Error> ParseErrorResponse(
            int httpStatusCode,
            HttpResponseHeaders responseHeaders,
            Stream responseStream)
        {
            var (error, document) = await GetJsonDocument(responseStream).ConfigureAwait(false);
            if (error != null)
                return error;

            var code = document!.RootElement.TryGetProperty("code", out var codeProp) ? codeProp.GetString() ?? httpStatusCode.ToString() : httpStatusCode.ToString();
            string? message = document.RootElement.TryGetProperty("message", out var messageProp) ? messageProp.GetString() : null;
            if (message == null)
                return new ServerError(ErrorInfo.Unknown);

            return new ServerError(code, _errorMapping.GetErrorInfo(code, message));
        }
    }
}