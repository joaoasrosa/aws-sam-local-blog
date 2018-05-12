using Newtonsoft.Json;

namespace Lambda
{
    internal class Sms
    {
        internal Sms(
            Notification notification)
        {
            To = notification.PhoneNumber;
            Body = GenerateSmsBody(notification);
        }

        [JsonProperty("to")] 
        internal string To { get; }

        [JsonProperty("body")] 
        internal string Body { get; }

        [JsonProperty("type")] 
        internal string Type => "SMS";

        internal string AsJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        private static string GenerateSmsBody(
            Notification notification)
        {
            return
                $"Hi {notification.DisplayName}, we are doing a blog post using AWS Lambda. The ultimate goal is to test it locally!";
        }
    }
}