namespace Lambda
{
    internal static class NotificationExtensions
    {
        internal static string Validate(
            this Notification notification,
            out bool isValid)
        {
            isValid = true;

            if (notification == null)
            {
                isValid = false;
                return "The Notification is null.";
            }

            if (string.IsNullOrWhiteSpace(notification.PhoneNumber))
            {
                isValid = false;
                return "Invalid Phone Number.";
            }

            if (string.IsNullOrWhiteSpace(notification.DisplayName))
            {
                isValid = false;
                return "Invalid Display Name.";
            }

            return string.Empty;
        }
    }
}