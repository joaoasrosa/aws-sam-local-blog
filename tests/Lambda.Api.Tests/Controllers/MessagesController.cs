using System;
using System.Net;
using Lambda.Api.Tests.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Lambda.Api.Tests.Controllers
{
    public class MessagesController : Controller
    {
        [HttpPost("v1/messages")]
        public IActionResult Send(
            [FromBody] Sms sms)
        {
            return SmsResponseFactory();
        }

        private static IActionResult SmsResponseFactory()
        {
            switch (Environment.GetEnvironmentVariable("API_BEHAVIOUR"))
            {
                case "ok":
                    return new OkResult();
                case "invalid_credentials":
                    return new UnauthorizedResult();
                case "insufficient_credits":
                    return new ForbidResult();
                default:
                    return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}