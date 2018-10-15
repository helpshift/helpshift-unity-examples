using System;
using Helpshift;
using UnityEngine;

namespace HelpshiftExample
{
    public class InboxPushNotificationDelegate : IHelpshiftInboxPushNotificationDelegate
    {
        public InboxPushNotificationDelegate()
        {
        }

        public void OnInboxMessagePushNotificationClicked(String messageIdentifier)
        {
            Debug.Log("OnInboxMessagePushNotificationClicked : " + messageIdentifier);
        }
    }
}

