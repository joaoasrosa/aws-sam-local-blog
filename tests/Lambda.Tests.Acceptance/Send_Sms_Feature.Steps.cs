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

        public void Dispose()
        {
            _context
                .TestingApiContainer
                .Stop();
            
            _context
                .Gateway
                .Stop();
        }

        private void Invalid_Api_Credentials()
        {
            _context.SetTestingApiContainer(
                Given.TestingApiConfiguredForInvalidCredentials()
            );
            
            _context.SetApiGateway(
                Given.ApiGatewayIsRunning()
            );
        }

        private void Insufficient_Credits_For_User_Account()
        {
            _context.SetTestingApiContainer(
                Given.TestingApiConfiguredForInsufficientCredits()
            );

            _context.SetApiGateway(
                Given.ApiGatewayIsRunning()
            );
        }

        private void The_notification_is_valid()
        {
            _context.SetTestingApiContainer(
                Given.TestingApiConfiguredForNotificationIsValid()
            );

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

        private void The_Api_returns_Forbidden()
        {
            _context
                .SmsResult
                .StatusCode
                .Should()
                .Be(HttpStatusCode.Forbidden);
        }

        private void The_Api_returns_Ok()
        {
            _context
                .SmsResult
                .StatusCode
                .Should()
                .Be(HttpStatusCode.OK);
        }
    }
}