using System;
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
            throw new NotImplementedException();
        }
    }
}