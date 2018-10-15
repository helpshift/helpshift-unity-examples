#if UNITY_ANDROID
using System;
using UnityEngine;


namespace Helpshift
{
    // Android implementation for InboxMessage.
    public class HelpshiftAndroidInboxMessage : HelpshiftInboxMessage
    {
        // Java object of InboxMessage.
        private AndroidJavaObject inboxMessageJavaInstance;

        // Java InboxMessage interface for accessing static fields.
        private static AndroidJavaClass inboxInterfaceClass;

        private String identifier;
        private String title;
        private String titleColor;
        private String body;
        private String bodyColor;
        private String backgroundColor;
        private long createdAt;
        private long expiryTimeStamp;
        private bool readStatus;
        private bool seenStatus;
        private int actionsCount;

        private HelpshiftAndroidInboxMessage()
        {
            // Lazily init inboxInterfaceClass since we need to be sure that this class is loaded
            // even with dex loading delays.
            if (inboxInterfaceClass == null || inboxInterfaceClass.GetRawObject().ToInt32() == 0)
            {
                inboxInterfaceClass = new AndroidJavaClass("com.helpshift.campaigns.models.InboxMessage");
            }
        }

        public String GetIdentifier()
        {
            return identifier;
        }

        public String GetCoverImageFilePath()
        {
			string coverImageFilePath = null;
            try
            {
                coverImageFilePath = inboxMessageJavaInstance.Get<string>("coverImageFilePath");
            } catch (Exception e)
            {
                // Some error in java-C# binding.
                // Generally happens when returning null from java.
                Debug.Log("Error getting inbox message cover image :" + e.Message);
            }

			try {
				// Invoke native getCoverImage native api to trigger lazy image download.
				inboxMessageJavaInstance.Call<AndroidJavaObject>("getCoverImage");
			}
			catch (Exception e) {
				// Some error in java-C# binding.
				// Generally happens when returning null from java.
				Debug.Log("Expected error. Java returning null for message cover image bitmap:" + e.Message);
			}
			
			return coverImageFilePath;
        }

        public String GetIconImageFilePath()
        {
            try
            {
                string iconImageFilePath = inboxMessageJavaInstance.Get<string>("iconImageFilePath");
                return iconImageFilePath;
            } catch (Exception e)
            {
                // Some error in java-C# binding.
                // Generally happens when returning null from java.
                Debug.Log("Error getting inbox message icon image :" + e.Message);
            }
            return null;
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
            return Convert.ToDouble(createdAt);
        }

        public double GetExpiryTimeStamp()
        {
            return Convert.ToDouble(expiryTimeStamp);
        }

        public bool HasExpiryTimestamp()
        {
            return expiryTimeStamp != inboxInterfaceClass.GetStatic<long>("NO_EXPIRY_TIME_STAMP");
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
            return inboxMessageJavaInstance.Call<String>("getActionTitle", index);
        }

        public String GetActionTitleColor(int index)
        {
            return inboxMessageJavaInstance.Call<String>("getActionTitleColor", index);
        }

        public bool IsActionGoalCompletion(int index)
        {
			return inboxMessageJavaInstance.Call<bool>("isActionGoalCompletion", index);
        }

        public void ExecuteAction(int index, System.Object activity)
        {
			// Fallback to unity's default activity if null. 
            if (activity == null) {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            inboxMessageJavaInstance.Call("executeAction", index, activity);
        }

        public String GetActionData(int index)
        {
            return inboxMessageJavaInstance.Call<String>("getActionData", index);
        }

        public HelpshiftInboxMessageActionType GetActionType(int index)
        {
            AndroidJavaObject actionType = inboxMessageJavaInstance.Call<AndroidJavaObject>("getActionType", index);
            int type = actionType.Call<int>("ordinal");
            HelpshiftInboxMessageActionType actionTypeEnum = HelpshiftInboxMessageActionType.UNKNOWN;

            // Resolve the action type we get from native android.
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
                actionTypeEnum = HelpshiftInboxMessageActionType.SHOW_CONVERSATION;
            } else if (type == 5)
            {
                actionTypeEnum = HelpshiftInboxMessageActionType.SHOW_SINGLE_FAQ;
            } else if (type == 6)
            {
                actionTypeEnum = HelpshiftInboxMessageActionType.SHOW_ALERT_TO_RATE_APP;
            }

            return actionTypeEnum;
        }

        /**
         * Maps the java interface "com.helpshift.campaigns.models.InboxMessage" to a C# object.
         */ 
        public static HelpshiftAndroidInboxMessage createInboxMessage(AndroidJavaObject message)
        {

            if (message == null || message.GetRawObject().ToInt32() == 0)
            {
                return null;
            }

            HelpshiftAndroidInboxMessage androidInboxMessage = new HelpshiftAndroidInboxMessage();

            androidInboxMessage.inboxMessageJavaInstance = message;
            androidInboxMessage.identifier = message.Call<String>("getIdentifier");
            androidInboxMessage.title = message.Call<String>("getTitle");
            androidInboxMessage.titleColor = message.Call<String>("getTitleColor");
            androidInboxMessage.body = message.Call<String>("getBody");
            androidInboxMessage.bodyColor = message.Call<String>("getBodyColor");
            androidInboxMessage.backgroundColor = message.Call<String>("getBackgroundColor");
            androidInboxMessage.createdAt = message.Call<long>("getCreatedAt");
            androidInboxMessage.expiryTimeStamp = message.Call<long>("getExpiryTimeStamp");
            androidInboxMessage.seenStatus = message.Call<bool>("getSeenStatus");
            androidInboxMessage.readStatus = message.Call<bool>("getReadStatus");
            androidInboxMessage.actionsCount = message.Call<int>("getCountOfActions");

            return androidInboxMessage;
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
