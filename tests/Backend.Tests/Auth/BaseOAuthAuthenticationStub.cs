using Jpp.Common.Backend.Auth;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.UnitTests.Auth
{
    class BaseOAuthAuthenticationStub : BaseOAuthAuthentication
    {
        public BaseOAuthAuthenticationStub(HttpMessageHandler handler) : base(handler)
        {

        }

        public async override Task Authenticate()
        {
            Authenticated = true;
            _accessToken = "123abc";
            OnAuthentication();
        }

        public override Task Expire()
        {
            throw new NotImplementedException();
        }
    }
}
