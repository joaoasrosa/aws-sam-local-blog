using System.Net;
using System.Net.Http;
using Lambda.Tests.Unit.Doubles;

namespace Lambda.Tests.Unit.Builders
{
    internal class HttpMesageHandlerBuilder
    {
        private HttpStatusCode _httpStatusCode;

        public HttpMesageHandlerBuilder WithHttpStatusCode(
            HttpStatusCode httpStatusCode)
        {
            _httpStatusCode = httpStatusCode;
            return this;
        }

        public HttpMessageHandler Build()
        {
            return new HttpMessageHandlerDouble(
                _httpStatusCode);
        }
    }
}