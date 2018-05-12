using Newtonsoft.Json;

namespace Lambda
{
    internal class ErrorResult
    {
        internal ErrorResult(
            string reason)
        {
            Reason = reason;
        }

        [JsonProperty("reason")] internal string Reason { get; }
    }
}