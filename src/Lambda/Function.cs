using System;
using System.Net;
using System.Net.Http;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using Serilog;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Lambda
{
    public class Function
    {
        private HttpMessageHandler _httpMessageHandler;

        public APIGatewayProxyResponse Handler(
            APIGatewayProxyRequest request,
            ILambdaContext context)
        {
            try
            {
                ConfigureLogger(context.AwsRequestId);

                var notification = JsonConvert.DeserializeObject<Notification>(
                    request.Body
                );
                
                Log.Logger.RecordNotification(
                    notification
                );
                
                var validationFailureReason = notification.Validate(out var isValid);

                if (isValid == false)
                {
                    var errorResult = new ErrorResult(
                        validationFailureReason);

                    Log.Logger.RecordValidationFailureReason(
                        errorResult
                    );
                    
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
                        Log.Logger.RecordInvalidCredentials();
                        break;
                    case InsufficientCredits _:
                        statusCode = HttpStatusCode.Forbidden;
                        Log.Logger.RecordInsufficientCredits();
                        break;
                    default:
                        Log.Logger.RecordException(exception);
                        break;
                }

                return new APIGatewayProxyResponse
                {
                    StatusCode = (int) statusCode
                };
            }
        }

        public void OverrideHttpMessageHandler(
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

        private static void ConfigureLogger(
            string requestId)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .Enrich.WithProperty("RequestId", requestId)
                .CreateLogger();
        }

        private static SmsClientSettings CreateSmsSettings()
        {
            return new SmsClientSettings(
                Environment.GetEnvironmentVariable("SMS_API_URL"),
                Environment.GetEnvironmentVariable("SMS_API_USERNAME"),
                Environment.GetEnvironmentVariable("SMS_API_PASSWORD")
            );
        }

        private static string CreateResponseBodyFromErrorResult(
            ErrorResult errorResult)
        {
            return JsonConvert.SerializeObject(errorResult);
        }
    }
}