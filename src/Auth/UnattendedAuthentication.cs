using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.Auth
{
    public class UnattendedAuthentication : BaseOAuthAuthentication
    {
        protected string _clientId, _clientSecret, _username, _password;

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
            _username = username;
            _password = password;
        }

        public override async Task Authenticate()
        {
            HttpClient client = new HttpClient();
            //TODO: Switch back to using discovery
            //var disco = await client.GetDiscoveryDocumentAsync($"http://{Backend.BASE_URL}");
            var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = $"http://{Backend.BASE_URL}/connect/token", //disco.TokenEndpoint,
                ClientId = _clientId,
                ClientSecret = _clientSecret,
                UserName = _username,
                Password = _password
            });

            if (response.IsError) throw new Exception(response.Error);

            _accessToken = response.AccessToken;            

            Authenticated = true;
            await OnAuthentication();
        }

        public override Task Expire()
        {
            throw new NotImplementedException();
        }
    }
}
