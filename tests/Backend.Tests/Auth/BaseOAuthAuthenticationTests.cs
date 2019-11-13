using Jpp.Common.Backend.Auth;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace Jpp.Common.Backend.UnitTests.Auth
{
    [TestFixture]
    class BaseOAuthAuthenticationTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Backend.BASE_URL = "jppuk.net";
        }

        [Test]
        public async Task OnAuthentication_Called()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            bool eventFired = false;

            IAuthentication auth = new BaseOAuthAuthenticationStub(handlerMock.Object);
            auth.AuthenticationChanged += delegate { eventFired = true; };
            await auth.Authenticate();

            Assert.IsTrue(eventFired);
        }

        [Test]
        public async Task GetUserProfile_Valid()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"sub\": \"248289761001\",\"name\": \"Bob Smith\",\"preferred_username\": \"Bobby\",\"given_name\": \"Bob\",\"family_name\": \"Smith\",\"role\": [\"user\",\"admin\"]}"),
                })
                .Verifiable();

            IAuthentication auth = new BaseOAuthAuthenticationStub(handlerMock.Object);
            await auth.Authenticate();
            User profile = await auth.GetUserProfile();
            
            handlerMock.Verify();

            Assert.Multiple(() =>
            {
                StringAssert.AreEqualIgnoringCase("Bob Smith", profile.Name, "Invalid name");
                StringAssert.AreEqualIgnoringCase("248289761001", profile.UserId, "Invalid UserId");
                StringAssert.AreEqualIgnoringCase("Bobby", profile.Username, "Invalid Username");
            });
        }

        [Test]
        public async Task GetUserProfile_NotAuthenticated()
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"sub\": \"248289761001\",\"name\": \"Bob Smith\",\"preferred_username\": \"Bobby\",\"given_name\": \"Bob\",\"family_name\": \"Smith\",\"role\": [\"user\",\"admin\"]}"),
                });

            IAuthentication auth = new BaseOAuthAuthenticationStub(handlerMock.Object);
            Assert.ThrowsAsync<NotAuthenticatedException>(async () =>
            {
                User u = await auth.GetUserProfile();
            });
            handlerMock.VerifyNoOtherCalls();
        }
    }
}
