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
            Backend.BASE_URL = TestContext.Parameters["BaseUrl"];
        }
    }
}
