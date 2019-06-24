#if UNITY_ANDROID
using System;
using UnityEngine;

namespace Helpshift
{
	public class HelpshiftAndroidCampaignsDelegate : AndroidJavaProxy
	{
		private IHelpshiftCampaignsDelegate externalCampaignsDelegate;
		public HelpshiftAndroidCampaignsDelegate (IHelpshiftCampaignsDelegate externalDelegate) : 
			base ("com.helpshift.campaigns.HelpshiftCampaignsDelegate")
		{
			this.externalCampaignsDelegate = externalDelegate;
		}

		public void didReceiveUnreadMessagesCount(int count) {
			externalCampaignsDelegate.didReceiveUnreadMessagesCount(count);
		}

		public void sessionBegan() {
			externalCampaignsDelegate.sessionBegan();
		}

		public void sessionEnded() {
			externalCampaignsDelegate.sessionEnded();
		}
	}
}
#endif