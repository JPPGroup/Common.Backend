using System;
using System.Threading.Tasks;

namespace Jpp.Common.Backend.IntegrationTests
{
    class MessageProviderFake : IMessageProvider
    {
        public async Task<bool> ShowConfirmDialog(string message, string title = "Confirm")
        {
            return true;
        }

        public async Task ShowCriticalError(string message)
        {
            Console.WriteLine(message);
        }

        public async Task ShowError(string message)
        {
            Console.WriteLine(message);
        }

        public async Task ShowMessage(string message, string title)
        {
            Console.WriteLine(message);
        }

        public async Task ShowStorageAccessPermissionWarning()
        {
            Console.WriteLine("Storage error");
        }
    }
}
