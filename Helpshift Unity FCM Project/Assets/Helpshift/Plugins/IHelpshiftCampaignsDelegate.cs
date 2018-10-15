using System;

namespace Helpshift
{
	public interface IHelpshiftCampaignsDelegate
	{
		/**
		 * Callback method for HelpshiftCampaigns.RequestUnreadMessagesCount() API.
		 */
		void didReceiveUnreadMessagesCount(int count);
	}
}

