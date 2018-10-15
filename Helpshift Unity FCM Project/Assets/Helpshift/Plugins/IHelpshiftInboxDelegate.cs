using System;
namespace Helpshift
{
    public interface IHelpshiftInboxDelegate
    {
        void InboxMessageAdded(HelpshiftInboxMessage message);

        /**
         * Delegate method which is called when icon image download is completed for a message
         *
         * @param messageIdentifier message identifier
        */
        void IconImageDownloaded(String messageIdentifier);

        /**
         * Delegate method which is called when cover image download is completed for a message
         *
         * @param messageIdentifier message identifier
        */
        void CoverImageDownloaded(String messageIdentifier);

        /**
         * Delegate method which is called when a message is deleted
         *
         * @param messageIdentifier message identifier
        */
        void InboxMessageDeleted(String messageIdentifier);

        /**
         * Delegate method which is called when a message is marked as seen
         *
         * @param messageIdentifier message identifier
        */
        void InboxMessageMarkedAsSeen(String messageIdentifier);

        /**
         * Delegate method which is called when a message is marked as read
         *
         * @param messageIdentifier message identifier
        */
        void InboxMessageMarkedAsRead(String messageIdentifier);

		#if UNITY_IOS
		/**
 		*  Delegate method which is called when the Helpshift SDK fails to add a Campaign message.
 		*
 		*  @param identifier identifier of the failed message.
 		*/
		void FailedToAddInboxMessageWithId(String messageIdentifier);
		#endif
    }
}

