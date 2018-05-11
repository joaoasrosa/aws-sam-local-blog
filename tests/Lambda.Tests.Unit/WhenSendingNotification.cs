using System.Net;
using FluentAssertions;
using Xunit;

namespace Lambda.Tests.Unit
{
    public class WhenSendingNotification
    {
        [Fact]
        public void GivenNullNotification_ThenReturnsBadRequest()
        {
            var lambdaContext = Given.LambdaContext.Build();
            
            var sut = CreateSut();

            var result = sut.Handler(null, lambdaContext);

            result.StatusCode.Should().Be(
                (int)HttpStatusCode.BadRequest,
                "the notification is null");
        }
        
        [Fact]
        public void GivenNullNotification_ThenReturnsReason()
        {
            var lambdaContext = Given.LambdaContext.Build();
            
            var sut = CreateSut();

            var result = sut.Handler(null, lambdaContext);

            result.Body.Should().Be(
                "The Notification is null.",
                "the notification is null");
        }

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
                (int)HttpStatusCode.BadRequest,
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

            result.Body.Should().Be(
                "Invalid Phone Number.",
                "the phone number is invalid");
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
                (int)HttpStatusCode.BadRequest,
                "the display name is invalid");
        }

        private Function CreateSut()
        {
            return new Function();
        }
    }
}