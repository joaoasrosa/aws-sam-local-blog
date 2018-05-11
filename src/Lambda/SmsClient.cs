using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Lambda
{
    internal class SmsClient
    {
        private readonly HttpMessageHandler _httpMessageHandler;
        private readonly SmsClientSettings _settings;

        internal SmsClient(
            SmsClientSettings settings,
            HttpMessageHandler httpMessageHandler)
        {
            _settings = settings;
            _httpMessageHandler = httpMessageHandler;
        }

        internal void Notify(
            Notification notification)
        {
            using (var httpClient = CreateHttpClient())
            {
                var sms = new Sms(notification);

                var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "/v1/messages"
                )
                {
                    Content = new StringContent(
                        sms.AsJson(),
                        Encoding.UTF8,
                        "application/json"
                    )
                };

                var result = httpClient.SendAsync(request).Result;

                result.Validate();
            }
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient(_httpMessageHandler)
            {
                BaseAddress = new Uri(_settings.Url)
            };

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic",
                    _settings.AsHttpClientAuthenticationHeader()
                );

            httpClient
                .DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json")
                );

            return httpClient;
        }
    }
}