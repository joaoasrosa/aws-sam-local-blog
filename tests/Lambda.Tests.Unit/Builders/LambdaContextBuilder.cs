using System;
using Amazon.Lambda.Core;
using Lambda.Tests.Unit.Doubles;

namespace Lambda.Tests.Unit.Builders
{
    internal class LambdaContextBuilder
    {
        private readonly string _awsRequestId;

        internal LambdaContextBuilder()
        {
            _awsRequestId = Guid.NewGuid().ToString();
        }

        internal ILambdaContext Build()
        {
            return new LambdaContextDouble(
                _awsRequestId);
        }
    }
}