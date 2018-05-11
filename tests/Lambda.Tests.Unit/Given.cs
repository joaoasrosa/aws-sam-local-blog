using Lambda.Tests.Unit.Builders;

namespace Lambda.Tests.Unit
{
    internal static class Given
    {
        internal static LambdaContextBuilder LambdaContext => new LambdaContextBuilder();
        internal static NotificationBuilder Notification => new NotificationBuilder();
        internal static HttpMesageHandlerBuilder HttpMesageHandler => new HttpMesageHandlerBuilder();
    }
}