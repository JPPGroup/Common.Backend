using System;
using System.Collections.Generic;
using System.Text;

namespace Jpp.Common.Backend.Notification
{
    public class Notifications
    {
        public const string LISTEN_CONNECTION_STRING =
            @"Endpoint = sb://jhub-notifications.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=9kTzeDewhVjgPVQVqwCNa+WHtp14Dkgs75GKDmjQNbU=";

        public const string NOTIFICATION_HUB_NAME = "LoneWorking";

        public static readonly string DEFAULT_CHANNEL_ID = "General";
    }
}
