using Newtonsoft.Json;

namespace Lambda.Tests.Unit
{
    internal class ErrorResult
    {
        [JsonConstructor]
        internal ErrorResult(
            string reason)
        {
            Reason = reason;
        }

        internal string Reason { get; }
    }
}