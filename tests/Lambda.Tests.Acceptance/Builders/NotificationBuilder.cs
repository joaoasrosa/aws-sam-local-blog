namespace Lambda.Tests.Acceptance.Builders
{
    internal class NotificationBuilder
    {
        internal Notification Build()
        {
            return new Notification(
                "123456789",
                "João Rosa"
            );
        }
    }
}