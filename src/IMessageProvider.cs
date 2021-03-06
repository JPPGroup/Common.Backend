﻿using System.Threading.Tasks;

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

        /// <summary>
        /// Show non critical error dialog to user
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of messagebox</param>
        /// <returns>Awaitable task</returns>
        Task ShowMessage(string message, string title);

        /// <summary>
        /// Show confirmation dialog to the user
        /// </summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of messagebox</param>
        /// <returns>Awaitable boolean indicating confirmation</returns>
        Task<bool> ShowConfirmDialog(string message, string title = "Confirm");
    }
}
