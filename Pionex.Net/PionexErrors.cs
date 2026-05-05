using CryptoExchange.Net.Objects.Errors;

namespace Pionex.Net
{
    internal static class PionexErrors
    {
        public static ErrorMapping RestErrors { get; } = new ErrorMapping(
            [
                new ErrorInfo(ErrorType.UnknownSymbol, false, "Invalid symbol", "INVALID_SYMBOL", "TRADE_INVALID_SYMBOL", "TRADE_INVAILD_SYMBOL"),
                new ErrorInfo(ErrorType.Unauthorized, false, "Unauthorized", "UNAUTHORIZED", "INVALID_SIGNATURE", "INVALID_API_KEY"),
                new ErrorInfo(ErrorType.RateLimitRequest, false, "Rate limit exceeded", "TOO_MANY_REQUESTS")
            ]);

        public static ErrorMapping SocketErrors { get; } = new ErrorMapping(
            [
                new ErrorInfo(ErrorType.UnknownSymbol, false, "Invalid symbol", "INVALID_SYMBOL")
            ]);
    }
}