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
        public static string BASE_URL { get; set; }

        /// <summary>
        /// Set to true to enable unattended authentication.
        /// WARNING - This flow is inherently risky and should not be used in a production environment
        /// </summary>
        public static bool UNATTENDED_AUTH_ENABLED { get; set; } = false;
    }
}
