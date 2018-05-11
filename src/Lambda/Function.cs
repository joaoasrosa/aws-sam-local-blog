using System;
using System.Net;
using System.Net.Http;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace Lambda
{
    public class Function
    {
        private HttpMessageHandler _httpMessageHandler;

        public APIGatewayProxyResponse Handler(
            Notification notification,
            ILambdaContext context)
        {
            try
            {
                var validationFailureReason = notification.Validate(out var isValid);

                if (isValid == false)
                {
                    var errorResult = new ErrorResult(
                        validationFailureReason);
                    
                    return new APIGatewayProxyResponse
                    {
                        StatusCode = (int) HttpStatusCode.BadRequest,
                        Body = CreateResponseBodyFromErrorResult(
                            errorResult)
                    };
                }

                var smsClient = CreateSmsClient();

                smsClient.Notify(notification);

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int) HttpStatusCode.OK
                };
            }
            catch (Exception exception)
            {
                var statusCode = HttpStatusCode.InternalServerError;
                switch (exception)
                {
                    case InvalidCredentials _:
                        statusCode = HttpStatusCode.Unauthorized;
                        break;
                    case InsufficientCredits _:
                        statusCode = HttpStatusCode.Forbidden;
                        break;
                }

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int) statusCode
                };
            }
        }

        internal void OverrideHttpMessageHandler(
            HttpMessageHandler httpMessageHandler)
        {
            _httpMessageHandler = httpMessageHandler;
        }

        private SmsClient CreateSmsClient()
        {
            var settings = CreateSmsSettings();
            var httpMessageHandler = _httpMessageHandler ?? new HttpClientHandler();

            return new SmsClient(
                settings,
                httpMessageHandler
            );
        }

        private SmsClientSettings CreateSmsSettings()
        {
            return new SmsClientSettings(
                Environment.GetEnvironmentVariable("SMS_API_URL"),
                Environment.GetEnvironmentVariable("SMS_API_USERNAME"),
                Environment.GetEnvironmentVariable("SMS_API_PASSWORD")
            );
        }

        private string CreateResponseBodyFromErrorResult(
            ErrorResult errorResult)
        {
            return JsonConvert.SerializeObject(errorResult);
        }
    }
}