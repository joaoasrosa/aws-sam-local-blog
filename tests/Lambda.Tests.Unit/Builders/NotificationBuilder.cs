using System;

namespace Lambda.Tests.Unit.Builders
{
    internal class NotificationBuilder
    {
        private string _displayName;
        private string _phoneNumber;

        internal NotificationBuilder()
        {
            _phoneNumber = Guid.NewGuid().ToString();
            _displayName = Guid.NewGuid().ToString();
        }

        internal NotificationBuilder WithPhoneNumber(
            string phoneNumber)
        {
            _phoneNumber = phoneNumber;
            return this;
        }

        internal NotificationBuilder WithDisplayName(
            string displayName)
        {
            _displayName = displayName;
            return this;
        }

        internal Notification Build()
        {
            return new Notification(
                _phoneNumber,
                _displayName
            );
        }
    }
}