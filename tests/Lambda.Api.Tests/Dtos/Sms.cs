using Newtonsoft.Json;

namespace Lambda.Api.Tests.Dtos
{
    public class Sms
    {
        private readonly string _body;
        private readonly string _to;
        private readonly string _type;

        [JsonConstructor]
        internal Sms(
            string to,
            string body,
            string type)
        {
            _to = to;
            _body = body;
            _type = type;
        }
    }
}