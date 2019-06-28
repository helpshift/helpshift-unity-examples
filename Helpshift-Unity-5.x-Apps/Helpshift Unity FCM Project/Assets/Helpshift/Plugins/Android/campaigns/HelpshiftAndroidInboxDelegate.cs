#if UNITY_ANDROID
using System;
using UnityEngine;
namespace Helpshift
{
    // Implements the com.helpshift.campaigns.delegates.InboxMessageDelegate from android java code.
    // Takes an external pure C# implementation for callbacks to the client and hides the actual java interface implementation.
    public class HelpshiftAndroidInboxDelegate : AndroidJavaProxy
    {
        private IHelpshiftInboxDelegate externalDelegate;
        public HelpshiftAndroidInboxDelegate(IHelpshiftInboxDelegate externalDelegate) : 
		                               base("com.helpshift.campaigns.delegates.InboxMessageDelegate")
        {
            this.externalDelegate = externalDelegate;
        } 

        public void inboxMessageAdded(AndroidJavaObject message)
        {
            HelpshiftAndroidInboxMessage externalMessage = HelpshiftAndroidInboxMessage.createInboxMessage(message);
            externalDelegate.InboxMessageAdded(externalMessage);
        }

        public void iconImageDownloaded(String messageIdentifier)
        {
            externalDelegate.IconImageDownloaded(messageIdentifier);
        }

        public void coverImageDownloaded(String messageIdentifier)
        {
            externalDelegate.CoverImageDownloaded(messageIdentifier);
        }

        public void inboxMessageDeleted(String messageIdentifier)
        {
            externalDelegate.InboxMessageDeleted(messageIdentifier);
        }

        public void inboxMessageMarkedAsSeen(String messageIdentifier)
        {
            externalDelegate.InboxMessageMarkedAsSeen(messageIdentifier);
        }

        public void inboxMessageMarkedAsRead(String messageIdentifier)
        {
            externalDelegate.InboxMessageMarkedAsRead(messageIdentifier);
        }
    }
}
#endif
