using System;

namespace Jpp.Common.Backend
{
    /// <summary>
    /// Class containing general backend methods and properties
    /// </summary>
    public class Backend
    {
        /// <summary>
        /// Base domain for backend service
        /// </summary>
        public static string BASE_URL 
        { 
            get
            {
                if (string.IsNullOrEmpty(_baseUrl))
                    throw new NullReferenceException("Base Url has not been set");

                return _baseUrl;
            }
            set { _baseUrl = value; } 
        }

        private static string _baseUrl;

        /// <summary>
        /// Set to true to enable unattended authentication.
        /// WARNING - This flow is inherently risky and should not be used in a production environment
        /// </summary>
        public static bool UNATTENDED_AUTH_ENABLED { get; set; } = false;
    }
}
