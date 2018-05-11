using System;
using System.Net;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;

namespace Lambda.Tests.Unit
{
    public class WhenSendingNotification
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenNotificationPhoneNumberIsInvalid_ThenReturnsBadRequest(
            string phoneNumber)
        {
            var lambdaContext = Given.LambdaContext
                .Build();

            var notification = Given.Notification
                .WithPhoneNumber(phoneNumber)
                .Build();

            var sut = CreateSut();

            var result = sut.Handler(notification, lambdaContext);

            result.StatusCode.Should().Be(
                (int) HttpStatusCode.BadRequest,
                "the phone number is invalid");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenNotificationPhoneNumberIsInvalid_ThenReturnsReason(
            string phoneNumber)
        {
            var lambdaContext = Given.LambdaContext
                .Build();

            var notification = Given.Notification
                .WithPhoneNumber(phoneNumber)
                .Build();

            var sut = CreateSut();

            var result = sut.Handler(notification, lambdaContext);

            var errorResult = JsonConvert.DeserializeObject<ErrorResult>(
                result.Body
            );

            errorResult.Reason.Should().Be(
                "Invalid Phone Number.",
                "the phone number is invalid"
            );
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenNotificationDisplayNameIsInvalid_ThenReturnsBadRequest(
            string displayName)
        {
            var lambdaContext = Given.LambdaContext
                .Build();

            var notification = Given.Notification
                .WithDisplayName(displayName)
                .Build();

            var sut = CreateSut();

            var result = sut.Handler(notification, lambdaContext);

            result.StatusCode.Should().Be(
                (int) HttpStatusCode.BadRequest,
                "the display name is invalid");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenNotificationDisplayNameIsInvalid_ThenReturnsReason(
            string displayName)
        {
            var lambdaContext = Given.LambdaContext
                .Build();

            var notification = Given.Notification
                .WithDisplayName(displayName)
                .Build();

            var sut = CreateSut();

            var result = sut.Handler(notification, lambdaContext);
          
            var errorResult = JsonConvert.DeserializeObject<ErrorResult>(
                result.Body
            );

            errorResult.Reason.Should().Be(
                "Invalid Display Name.",
                "the display name is invalid"
            );
        }

        [Fact]
        public void GivenInvalidSmsApiCredentials_ThenReturnsUnauthorized()
        {
            var lambdaContext = Given.LambdaContext
                .Build();

            var notification = Given.Notification
                .Build();

            var sut = CreateSut(HttpStatusCode.Unauthorized);

            var result = sut.Handler(notification, lambdaContext);

            result.StatusCode.Should().Be(
                (int) HttpStatusCode.Unauthorized,
                "the client has provided invalid credentials");
        }

        [Fact]
        public void GivenNullNotification_ThenReturnsBadRequest()
        {
            var lambdaContext = Given.LambdaContext.Build();

            var sut = CreateSut();

            var result = sut.Handler(null, lambdaContext);

            result.StatusCode.Should().Be(
                (int) HttpStatusCode.BadRequest,
                "the notification is null");
        }

        [Fact]
        public void GivenNullNotification_ThenReturnsReason()
        {
            var lambdaContext = Given.LambdaContext.Build();

            var sut = CreateSut();

            var result = sut.Handler(null, lambdaContext);

            var errorResult = JsonConvert.DeserializeObject<ErrorResult>(
                result.Body
            );

            errorResult.Reason.Should().Be(
                "The Notification is null.",
                "the notification is null"
            );
        }

        private Function CreateSut()
        {
            return CreateSut(HttpStatusCode.OK);
        }

        private Function CreateSut(
            HttpStatusCode httpStatusCode)
        {
            SetEnvironmentVariables();

            var httpMessageHandlerDouble = Given.HttpMesageHandler
                .WithHttpStatusCode(httpStatusCode)
                .Build();

            var function = new Function();
            function.OverrideHttpMessageHandler(httpMessageHandlerDouble);

            return function;
        }

        private void SetEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable(
                "SMS_API_URL",
                "https://joaorosa.io"
            );
        }
    }
}