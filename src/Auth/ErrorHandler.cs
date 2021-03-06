﻿using System.Net.Http;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.Auth
{
    internal class ErrorHandler : HttpClientHandler
    {
        private readonly IMessageProvider _messageProvider;

        public ErrorHandler(IMessageProvider messageProvider)
        {
            _messageProvider = messageProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode) await _messageProvider.ShowError($"{request.RequestUri}\t{(int)response.StatusCode}\t{response.Headers.Date}");
            
            return response;
        }
    }
}
