#if UNITY_ANDROID
using System;
using UnityEngine;
namespace Helpshift
{
    public class HelpshiftAndroidInboxPushNotificationDelegate : AndroidJavaProxy
    {
        private IHelpshiftInboxPushNotificationDelegate externalDelegate;

        public HelpshiftAndroidInboxPushNotificationDelegate(IHelpshiftInboxPushNotificationDelegate externalDelegate) : 
			base("com.helpshift.campaigns.delegates.InboxPushNotificationDelegate")
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
