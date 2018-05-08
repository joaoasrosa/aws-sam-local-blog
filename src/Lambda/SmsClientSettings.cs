using System;
using System.Text;

namespace Lambda
{
    internal class SmsClientSettings
    {
        private readonly string _password;
        private readonly string _username;

        internal SmsClientSettings(
            string url,
            string username,
            string password)
        {
            _username = username;
            _password = password;
            Url = url;
        }

        internal string Url { get; }

        internal string AsHttpClientAuthenticationHeader()
        {
            var byteArray = Encoding.ASCII.GetBytes($"{_username}:{_password}");
            return Convert.ToBase64String(byteArray);
        }
    }
}