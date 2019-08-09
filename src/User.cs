namespace Jpp.Common.Backend
{
    /// <summary>
    /// Class describing the currently signed in user profile
    /// </summary>
    public class User
    {
        /// <summary>
        /// Preferred username as returned by the ID service
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Actual full name as returned by the ID service
        /// </summary>
        public string Name { get; set; }

        public string UserId { get; set; }
    }
}
