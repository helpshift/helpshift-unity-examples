using System;
using UnityEngine;

namespace Helpshift
{
    public enum HelpshiftInboxMessageActionType
    {
        UNKNOWN,
        OPEN_DEEP_LINK,
        SHOW_FAQS,
        SHOW_FAQ_SECTION,
        SHOW_CONVERSATION,
        SHOW_SINGLE_FAQ,
        SHOW_ALERT_TO_RATE_APP
    }

    public interface HelpshiftInboxMessage
    {
        /**
   		 * Gets identifier of the inbox message.
         *
         * @return message identifier
        */
        String GetIdentifier();

        /**
         * Gets the file path of the cover image for the inbox message.
         * returns null if cover image is not available or if its download is in progress.
		*/
        String GetCoverImageFilePath();

        /**
         * Gets the file path of the icon image for the inbox message.
         * returns null if icon image is not available or if its download is in progress.
         *
        */
        String GetIconImageFilePath();

        /**
         * Gets title of the inbox message.
         *
        */
        String GetTitle();

        /**
         * Gets title color of the inbox message in hex string format.
         *
        */
        String GetTitleColor();

        /**
         * Gets body of the inbox message.
         *
        */
        String GetBody();

        /**
         * Gets body color of the inbox message in hex string format.
        */
        String GetBodyColor();

        /**
         * Gets background color of the inbox message in hex string format.
        */
        String GetBackgroundColor();

        /**
         * Gets creation time of message in millisecond
        */
        double GetCreatedAt();

        /**
         * Gets expiry time of message in seconds
        */
        double GetExpiryTimeStamp();

        /**
		 * Returns true if the Inbox message has an expiry timestamp,
		 * false otherwise.
		 */
        bool HasExpiryTimestamp();

        /**
         * Gets the read status of message as boolean value where true means message is read by user.
        */
        bool GetReadStatus();

        /**
         * Gets the seen status of message as boolean value where true means message is seen by user.
        */
        bool GetSeenStatus();

        /**
         * Gets the count of action buttons in the message.
        */
        int GetCountOfActions();

        /**
         * Gets the title of action button at given index
         *
         * @param index index of action button
		*/
        String GetActionTitle(int index);

        /**
         * Gets the color for title of action button at given index in hex string format
         *
         * @param index index of action button
	    */
        String GetActionTitleColor(int index);

        /**
         * Gets the boolean value for action button at given index depicting whether
         * the action is treated as goal completion.
         *
         * @param index index of action button
		*/
        bool IsActionGoalCompletion(int index);

        /**
         * Perform the action on clicking action button for a given index
         *
         * @param index    index of the action button for which action has to be performed
         * @param activity Activity which contain the action button
        */
        void ExecuteAction(int index, System.Object activity);

        /**
         * Gets the title of action button at given index
         *
         * @param index index of action button
        */
        HelpshiftInboxMessageActionType GetActionType(int index);

        /**
   		 * Gets the data to perform the action at given index
   		 * The relationship between action type and action data:
   		 *
   		 * Action type : HelpshiftInboxMessageActionTypes#OPEN_DEEP_LINK
   		 * Action Data : URL String for the deep link.
   		 * 
   		 * Action type : HelpshiftInboxMessageActionTypes#SHOW_FAQS
   		 * Data : null
   		 * 
   		 * Action type : HelpshiftInboxMessageActionTypes#SHOW_FAQ_SECTION
   		 * Action Data : Section Publish Id String
   		 * 
   		 * Action type : HelpshiftInboxMessageActionTypes#SHOW_SINGLE_FAQ
   		 * Action Data : FAQ Publish Id String
  		 *
   		 * Action type : HelpshiftInboxMessageActionTypes#SHOW_CONVERSATION
   		 * Action Data : Pre-fill text string (Optional)
   		 * 
   		 * Action type : HelpshiftInboxMessageActionTypes#SHOW_ALERT_TO_RATE_APP
   		 * Action Data : App Review URL string
		 *
         * @param index index of the action button for which action has to be performed
		*/
        String GetActionData(int index);
    }
}

