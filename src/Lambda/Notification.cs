using Newtonsoft.Json;

namespace Lambda
{
    internal class Notification
    {
        [JsonConstructor]
        internal Notification(
            string phoneNumber,
            string displayName)
        {
            PhoneNumber = phoneNumber;
            DisplayName = displayName;
        }

        internal string PhoneNumber { get; }
        internal string DisplayName { get; }
    }
}