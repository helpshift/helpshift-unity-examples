using System;

namespace Helpshift
{
    public interface IHelpshiftCampaignsDelegate
    {
        /// Callback method for HelpshiftCampaigns.RequestUnreadMessagesCount() API.
        void didReceiveUnreadMessagesCount(int count);

        /// Invoked when the user enters a Campaigns screen.
        /// Note : This callback is invoked on the UI thread. DO NOT do heavy operations on this callback.
        void sessionBegan();

        /// Invoked when the user exits all the Campaigns screens.
        /// Note : This callback is invoked on the UI thread. DO NOT do heavy operations on this callback.
        void sessionEnded();
    }
}

