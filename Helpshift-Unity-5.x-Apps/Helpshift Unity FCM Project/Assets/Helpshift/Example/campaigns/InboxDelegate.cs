using System;
using UnityEngine;
using Helpshift;
namespace HelpshiftExample
{
    public class InboxDelegate : IHelpshiftInboxDelegate
    {
        public void InboxMessageAdded(HelpshiftInboxMessage message)
        {
            Debug.Log("inboxMessageAdded : " + message.ToString() + message.GetActionType(0));
        }

        public void IconImageDownloaded(String messageIdentifier)
        {
            Debug.Log("iconImageDownloaded : " + messageIdentifier);
        }

        public void CoverImageDownloaded(String messageIdentifier)
        {
            Debug.Log("coverImageDownloaded : " + messageIdentifier);
        }

        public void InboxMessageDeleted(String messageIdentifier)
        {
            Debug.Log("inboxMessageDeleted : " + messageIdentifier);
        }

        public void InboxMessageMarkedAsSeen(String messageIdentifier)
        {
            Debug.Log("inboxMessageMarkedAsSeen : " + messageIdentifier);
        }

        public void InboxMessageMarkedAsRead(String messageIdentifier)
        {
            Debug.Log("inboxMessageMarkedAsRead : " + messageIdentifier);
        }

        public void FailedToAddInboxMessageWithId(String messageIdentifier)
        {
            Debug.Log("failedToAddInboxMessageWithId : " + messageIdentifier);
        }
    }
}

