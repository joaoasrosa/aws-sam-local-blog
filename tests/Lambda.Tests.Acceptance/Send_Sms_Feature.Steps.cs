using System;
using System.Net;
using FluentAssertions;
using LightBDD.XUnit2;

namespace Lambda.Tests.Acceptance
{
    public partial class Send_Sms_Feature : FeatureFixture, IDisposable
    {
        private readonly Context _context;

        public Send_Sms_Feature()
        {
            _context = new Context();
        }

        private void Invalid_Api_Credentials()
        {
            Given.TestingApiConfiguredForInvalidCredentials();

            _context.SetApiGateway(
                Given.ApiGatewayIsRunning()
            );
        }

        private void The_user_tries_to_send_an_sms()
        {
            _context.SetSmsResult(
                When.SendTheNotification()
            );
        }

        private void The_Api_returns_Unauthorized()
        {
            _context
                .SmsResult
                .StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        public void Dispose()
        {
            _context
                .Gateway
                .Stop();
        }
    }
}