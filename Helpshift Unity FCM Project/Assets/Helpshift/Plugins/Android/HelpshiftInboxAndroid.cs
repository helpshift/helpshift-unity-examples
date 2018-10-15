#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Helpshift
{
    /**
	 * Inbox class for Android.
	 */ 
    public class HelpshiftInboxAndroid : IWorkerMethodDispatcher, IDexLoaderListener
    {
        private AndroidJavaObject hsInboxJavaInstance;

        public HelpshiftInboxAndroid()
        {
            HelpshiftDexLoader.getInstance().registerListener(this);
            HelpshiftWorker.getInstance().registerClient("inbox", this);
        }

        public void onDexLoaded()
        {
			// In cases where dex is already loaded, we would need to make sure that the listener
			// for worker thread is also registered since we add  "getInboxInstance" to worker queue.
			HelpshiftWorker.getInstance().registerClient("inbox", this);

			// Get the Inbox instance only after the queue finishes since we want the "install" call
			// to finish before calling Inbox.
            addHSApiCallToQueue("initializeInboxInstance", "getInboxInstance", null);
        }

        private void addHSApiCallToQueue(String methodIdentifier, String api, object[] args)
        {
            HelpshiftWorker.getInstance().enqueueApiCall("inbox", methodIdentifier, api, args);
        }

        private void hsInboxApiCall(String api, object[] args)
        {
            addHSApiCallToQueue("hsInboxApiCall", api, args);
        }

        public void resolveAndCallApi(string methodIdentifier, string apiName, object[] args)
        {
            if (methodIdentifier.Equals("hsInboxApiCall"))
            {
                hsInboxJavaInstance.Call(apiName, args);
            } else if (methodIdentifier.Equals("initializeInboxInstance"))
            {
                // Initialize the java Inbox instance
                hsInboxJavaInstance = HelpshiftDexLoader.getInstance().getHSDexLoaderJavaClass().
					              CallStatic<AndroidJavaObject>(apiName);
            }

        }

        private void synchronousWaitForHSApiCallQueue()
        {
            HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
        }

        public List<HelpshiftInboxMessage> GetAllInboxMessages()
        {
            synchronousWaitForHSApiCallQueue();

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
            synchronousWaitForHSApiCallQueue();
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
            synchronousWaitForHSApiCallQueue();
            hsInboxJavaInstance.Call("markInboxMessageAsRead", messageIdentifier);
        }

        public void MarkInboxMessageAsSeen(String messageIdentifier)
        {
            synchronousWaitForHSApiCallQueue();
            hsInboxJavaInstance.Call("markInboxMessageAsSeen", messageIdentifier);
        }

        public void DeleteInboxMessage(String messageIdentifier)
        {
            synchronousWaitForHSApiCallQueue();
            hsInboxJavaInstance.Call("deleteInboxMessage", messageIdentifier);
        }

        public void SetInboxMessageDelegate(IHelpshiftInboxDelegate externalDelegate)
        {
            // Wait for dex loading to complete since we are going to implement an interface from Helpshift SDK java code.
            synchronousWaitForHSApiCallQueue();
            HelpshiftAndroidInboxDelegate internalDelegate = new HelpshiftAndroidInboxDelegate(externalDelegate);
            hsInboxJavaInstance.Call("setInboxMessageDelegate", internalDelegate);
        }

        public void SetInboxPushNotificationDelegate(IHelpshiftInboxPushNotificationDelegate externalDelegate)
        {
            synchronousWaitForHSApiCallQueue();

            HelpshiftAndroidInboxPushNotificationDelegate internalDelegate = 
				                            new HelpshiftAndroidInboxPushNotificationDelegate(externalDelegate);

            hsInboxJavaInstance.Call("setInboxPushNotificationDelegate", internalDelegate);
        }
    }
}
#endif
