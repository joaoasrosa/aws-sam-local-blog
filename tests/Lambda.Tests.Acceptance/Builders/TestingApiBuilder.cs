namespace Lambda.Tests.Acceptance.Builders
{
    internal class TestingApiBuilder
    {
        private string _apiBehaviour;

        internal TestingApiBuilder WithInvalidCredentials()
        {
            _apiBehaviour = "invalid_credentials";
            return this;
        }

        internal Container Build()
        {
            return new Container(
                _apiBehaviour
            );
        }
    }
}