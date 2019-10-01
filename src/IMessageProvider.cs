using System.Threading.Tasks;

namespace Jpp.Common.Backend
{
    /// <summary>
    /// Interface defining messages that should be displayed to the user
    /// </summary>
    public interface IMessageProvider
    {
        /// <summary>
        /// Show prompt asking for storage permission
        /// </summary>
        /// <returns>Awaitable task</returns>
        Task ShowStorageAccessPermissionWarning();

        /// <summary>
        /// Show error dialog to user and terminate application
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        /// <returns>Awaitable task</returns>
        Task ShowCriticalError(string message);

        /// <summary>
        /// Show non critical error dialog to user
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        /// <returns>Awaitable task</returns>
        Task ShowError(string message);
    }
}
