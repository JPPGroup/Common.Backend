using NUnit.Framework;

namespace Jpp.Common.Backend.IntegrationTests
{
    [SetUpFixture]
    class Common
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Backend.BASE_URL = TestContext.Parameters["BaseUrl"];
            Backend.UNATTENDED_AUTH_ENABLED = true;
        }
    }
}
