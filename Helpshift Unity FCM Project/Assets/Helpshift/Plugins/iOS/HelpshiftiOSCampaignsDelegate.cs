#if UNITY_IOS
using System;
using UnityEngine;
namespace Helpshift
{
	public class HelpshiftiOSCampaignsDelegate
	{
		private IHelpshiftCampaignsDelegate externalCampaignsDelegate;
		public HelpshiftiOSCampaignsDelegate (IHelpshiftCampaignsDelegate externalCampaignsDelegate)
		{
			this.externalCampaignsDelegate = externalCampaignsDelegate;
		}

		public void didReceiveUnreadMessagesCount(int count)
		{
			externalCampaignsDelegate.didReceiveUnreadMessagesCount (count);
		}
	}
}
#endif