﻿using IdentityModel.Client;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.Auth
{
    public class UnattendedAuthentication : IAuthentication
    {
        public bool Authenticated { get; set; } = false;

        HttpClient _client;
        private readonly ErrorHandler _errorHandler;
        private IMessageProvider _messenger;
        public event EventHandler AuthenticationChanged;
        string _clientId, _clientSecret, _accessToken;

        /// <summary>
        /// Create new unattended authentication service
        /// WARNING - This flow is inherently risky and should not be used in a production environment
        /// </summary>
        /// <param name="messenger">Messenger service to be used</param>
        /// <param name="clientId">Client id to be passed to backend</param>
        /// <param name="clientSecret">Client secret to be passed to backend</param>
        /// <param name="username">Username of user</param>
        /// <param name="password">Password of user</param>
        public UnattendedAuthentication(IMessageProvider messenger, string clientId, string clientSecret, string username, string password)
        {
            if(!Backend.UNATTENDED_AUTH_ENABLED)
                throw  new InvalidOperationException("Unattended authentication requires enabling via Backend class prior to creating.");

            _errorHandler = new ErrorHandler(messenger);
            _messenger = messenger;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public async Task Authenticate()
        {
            HttpClient client = new HttpClient();
            //TODO: Switch back to using discovery
            //var disco = await client.GetDiscoveryDocumentAsync($"http://{Backend.BASE_URL}");
            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = $"http://{Backend.BASE_URL}/connect/token", //disco.TokenEndpoint,
                ClientId = _clientId,
                ClientSecret = _clientSecret,
                UserName = "michael.liddiard@jppuk.net",
                Password = "Regit-340"
            });

            if (response.IsError) throw new Exception(response.Error);

            _accessToken = response.AccessToken;            

            Authenticated = true;
            await OnAuthentication();
        }

        public Task Expire()
        {
            throw new NotImplementedException();
        }

        public HttpClient GetAuthenticatedClient()
        {
            if (!Authenticated)
                throw new NotAuthenticatedException();

            if (_client == null)
            {
                _client = new HttpClient(_errorHandler);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }

            return _client;
        }

        public async Task<User> GetUserProfile()
        {
            var userresponse = await GetAuthenticatedClient().GetUserInfoAsync(new UserInfoRequest
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

        protected async Task OnAuthentication()
        {
            //await LoadUser();
            _client = null;

            EventHandler handler = AuthenticationChanged;
            if (null != handler) handler(null, EventArgs.Empty);
        }
    }
}
