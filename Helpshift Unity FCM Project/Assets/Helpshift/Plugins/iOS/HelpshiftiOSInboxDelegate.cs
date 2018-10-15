#if UNITY_IOS
using System;
using UnityEngine;
namespace Helpshift
{
	
    public class HelpshiftiOSInboxDelegate
    {
        private IHelpshiftInboxDelegate externalDelegate;
        public HelpshiftiOSInboxDelegate(IHelpshiftInboxDelegate externalDelegate)
        { 
            this.externalDelegate = externalDelegate;
        }

        public void inboxMessageAdded(String stringEncodedInboxmessage)
        {
            HelpshiftiOSInboxMessage externalMessage = HelpshiftiOSInboxMessage.createInboxMessage(stringEncodedInboxmessage);
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

        public void failedToAddInboxMessageWithId(String messageIdentifier)
        {
            externalDelegate.FailedToAddInboxMessageWithId(messageIdentifier);
        }
    }
}
#endif
