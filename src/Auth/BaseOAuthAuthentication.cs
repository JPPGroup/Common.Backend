using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Jpp.Common.Backend.Auth
{
    public abstract class BaseOAuthAuthentication : IAuthentication
    {
        public bool Authenticated { get; private set; }

        private string _accessToken;
        private string _idToken;
        private HttpClient _client;

        private IMessageProvider _messenger;

        public BaseOAuthAuthentication(IMessageProvider messenger)
        {
            _messenger = messenger;
        }

        public async Task Authenticate()
        {
            AuthenticationPrompt();
        }

        public async Task Expire()
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
                var authResponse = new AuthorizeResponse(uri.ToString());
                if (!string.IsNullOrWhiteSpace(authResponse.Code))
                {
                    HttpClient client = new HttpClient();

                    //await Backend.Authenticate(authResponse.Code);
                    try
                    {
                        var response = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
                        {
                            Address = $"http://{Backend.BASE_URL}/connect/token",

                            ClientId = "clientApp",
                            ClientSecret = "secret",

                            Code = authResponse.Code,
                            RedirectUri = "https://localhost/signin-oidc",

                        });

                       if(response.IsError)
                           throw new Exception(response.ErrorDescription);

                        _accessToken = response.AccessToken;
                        _idToken = response.IdentityToken;

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                        Authenticated = true;
                        OnAuthentication();

                        return true;
                    }
                    catch (Exception e)
                    {
                        await _messenger.ShowCriticalError("Unable to communicate with server. Please try again later.");
                        //TODO: Log this.
                    }
                }
            }

            if (escapedUri.StartsWith(@"http://" + Backend.BASE_URL + @"/Identity/Account/Logout"))
            {
                //TODO: Do we need to clear cookies?
                //LoginUrl = Backend.GetAuthenticationURL();
            }

            return false;
        }

        public event EventHandler AuthenticationChanged;

        public async Task<User> GetUserProfile()
        {
            if (!Authenticated)
              throw new NotAuthenticatedException();

            HttpClient _client = GetAuthenticatedClient();

            var userresponse = await _client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = $"http://{Backend.BASE_URL}/connect/userinfo",
                Token = _accessToken
            });

            User result = new User()
            {
                Username = userresponse.Claims.First(c => c.Type == "preferred_username").Value,
                Name = userresponse.Claims.First(c => c.Type == "name").Value,
                UserId = userresponse.Claims.First(c => c.Type == "sub").Value
            };

            return result;
        }

        public HttpClient GetAuthenticatedClient()
        {
            if(!Authenticated)
                throw new NotAuthenticatedException();

            if (_client == null)
            {
                _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }

            return _client;
        }

        protected async Task OnAuthentication()
        {
            //await LoadUser();
            _client = null;

            EventHandler handler = AuthenticationChanged;
            if (null != handler) handler(null, EventArgs.Empty);
        }
    }
}
