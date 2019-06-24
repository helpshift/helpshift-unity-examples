#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Helpshift
{
    /**
	 * Inbox class for Android.
	 */ 
    public class HelpshiftInboxAndroid
    {
        private AndroidJavaObject hsInboxJavaInstance;

		public HelpshiftInboxAndroid() {
			AndroidJavaClass hsInboxJava = new AndroidJavaClass ("com.helpshift.campaigns.Inbox");
			hsInboxJavaInstance = hsInboxJava.CallStatic<AndroidJavaObject> ("getInstance");
		}

        public List<HelpshiftInboxMessage> GetAllInboxMessages()
        {
            try
            {
                AndroidJavaObject inboxMessagesJavaList = hsInboxJavaInstance.Call<AndroidJavaObject>("getAllInboxMessages");

                List<HelpshiftInboxMessage> messagesList = new List<HelpshiftInboxMessage>();
                int size = inboxMessagesJavaList.Call<int>("size");

                for (int i = 0; i < size; i++)
                {
                    AndroidJavaObject obj = inboxMessagesJavaList.Call<AndroidJavaObject>("get", i);
                    messagesList.Add(HelpshiftAndroidInboxMessage.createInboxMessage(obj));
                }

                return messagesList;
            } catch (Exception e)
            {
                Debug.Log("Helpshift : Error getting all inbox messages : " + e.Message);
            }

            return null;
        }

        public HelpshiftInboxMessage GetInboxMessage(String messageIdentifier)
        {
            try
            {
                AndroidJavaObject inboxMessageJava = hsInboxJavaInstance.Call<AndroidJavaObject>("getInboxMessage", messageIdentifier);
                HelpshiftAndroidInboxMessage inboxMessage = HelpshiftAndroidInboxMessage.createInboxMessage(inboxMessageJava);
                return inboxMessage;
            } catch (Exception e)
            {
                Debug.Log("Helpshift : Error getting inbox message : " + e.Message);
            }

            return null;
        }

        public void MarkInboxMessageAsRead(String messageIdentifier)
        {
            hsInboxJavaInstance.Call("markInboxMessageAsRead", messageIdentifier);
        }

        public void MarkInboxMessageAsSeen(String messageIdentifier)
        {
            hsInboxJavaInstance.Call("markInboxMessageAsSeen", messageIdentifier);
        }

        public void DeleteInboxMessage(String messageIdentifier)
        {
            hsInboxJavaInstance.Call("deleteInboxMessage", messageIdentifier);
        }

        public void SetInboxMessageDelegate(IHelpshiftInboxDelegate externalDelegate)
        {
            HelpshiftAndroidInboxDelegate internalDelegate = new HelpshiftAndroidInboxDelegate(externalDelegate);
            hsInboxJavaInstance.Call("setInboxMessageDelegate", internalDelegate);
        }

        public void SetInboxPushNotificationDelegate(IHelpshiftInboxPushNotificationDelegate externalDelegate)
        {
            HelpshiftAndroidInboxPushNotificationDelegate internalDelegate = 
				                            new HelpshiftAndroidInboxPushNotificationDelegate(externalDelegate);

            hsInboxJavaInstance.Call("setInboxPushNotificationDelegate", internalDelegate);
        }
    }
}
#endif
