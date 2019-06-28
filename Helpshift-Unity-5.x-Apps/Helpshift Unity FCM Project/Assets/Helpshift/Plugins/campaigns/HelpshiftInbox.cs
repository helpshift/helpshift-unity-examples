using System;
using System.Collections.Generic;
namespace Helpshift
{
    /**
	 * Inbox class.
	 */
    public class HelpshiftInbox
    {
#if UNITY_IOS || UNITY_ANDROID
        private static HelpshiftInbox instance = null;
#endif

#if UNITY_IOS
		private static HelpshiftInboxiOS nativeSdk = null;
#elif UNITY_ANDROID
        private static HelpshiftInboxAndroid nativeSdk = null;
#endif

        private HelpshiftInbox()
        {
        }

        /// <summary>
        /// Main function which should be used to get the HelpshiftInbox instance.
        /// </summary>
        /// <returns>Singleton HelpshiftInbox instance</returns>
        public static HelpshiftInbox getInstance()
        {
#if UNITY_IOS || UNITY_ANDROID
            if (instance == null)
            {
                instance = new HelpshiftInbox();
#if UNITY_IOS
				nativeSdk = new HelpshiftInboxiOS();
#elif UNITY_ANDROID
                nativeSdk = new HelpshiftInboxAndroid();
#endif
            }
            return instance;
#else
            return null;
#endif
        }

        /**
		 * Gets all Inbox messages.
		 */
        public List<HelpshiftInboxMessage> GetAllInboxMessages()
        {
#if UNITY_IOS || UNITY_ANDROID
            return nativeSdk.GetAllInboxMessages();
#else
            return null;
#endif
        }

        /**
		 * Get the Inbox message for messageIdentifier.
		 */
        public HelpshiftInboxMessage GetInboxMessage(String messageIdentifier)
        {
#if UNITY_IOS || UNITY_ANDROID
            return nativeSdk.GetInboxMessage(messageIdentifier);
#else
            return null;
#endif
        }

        /**
		 * Mark the inbox message with messageIdentifier as read.
		 */
        public void MarkInboxMessageAsRead(String messageIdentifier)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.MarkInboxMessageAsRead(messageIdentifier);
#endif
        }

        /**
		 * Mark the inbox message with messageIdentifier as seen.
		 */
        public void MarkInboxMessageAsSeen(String messageIdentifier)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.MarkInboxMessageAsSeen(messageIdentifier);
#endif
        }

        /**
		 * Delete the inbox message with messageIdentifier.
		 */
        public void DeleteInboxMessage(String messageIdentifier)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.DeleteInboxMessage(messageIdentifier);
#endif
        }

        public void SetInboxMessageDelegate(IHelpshiftInboxDelegate externalDelegate)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.SetInboxMessageDelegate(externalDelegate);
#endif
        }

        public void SetInboxPushNotificationDelegate(IHelpshiftInboxPushNotificationDelegate externalDelegate)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.SetInboxPushNotificationDelegate(externalDelegate);
#endif
        }
    }
}

