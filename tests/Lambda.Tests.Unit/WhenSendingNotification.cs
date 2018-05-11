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

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        private Function CreateSut()
        {
            return new Function();
        }
    }
}