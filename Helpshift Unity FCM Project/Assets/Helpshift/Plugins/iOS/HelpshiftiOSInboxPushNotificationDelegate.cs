#if UNITY_IOS
using System;
using UnityEngine;
namespace Helpshift
{
    public class HelpshiftiOSInboxPushNotificationDelegate
    {
        private IHelpshiftInboxPushNotificationDelegate externalDelegate;

        public HelpshiftiOSInboxPushNotificationDelegate(IHelpshiftInboxPushNotificationDelegate externalDelegate)
        {
            this.externalDelegate = externalDelegate;
        }

        public void onInboxMessagePushNotificationClicked(String messageIdentifier)
        {
            externalDelegate.OnInboxMessagePushNotificationClicked(messageIdentifier);
        }
    }
}
#endif
