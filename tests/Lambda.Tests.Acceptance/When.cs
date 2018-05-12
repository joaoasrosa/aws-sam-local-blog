using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Lambda.Tests.Acceptance
{
    internal static class When
    {
        internal static HttpResponseMessage SendTheNotification()
        {
            var notification = Given
                .Notification
                .Build();

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://127.0.0.1:3000");

                var httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Post,
                    "/"
                )
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(notification)
                    )
                };

                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );

                return httpClient.SendAsync(httpRequestMessage).Result;
            }
        }
    }
}