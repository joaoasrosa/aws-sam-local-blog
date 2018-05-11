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

        internal string Validate(out bool isValid)
        {
            isValid = true;

            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                isValid = false;
                return "Invalid Phone Number.";
            }

            if (string.IsNullOrWhiteSpace(DisplayName))
            {
                isValid = false;
                return "Invalid Display Name.";
            }

            return string.Empty;
        }
    }
}