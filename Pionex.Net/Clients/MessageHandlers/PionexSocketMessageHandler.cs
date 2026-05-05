using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using Pionex.Net.Objects.Models;
using Pionex.Net.Objects.Sockets;
using System.Text.Json;

namespace Pionex.Net.Clients.MessageHandlers
{
    internal class PionexSocketMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = PionexExchange._serializerContext;

        public PionexSocketMessageHandler()
        {
            AddTopicMapping<PionexSocketEvent<PionexTrade[]>>(x => x.Symbol);
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("topic"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("topic")!,
            },
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("op"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("op")!,
            },
            new MessageTypeDefinition {
                Fields = [
                    new PropertyFieldReference("type"),
                ],
                TypeIdentifierCallback = x => x.FieldValue("type")!,
            }
        ];
    }
}