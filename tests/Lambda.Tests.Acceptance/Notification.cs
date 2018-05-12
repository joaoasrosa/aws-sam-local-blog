using Newtonsoft.Json;

namespace Lambda.Tests.Acceptance
{
    internal class Notification
    {
        internal Notification(
            string phoneNumber,
            string displayName)
        {
            PhoneNumber = phoneNumber;
            DisplayName = displayName;
        }

        [JsonProperty("phoneNumber")]
        internal string PhoneNumber { get; }

        [JsonProperty("displayName")]
        internal string DisplayName { get; }
    }
}