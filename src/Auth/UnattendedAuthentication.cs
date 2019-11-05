using IdentityModel.Client;
using System;
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

        public UnattendedAuthentication(IMessageProvider messenger, string clientId, string clientSecret)
        {
            _errorHandler = new ErrorHandler(messenger);
            _messenger = messenger;
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public async Task Authenticate()
        {
            HttpClient client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync($"http://{Backend.BASE_URL}");
            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = _clientId,
                ClientSecret = _clientSecret
            });

            if (response.IsError) throw new Exception(response.Error);

            var token = response.AccessToken;
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

        public Task<User> GetUserProfile()
        {
            throw new NotImplementedException();
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
