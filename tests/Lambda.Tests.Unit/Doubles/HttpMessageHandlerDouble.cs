using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lambda.Tests.Unit.Doubles
{
    internal class HttpMessageHandlerDouble : HttpMessageHandler
    {
        private readonly HttpStatusCode _httpStatusCode;

        public HttpMessageHandlerDouble(
            HttpStatusCode httpStatusCode)
        {
            _httpStatusCode = httpStatusCode;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new HttpResponseMessage
                {
                    StatusCode = _httpStatusCode
                }
            );
        }
    }
}