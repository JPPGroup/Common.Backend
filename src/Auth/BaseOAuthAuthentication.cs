using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Jpp.Common.Backend.Auth
{
    public abstract class BaseOAuthAuthentication : IAuthentication
    {
        internal HttpMessageHandler _messageHandler;
        protected IMessageProvider Messenger { get; private set; }

        public bool Authenticated { get; set; } = false;
        public event EventHandler AuthenticationChanged;

        protected HttpClient _client;
        protected string _accessToken;

        public abstract Task Authenticate();

        public abstract Task Expire();

        protected BaseOAuthAuthentication(IMessageProvider messenger) : this(messenger, new ErrorHandler(messenger))
        {
        }

        internal BaseOAuthAuthentication(IMessageProvider messenger, HttpMessageHandler handler)
        {            
            _messageHandler = handler;
            Messenger = messenger;
        }

        public HttpClient GetAuthenticatedClient()
        {
            if (!Authenticated)
                throw new NotAuthenticatedException();

            if (_client == null)
            {
                _client = new HttpClient(_messageHandler);
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
            }

            return _client;
        }

        public async Task<User> GetUserProfile()
        {
            if (!Authenticated)
                throw new NotAuthenticatedException();

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
