using System;
namespace Helpshift
{
    public interface IHelpshiftInboxPushNotificationDelegate
    {
        /**
         * Delegate method which is called when push notification for new inbox message is tapped
         *
         * @param messageIdentifier message identifier
         */
        void OnInboxMessagePushNotificationClicked(String messageIdentifier);
    }
}

