using System;
using Helpshift;
using UnityEngine;

namespace HelpshiftExample
{
	public class CampaignsDelegate : IHelpshiftCampaignsDelegate
	{
		public void didReceiveUnreadMessagesCount(int count) {
			Debug.Log ("Campaigns : didReceiveUnreadMessagesCount : " + count);
		}

		public void sessionBegan()
        {
            Debug.Log("Helpshift Campaigns Session Has Begun ************************************************************");
        }

		public void sessionEnded()
        {
            Debug.Log("Helpshift Campaigns Session Has Ended ************************************************************");
        }
	}
}

