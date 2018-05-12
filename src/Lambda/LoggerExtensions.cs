using System;
using Newtonsoft.Json;
using Serilog;

namespace Lambda
{
    internal static class LoggerExtensions
    {
        
        internal static void RecordNotification(
            this ILogger logger,
            Notification notification)
        {
            logger.Information("The information for '{name}' with phone number '{phoneNumber}' will be sent.",
                notification?.DisplayName,
                notification?.PhoneNumber
            );
        }

        
        internal static void RecordValidationFailureReason(
            this ILogger logger,
            ErrorResult errorResult)
        {
            logger.Information("The failed the validation with '{errorResultReason}'.",
                errorResult.Reason
            );
        }
        
        internal static void RecordInvalidCredentials(
            this ILogger logger)
        {
            logger.Error("The credentials supplied are invalid.");
        }
        
        internal static void RecordInsufficientCredits(
            this ILogger logger)
        {
            logger.Error("The account doesn't have sufficient credits.");
        }
        
        internal static void RecordException(
            this ILogger logger,
            Exception exception)
        {
            if (logger == null)
                Console.Error.Write(JsonConvert.SerializeObject(exception));
            else
                logger.Fatal(exception, exception.Message);
        }
    }
}