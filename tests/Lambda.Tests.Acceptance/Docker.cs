using Lambda.Tests.Acceptance.Builders;

namespace Lambda.Tests.Acceptance
{
    internal static class Docker
    {
        internal static TestingApiBuilder TestingApi => new TestingApiBuilder();
    }
}