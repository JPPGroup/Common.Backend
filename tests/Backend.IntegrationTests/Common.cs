using NUnit.Framework;
using System;

namespace Jpp.Common.Backend.IntegrationTests
{
    [SetUpFixture]
    class Common
    {
        [OneTimeSetUp]
        public void Setup()
        {
            TestContext.Progress.WriteLine(TestContext.Parameters["BaseUrl"]);
            TestContext.Progress.WriteLine(TestContext.Parameters["ClientId"]);
            TestContext.Progress.WriteLine(TestContext.Parameters["ClientSecret"]);

            Backend.BASE_URL = TestContext.Parameters["BaseUrl"];
        }
    }
}
