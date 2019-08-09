using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.Auth
{
    /// <summary>
    /// Interface for providing authentication for backend services
    /// </summary>
    public interface IAuthentication
    {
        /// <summary>
        /// Authenticate with backend cluster
        /// </summary>
        /// <returns></returns>
        Task Authenticate();

        /// <summary>
        /// Remove authentication with backend cluster
        /// </summary>
        /// <returns></returns>
        Task Expire();

        /// <summary>
        /// Boolean indicating if currently authenticated
        /// </summary>
        bool Authenticated { get; }

        /// <summary>
        /// Event fired when the authentication stage changes
        /// </summary>
        event EventHandler AuthenticationChanged;

        /// <summary>
        /// Get the profile of the currently authenticated user
        /// </summary>
        /// <returns>Currently authenticated user</returns>
        Task<User> GetUserProfile();

        /// <summary>
        /// Get an instance of httpclient with requisite authentication headers
        /// </summary>
        /// <returns>Authenticated http client</returns>
        HttpClient GetAuthenticatedClient();

    }
}
