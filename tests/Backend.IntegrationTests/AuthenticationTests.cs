using Jpp.Common.Backend.Auth;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.IntegrationTests
{
    [TestFixture]
    class AuthenticationTests
    {
        IMessageProvider _message;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _message = new MessageProviderFake();

        }

        [Test]
        public async Task BasicLoginTest()
        {
            IAuthentication auth = new UnattendedAuthentication(_message, TestContext.Parameters["ClientId"], TestContext.Parameters["ClientSecret"], TestContext.Parameters["Username"], TestContext.Parameters["Password"]);
            await auth.Authenticate();

            //Confirm sustem believes authenticated
            Assert.IsTrue(auth.Authenticated, "Not authenticated");
        }

        [Test]
        public async Task GetUserTest()
        {
            IAuthentication auth = new UnattendedAuthentication(_message, TestContext.Parameters["ClientId"], TestContext.Parameters["ClientSecret"], TestContext.Parameters["Username"], TestContext.Parameters["Password"]);
            await auth.Authenticate();

            //Verify by retrieving the user info
            User profile = await auth.GetUserProfile();
            Assert.Multiple(() =>
            {
                Assert.IsFalse(String.IsNullOrEmpty(profile.Name), "Empty name");
                Assert.IsFalse(String.IsNullOrEmpty(profile.UserId), "Empty User Id");
                Assert.IsFalse(String.IsNullOrEmpty(profile.Username), "Empty username");
            });
        }

        [Test]
        public async Task GetAuthenticatedClientTest()
        {
            IAuthentication auth = new UnattendedAuthentication(_message, TestContext.Parameters["ClientId"], TestContext.Parameters["ClientSecret"], TestContext.Parameters["Username"], TestContext.Parameters["Password"]);
            await auth.Authenticate();

            HttpClient client = auth.GetAuthenticatedClient();
            Assert.Multiple(() =>
            {
                StringAssert.AreEqualIgnoringCase("Bearer", client.DefaultRequestHeaders.Authorization.Scheme, "Bearer token not set");
                Assert.IsFalse(String.IsNullOrEmpty(client.DefaultRequestHeaders.Authorization.Parameter), "No auth parameter set");
                });
        }
    }
}
