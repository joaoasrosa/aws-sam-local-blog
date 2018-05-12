namespace Lambda.Tests.Acceptance.Builders
{
    internal class ApiGatewayBuilder
    {
        internal ApiGateway Build()
        {
            return new ApiGateway(
                "../../../../../local-deploy/");
        }
    }
}