using System;
using System.Net;
using System.Net.Http;

namespace Lambda
{
    internal static class HttpResponseMessageExtensions
    {
        internal static void Validate(
            this HttpResponseMessage httpResponseMessage)
        {
            switch (httpResponseMessage.StatusCode)
            {
                case HttpStatusCode.OK:
                    return;

                case HttpStatusCode.Unauthorized:
                    throw new InvalidCredentials();

                case HttpStatusCode.Forbidden:
                    throw new InsufficientCredits();

                case HttpStatusCode.Accepted:

                case HttpStatusCode.Ambiguous:

                case HttpStatusCode.BadGateway:

                case HttpStatusCode.BadRequest:

                case HttpStatusCode.Conflict:

                case HttpStatusCode.Continue:

                case HttpStatusCode.Created:

                case HttpStatusCode.ExpectationFailed:

                case HttpStatusCode.Found:

                case HttpStatusCode.GatewayTimeout:

                case HttpStatusCode.Gone:

                case HttpStatusCode.HttpVersionNotSupported:

                case HttpStatusCode.InternalServerError:

                case HttpStatusCode.LengthRequired:

                case HttpStatusCode.MethodNotAllowed:

                case HttpStatusCode.Moved:

                case HttpStatusCode.NoContent:

                case HttpStatusCode.NonAuthoritativeInformation:

                case HttpStatusCode.NotAcceptable:

                case HttpStatusCode.NotFound:

                case HttpStatusCode.NotImplemented:

                case HttpStatusCode.NotModified:

                case HttpStatusCode.PartialContent:

                case HttpStatusCode.PaymentRequired:

                case HttpStatusCode.PreconditionFailed:

                case HttpStatusCode.ProxyAuthenticationRequired:

                case HttpStatusCode.RedirectKeepVerb:

                case HttpStatusCode.RedirectMethod:

                case HttpStatusCode.RequestedRangeNotSatisfiable:

                case HttpStatusCode.RequestEntityTooLarge:

                case HttpStatusCode.RequestTimeout:

                case HttpStatusCode.RequestUriTooLong:

                case HttpStatusCode.ResetContent:

                case HttpStatusCode.ServiceUnavailable:

                case HttpStatusCode.SwitchingProtocols:

                case HttpStatusCode.UnsupportedMediaType:

                case HttpStatusCode.Unused:

                case HttpStatusCode.UpgradeRequired:

                case HttpStatusCode.UseProxy:

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}