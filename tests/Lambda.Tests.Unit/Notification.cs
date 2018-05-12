using Newtonsoft.Json;

namespace Lambda.Tests.Unit
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

        [JsonProperty]
        internal string PhoneNumber { get; }
        
        [JsonProperty]
        internal string DisplayName { get; }
    }
}