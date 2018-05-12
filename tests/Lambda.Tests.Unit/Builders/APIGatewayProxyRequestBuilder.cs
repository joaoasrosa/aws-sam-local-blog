using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;

namespace Lambda.Tests.Unit.Builders
{
    internal class APIGatewayProxyRequestBuilder
    {
        private object _event;

        internal APIGatewayProxyRequestBuilder WithEvent(
            object @event)
        {
            _event = @event;
            return this;
        }

        internal APIGatewayProxyRequest Build()
        {
            return new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(_event)
            };
        }
    }
}