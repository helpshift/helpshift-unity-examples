#if UNITY_IOS
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace Helpshift
{
    public class HelpshiftiOSInboxDelegatesHolder
    {
        private static HelpshiftiOSInboxDelegatesHolder delegateHolder;
        private static HelpshiftiOSInboxDelegate externalInboxDelegate;
        private static HelpshiftiOSInboxPushNotificationDelegate externalInboxPushNotificationDelegate; 
		private static HelpshiftiOSCampaignsDelegate externalCampaignsDelegate;

        private delegate void ReceiveInboxCallbackDelegate(string key);

        [MonoPInvokeCallback (typeof(ReceiveInboxCallbackDelegate))]
        private static void ReceiveInboxCallbackDelegateImpl(string callbackMessage)
        {
            // Parse callbackMessage and resolve which api is to be called from
            // either externalInboxDelegate or externalInboxPushNotificationDelegate
			string[] tokenizedArray = callbackMessage.Split(new string[] { "<hs-separator>" }, StringSplitOptions.None);
			if (tokenizedArray.Length != 2)
			{
				return;
			}
            string delegateMethod = tokenizedArray [0];

			if (delegateMethod == "inboxMessageAdded") {
				externalInboxDelegate.inboxMessageAdded (tokenizedArray [1]);
			} else if (delegateMethod == "inboxMessageDeleted") {
				externalInboxDelegate.inboxMessageDeleted (tokenizedArray [1]);
			} else if (delegateMethod == "failedToAddInboxMessageWithId") {
				externalInboxDelegate.failedToAddInboxMessageWithId (tokenizedArray [1]);
			} else if (delegateMethod == "iconImageDownloadedForInboxMessage") {
				externalInboxDelegate.iconImageDownloaded (tokenizedArray [1]);
			} else if (delegateMethod == "coverImageDownloadedForInboxMessage") {
				externalInboxDelegate.coverImageDownloaded (tokenizedArray [1]);
			} else if (delegateMethod == "inboxMessageMarkedAsSeen") {
				externalInboxDelegate.inboxMessageMarkedAsSeen (tokenizedArray [1]);
			} else if (delegateMethod == "inboxMessageMarkedAsRead") {
				externalInboxDelegate.inboxMessageMarkedAsRead (tokenizedArray [1]);
			} else if (delegateMethod == "handleNotificationForInboxMessage") {
				externalInboxPushNotificationDelegate.onInboxMessagePushNotificationClicked (tokenizedArray [1]);
			} else if (delegateMethod == "didReceiveUnreadMessagesCount") {
				externalCampaignsDelegate.didReceiveUnreadMessagesCount(Int32.Parse(tokenizedArray[1]));
			}
        }

        [DllImport ("__Internal")]
        private static extern void _hsRegisterInboxMessageCallback(ReceiveInboxCallbackDelegate callback);

        private HelpshiftiOSInboxDelegatesHolder()
        {
        }

        public static HelpshiftiOSInboxDelegatesHolder GetInstance()
        {
            if (delegateHolder == null)
            {
                delegateHolder = new HelpshiftiOSInboxDelegatesHolder();
                _hsRegisterInboxMessageCallback(ReceiveInboxCallbackDelegateImpl);
            }

            return delegateHolder;
        }

        public void setInboxDelegate(HelpshiftiOSInboxDelegate externalDelegate)
        {
            externalInboxDelegate = externalDelegate;
        }

        public void setInboxPushNotificationDelegate(HelpshiftiOSInboxPushNotificationDelegate externalDelegate)
        {
            externalInboxPushNotificationDelegate = externalDelegate;
        }

		public void setCampaignsDelegate(HelpshiftiOSCampaignsDelegate externalDelegate)
		{
			externalCampaignsDelegate = externalDelegate;
		}
    }
}
#endif