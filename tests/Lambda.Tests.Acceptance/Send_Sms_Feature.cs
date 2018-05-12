using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.XUnit2;

[assembly: LightBddScope]

namespace Lambda.Tests.Acceptance
{
    [FeatureDescription(
        @"As a user
I want to send a SMS
So I can communicate")]
    public partial class Send_Sms_Feature
    {
        [Scenario]
        [Label("Invalid credentials")]
        public void Invalid_Credentials()
        {
            Runner.RunScenario(
                given => Invalid_Api_Credentials(),
                when => The_user_tries_to_send_an_sms(),
                then => The_Api_returns_Unauthorized()
            );
        }
    }
}