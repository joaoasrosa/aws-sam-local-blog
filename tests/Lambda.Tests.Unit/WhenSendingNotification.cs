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

            var apiRequest = Given.APIGatewayProxyRequest
                .WithEvent(notification)
                .Build();

            var sut = CreateSut();

            var result = sut.Handler(apiRequest, lambdaContext);

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

            var apiRequest = Given.APIGatewayProxyRequest
                .WithEvent(notification)
                .Build();

            var sut = CreateSut();

            var result = sut.Handler(apiRequest, lambdaContext);

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

            var apiRequest = Given.APIGatewayProxyRequest
                .WithEvent(notification)
                .Build();

            var sut = CreateSut();

            var result = sut.Handler(apiRequest, lambdaContext);

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

            var apiRequest = Given.APIGatewayProxyRequest
                .WithEvent(notification)
                .Build();

            var sut = CreateSut();

            var result = sut.Handler(apiRequest, lambdaContext);

            var errorResult = JsonConvert.DeserializeObject<ErrorResult>(
                result.Body
            );

            errorResult.Reason.Should().Be(
                "Invalid Display Name.",
                "the display name is invalid"
            );
        }

        [Theory]
        [InlineData(null, "Bar")]
        [InlineData("", "Bar")]
        [InlineData("Foo", null)]
        [InlineData("Foo", "")]
        public void GivenInvalidSmsApiCredentials_ThenReturnsUnauthorized(
            string username,
            string password)
        {
            var lambdaContext = Given.LambdaContext
                .Build();

            var notification = Given.Notification
                .Build();

            var apiRequest = Given.APIGatewayProxyRequest
                .WithEvent(notification)
                .Build();

            var sut = CreateSut(
                HttpStatusCode.Unauthorized,
                username,
                password);

            var result = sut.Handler(apiRequest, lambdaContext);

            result.StatusCode.Should().Be(
                (int) HttpStatusCode.Unauthorized,
                "the client has provided invalid credentials");
        }

        [Fact]
        public void GivenInsufficientCredits_ThenReturnsForbidden()
        {
            var lambdaContext = Given.LambdaContext
                .Build();

            var notification = Given.Notification
                .Build();

            var apiRequest = Given.APIGatewayProxyRequest
                .WithEvent(notification)
                .Build();

            var sut = CreateSut(
                HttpStatusCode.Forbidden);

            var result = sut.Handler(apiRequest, lambdaContext);

            result.StatusCode.Should().Be(
                (int) HttpStatusCode.Forbidden,
                "the client has insufficient credits");
        }

        [Fact]
        public void GivenSmsIsSent_ThenReturnsOk()
        {
            var lambdaContext = Given.LambdaContext
                .Build();

            var notification = Given.Notification
                .Build();

            var apiRequest = Given.APIGatewayProxyRequest
                .WithEvent(notification)
                .Build();

            var sut = CreateSut(
                HttpStatusCode.OK);

            var result = sut.Handler(apiRequest, lambdaContext);

            result.StatusCode.Should().Be(
                (int) HttpStatusCode.OK,
                "the SMS was sent");
        }

        private static Function CreateSut()
        {
            return CreateSut(
                HttpStatusCode.OK,
                "https://joaorosa.io",
                "Foo",
                "Bar");
        }

        private static Function CreateSut(
            HttpStatusCode httpStatusCode)
        {
            return CreateSut(
                httpStatusCode,
                "https://joaorosa.io",
                "Foo",
                "Bar");
        }

        private static Function CreateSut(
            HttpStatusCode httpStatusCode,
            string username,
            string password)
        {
            return CreateSut(
                httpStatusCode,
                "https://joaorosa.io",
                username,
                password);
        }

        private static Function CreateSut(
            HttpStatusCode httpStatusCode,
            string url,
            string username,
            string password)
        {
            SetEnvironmentVariables(
                url,
                username,
                password);

            var httpMessageHandlerDouble = Given.HttpMesageHandler
                .WithHttpStatusCode(httpStatusCode)
                .Build();

            var function = new Function();
            function.OverrideHttpMessageHandler(httpMessageHandlerDouble);

            return function;
        }

        private static void SetEnvironmentVariables(
            string url,
            string username,
            string password)
        {
            Environment.SetEnvironmentVariable(
                "SMS_API_URL",
                url
            );
            Environment.SetEnvironmentVariable(
                "SMS_API_USERNAME",
                username
            );
            Environment.SetEnvironmentVariable(
                "SMS_API_PASSWORD",
                password
            );
        }
    }
}