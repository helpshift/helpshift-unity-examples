#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Helpshift
{
    // iOS implementation for InboxMessage.
    public class HelpshiftiOSInboxMessage : HelpshiftInboxMessage
    {
        private String identifier;
        private String title;
        private String titleColor;
        private String body;
        private String bodyColor;
        private String iconImageLocalURL;
        private String coverImageLocalURL;
        private String backgroundColor;
        private double createdAt;
        private double expiryTimeStamp;
        private bool readStatus;
        private bool seenStatus;
        private int actionsCount;
        private List<object> actions;

        [DllImport ("__Internal")]
        private static extern void hsExecuteActionAtIndex(string identifier, int index);

		[DllImport ("__Internal")]
		private static extern void hsGetCoverImage(string identifier);

        public String GetIdentifier()
        {
            return identifier;
        }

        public String GetCoverImageFilePath()
        {
			hsGetCoverImage (identifier);
            return coverImageLocalURL;
        }

        public String GetIconImageFilePath()
        {
            return iconImageLocalURL;
        }

        public String GetTitle()
        {
            return title;
        }

        public String GetTitleColor()
        {
            return titleColor;
        }

        public String GetBody()
        {
            return body;
        }

        public String GetBodyColor()
        {
            return bodyColor;
        }

        public String GetBackgroundColor()
        {
            return backgroundColor;
        }

        public double GetCreatedAt()
        {
            return createdAt;
        }

        public double GetExpiryTimeStamp()
        {
            return expiryTimeStamp;
        }

        public bool HasExpiryTimestamp()
        {
            if (expiryTimeStamp == 0)
            {
                return false;
            }
            return true;
        }

        public bool GetReadStatus()
        {
            return readStatus;
        }

        public bool GetSeenStatus()
        {
            return seenStatus;
        }

        public int GetCountOfActions()
        {
            return actionsCount;
        }

        public String GetActionTitle(int index)
        {
            Dictionary <string,object> actionItem = (Dictionary <string,object>)actions [index];
            return Convert.ToString(actionItem ["actionTitle"]);
        }

        public String GetActionTitleColor(int index)
        {
            Dictionary <string,object> actionItem = (Dictionary <string,object>)actions [index];
            return Convert.ToString(actionItem ["actionTitleColour"]);
        }

        public bool IsActionGoalCompletion(int index)
        {
            Dictionary <string,object> actionItem = (Dictionary <string,object>)actions [index];
            return Convert.ToBoolean(actionItem ["actionGoalCompletion"]);
        }

        public void ExecuteAction(int index, System.Object activity)
        {
            hsExecuteActionAtIndex(identifier, index);
        }

        public String GetActionData(int index)
        {
            Dictionary <string,object> actionItem = (Dictionary <string,object>)actions [index];
            return Convert.ToString(actionItem ["actionData"]);
        }

        public HelpshiftInboxMessageActionType GetActionType(int index)
        {
            Dictionary <string,object> actionItem = (Dictionary <string,object>)actions [index];
            int type = Convert.ToInt32(actionItem ["actionType"]);
            HelpshiftInboxMessageActionType actionTypeEnum = HelpshiftInboxMessageActionType.UNKNOWN;
            if (type == 0)
            {
                actionTypeEnum = HelpshiftInboxMessageActionType.UNKNOWN;
            } else if (type == 1)
            {
                actionTypeEnum = HelpshiftInboxMessageActionType.OPEN_DEEP_LINK;
            } else if (type == 2)
            {
                actionTypeEnum = HelpshiftInboxMessageActionType.SHOW_FAQS;
            } else if (type == 3)
            {
                actionTypeEnum = HelpshiftInboxMessageActionType.SHOW_FAQ_SECTION;
            } else if (type == 4)
            {
                actionTypeEnum = HelpshiftInboxMessageActionType.SHOW_SINGLE_FAQ;
            } else if (type == 5)
            {
                actionTypeEnum = HelpshiftInboxMessageActionType.SHOW_CONVERSATION;
            } else if (type == 6)
            {
                actionTypeEnum = HelpshiftInboxMessageActionType.SHOW_ALERT_TO_RATE_APP;
            }
            return actionTypeEnum;
        }

        public static HelpshiftiOSInboxMessage createInboxMessage(string message)
        {
            Dictionary<string, object> inboxMessageJson = (Dictionary<string, object>)HSMiniJSON.Json.Deserialize(message);
            return HelpshiftiOSInboxMessage.HelpshiftiOSInboxMessageFromJSON(inboxMessageJson);
        }

        private static HelpshiftiOSInboxMessage HelpshiftiOSInboxMessageFromJSON(Dictionary<string, object> inboxMessageDict)
        {
            return new HelpshiftiOSInboxMessage {
				identifier = Convert.ToString(inboxMessageDict ["identifier"]),
				title = Convert.ToString(inboxMessageDict ["title"]),
				titleColor = Convert.ToString(inboxMessageDict ["titleColor"]),
				body = Convert.ToString(inboxMessageDict ["body"]),
				bodyColor = Convert.ToString(inboxMessageDict ["bodyColor"]),
				iconImageLocalURL = Convert.ToString(inboxMessageDict ["iconImageLocalURL"]),
				coverImageLocalURL = Convert.ToString(inboxMessageDict ["coverImageLocalURL"]),
				backgroundColor = Convert.ToString(inboxMessageDict ["backgroundColor"]),
				createdAt = Convert.ToDouble (inboxMessageDict ["createdAt"]),
				expiryTimeStamp = Convert.ToDouble (inboxMessageDict ["expiryTimeStamp"]),
				readStatus = Convert.ToBoolean(inboxMessageDict ["readStatus"]),
				seenStatus = Convert.ToBoolean(inboxMessageDict ["seenStatus"]),
				actionsCount = Convert.ToInt32 (inboxMessageDict ["actionsCount"]),
				actions = (List<object> )inboxMessageDict ["actions"],
			};
        }

		public override String ToString()
		{
			String stringValue = "Title : " + title + "  Identifier : " + identifier +
				    "\n Body : " + body + "  CreatedAt : " + createdAt +
					"\n Seen state :" + seenStatus + "  Read state : " + readStatus +
					"\n Count of Actions :" + actionsCount + "  Expiry : " + expiryTimeStamp +
					"\n BodyColor : " + bodyColor + " BackgroundColor : " + backgroundColor +
					"\n Action count : " + actionsCount;
			return stringValue;
		}
    }
}
#endif
