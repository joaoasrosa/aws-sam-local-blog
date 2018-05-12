using System.Net.Http;

namespace Lambda.Tests.Acceptance
{
    internal class Context
    {
        internal ApiGateway Gateway { get; private set; }
        internal HttpResponseMessage SmsResult { get; private set; }

        internal void SetSmsResult(
            HttpResponseMessage smsResult)
        {
            SmsResult = smsResult;
        }

        internal void SetApiGateway(
            ApiGateway apiGateway)
        {
            Gateway = apiGateway;
        }
    }
}