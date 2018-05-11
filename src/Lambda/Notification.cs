using Newtonsoft.Json;

namespace Lambda
{
    public class Notification
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

        internal bool IsValid()
        {
            return string.IsNullOrWhiteSpace(PhoneNumber) == false;
        }
    }
}