﻿using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace Lambda
{
    public class Function
    {
        public APIGatewayProxyResponse Handler(
            Notification notification,
            ILambdaContext context)
        {
            var statusCode = (int) HttpStatusCode.InternalServerError;

            if (notification == null)
                statusCode = (int) HttpStatusCode.BadRequest;

            return new APIGatewayProxyResponse
            {
                StatusCode = statusCode
            };
        }
    }
}