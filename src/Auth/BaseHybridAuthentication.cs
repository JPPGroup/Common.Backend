using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Jpp.Common.Backend.Auth
{
    public abstract class BaseHybridAuthentication : BaseOAuthAuthentication
    {
        private string _idToken;

        protected BaseHybridAuthentication(IMessageProvider messenger) : base(messenger)
        {
        }

        public override async Task Authenticate()
        {
            AuthenticationPrompt();
        }

        public override async Task Expire()
        {
            ExpirePrompt();
        }

        public abstract void AuthenticationPrompt();
        public abstract void ExpirePrompt();

        protected string GetAuthenticationURL()
        {
            var ru = new RequestUrl($"http://{Backend.BASE_URL}/connect/authorize");

            var url = ru.CreateAuthorizeUrl(
                clientId: "clientApp",
                responseType: "code id_token",
                redirectUri: "https://localhost/signin-oidc",
                nonce: "xyz",
                scope: "openid offline_access profile resourceApi");

            return url;
        }

        protected string GetExpireURL()
        {
            var ru = new RequestUrl($"http://{Backend.BASE_URL}/connect/authorize");
            var url = ru.CreateEndSessionUrl(idTokenHint: _idToken);
            return url;
        }

        /// <summary>
        /// Determine if current URL is part of flow
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>Return true when the current URL is the end of a flow</returns>
        public async Task<bool> EvaluateURL(Uri uri)
        {
            var escapedUri = System.Net.WebUtility.UrlDecode(uri.ToString());

            if (escapedUri.StartsWith(@"https://localhost/signin-oidc"))
            {
                try
                {
                    return await ProcessLogin(uri);
                }
                catch (Exception e)
                {
                    await _messenger.ShowCriticalError("Unable to communicate with server. Please try again later.");
                    //TODO: Log this.
                }
            }

            if (escapedUri.StartsWith(@"http://" + Backend.BASE_URL + @"/Identity/Account/Logout"))
            {
                //TODO: Do we need to clear cookies?
                //LoginUrl = Backend.GetAuthenticationURL();
            }

            return false;
        }

        private async Task<bool> ProcessLogin(Uri uri)
        {
            var authResponse = new AuthorizeResponse(uri.ToString());
            if (!string.IsNullOrWhiteSpace(authResponse.Code))
            {
                HttpClient client = new HttpClient();

                var response = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
                {
                    Address = $"http://{Backend.BASE_URL}/connect/token",

                    ClientId = "clientApp",
                    ClientSecret = "secret",

                    Code = authResponse.Code,
                    RedirectUri = "https://localhost/signin-oidc",

                });

                if (response.IsError)
                    throw new Exception(response.ErrorDescription);

                _accessToken = response.AccessToken;
                _idToken = response.IdentityToken;

                Authenticated = true;
                OnAuthentication();
                return true;
            }

            return false;
        }
    }
}
