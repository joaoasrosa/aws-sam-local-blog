using System.Net;

namespace Lambda.Tests.Unit
{
    public class WhenSendingNotification
    {
        public void GivenNullNotification_ThenReturnsBadRequest()
        {
            var lambdaContext = Given.LambdaContext.Build();
            
            var sut = CreateSut();

            var result = sut.Handler(null, null);

            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        private Function CreateSut()
        {
            return new Function();
        }
    }
}