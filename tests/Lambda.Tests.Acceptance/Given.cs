using Lambda.Tests.Acceptance.Builders;

namespace Lambda.Tests.Acceptance
{
    internal static class Given
    {
        internal static Container TestingApiConfiguredForInvalidCredentials()
        {
            var container = Docker
                .TestingApi
                .WithInvalidCredentials()
                .Build();

            container.Start();
            
            return container;
        }
        
        internal static Container TestingApiConfiguredForInsufficientCredits()
        {
            var container = Docker
                .TestingApi
                .WithInsufficientCredits()
                .Build();

            container.Start();
            
            return container;
        }

        internal static ApiGateway ApiGatewayIsRunning()
        {
            var apiGateway = SamLocal
                .ApiGateway
                .Build();

            apiGateway.Start();

            return apiGateway;
        }

        internal static NotificationBuilder Notification => new NotificationBuilder();
    }
}