#if UNITY_IOS
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Helpshift
{
    /**
	 * Inbox class for iOS.
	 */ 
    public class HelpshiftInboxiOS
    {
        [DllImport ("__Internal")]
        private static extern void hsMarkInboxMessageAsRead(string messageIdentifier);

        [DllImport ("__Internal")]
        private static extern void hsMarkInboxMessageAsSeen(string messageIdentifier);

        [DllImport ("__Internal")]
        private static extern void hsDeleteInboxMessage(string messageIdentifier);

        [DllImport ("__Internal")]
        private static extern string hsGetAllInboxMessages();

        [DllImport ("__Internal")]
        private static extern string hsGetInboxMessage(string messageIdentifier);

        [DllImport ("__Internal")]
        private static extern void hsSetInboxMessageDelegate();

        [DllImport ("__Internal")]
        private static extern void hsSetInboxPushNotificationDelegate();


        // Add native methods for MarkInboxMessageAsRead, MarkInboxMessageAsSeen, DeleteInboxMessage
        // The native method for "GetAllInboxMessages" should get an encoded string which can then be parsed to
        // HelpshiftInboxMessage via the method "HelpshiftiOSInboxMessage.createInboxMessage(string message)"
        // Similarly for native method for  "GetInboxMessage"

        // Add a MonoPInvokeCallback delegate implementation for passing it to objective C layer as a C function pointer.
        // This delegate will get encoded string messages from ObjectiveC layer
        // (from required delegates implemented in ObjectiveC code). The encoded string message should be parsed and
        // mapped to method calls for either HelpshiftiOSInboxDelegate or HelpshiftiOSInboxPushNotificationDelegate

        public HelpshiftInboxiOS()
        {
        }

        public List<HelpshiftInboxMessage> GetAllInboxMessages()
        {
            List<HelpshiftInboxMessage> inboxMessageList = new List<HelpshiftInboxMessage>();
            string encodedInboxMessages = hsGetAllInboxMessages();
			if (encodedInboxMessages == null || encodedInboxMessages.Length <= 0) {
				return null;
			}
            string[] tokenizedArray = encodedInboxMessages.Split(';');
            foreach (string inboxMessage in tokenizedArray)
            {
                inboxMessageList.Add(HelpshiftiOSInboxMessage.createInboxMessage(inboxMessage));
            }
            return inboxMessageList;
        }

        public HelpshiftInboxMessage GetInboxMessage(String messageIdentifier)
        {
            string encodedInboxMessage = hsGetInboxMessage(messageIdentifier);
			if (encodedInboxMessage == null  || encodedInboxMessage.Length <= 0) {
				return null;
			}
            HelpshiftiOSInboxMessage inboxMessage = HelpshiftiOSInboxMessage.createInboxMessage(encodedInboxMessage);
            return inboxMessage;
        }

        public void MarkInboxMessageAsRead(String messageIdentifier)
        {
            hsMarkInboxMessageAsRead(messageIdentifier);
        }

        public void MarkInboxMessageAsSeen(String messageIdentifier)
        {
            hsMarkInboxMessageAsSeen(messageIdentifier);
        }

        public void DeleteInboxMessage(String messageIdentifier)
        {
            hsDeleteInboxMessage(messageIdentifier);
        }

        public void SetInboxMessageDelegate(IHelpshiftInboxDelegate externalDelegate)
        {
            hsSetInboxMessageDelegate();
            HelpshiftiOSInboxDelegate internalInboxDelegate = new HelpshiftiOSInboxDelegate(externalDelegate);
            HelpshiftiOSInboxDelegatesHolder.GetInstance().setInboxDelegate(internalInboxDelegate);
        }

        public void SetInboxPushNotificationDelegate(IHelpshiftInboxPushNotificationDelegate externalDelegate)
        {
            hsSetInboxPushNotificationDelegate();
            HelpshiftiOSInboxPushNotificationDelegate internalInboxPushNotificationDelegate = new HelpshiftiOSInboxPushNotificationDelegate(externalDelegate);
            HelpshiftiOSInboxDelegatesHolder.GetInstance().setInboxPushNotificationDelegate(internalInboxPushNotificationDelegate);
        }
    }
}
#endif
