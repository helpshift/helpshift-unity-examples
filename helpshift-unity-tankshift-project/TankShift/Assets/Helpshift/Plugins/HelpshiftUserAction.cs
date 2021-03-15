using System;

namespace Helpshift
{
    /// <summary>
    /// Class to define a Helpshift user action on action card bots.
    /// </summary>
    public class HelpshiftUserAction
    {
        /// Type of action clicked by the user. Can be "call", "link"
        public readonly string actionType;

        /// Data with respect to the action. Can be a phone number, url link etc
        public readonly string actionData;

        public HelpshiftUserAction(string actionType, string actionData)
        {
            this.actionType = actionType;
            this.actionData = actionData;
        }
    }
}