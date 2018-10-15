/*
 * Copyright 2015, Helpshift, Inc.
 * All rights reserved
 */

#if UNITY_IPHONE
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using HSMiniJSON;

namespace Helpshift.Campaigns
{
    public class HelpshiftCampaignsiOS
    {
        [DllImport ("__Internal")]
        private static extern bool hsAddPropertyInteger(string key, int value);

		[DllImport ("__Internal")]
		private static extern bool hsAddPropertyLong(string key, long value);

		[DllImport ("__Internal")]
        private static extern bool hsAddPropertyString(string key, string value);

		[DllImport ("__Internal")]
        private static extern bool hsAddPropertyBoolean(string key, bool value);

		[DllImport ("__Internal")]
        private static extern bool hsAddPropertyDate(string key, double value);

		[DllImport ("__Internal")]
        private static extern string hsAddProperties(string value);

		[DllImport ("__Internal")]
        private static extern void hsShowInboxWithOptions(string optionsDictionaryString);

		[DllImport ("__Internal")]
        private static extern int hsGetCountOfUnreadMessages();

		[DllImport ("__Internal")]
        private static extern void hsRefetchMessages();

		[DllImport ("__Internal")]
		private static extern void hsShowInboxMessage (string messageIdentifier, string optionsDictionaryString);

		[DllImport ("__Internal")]
		private static extern void hsRequestCampaignsUnreadMessagesCount();

		[DllImport ("__Internal")]
		private static extern void hsSetCampaignsDelegate();

        public HelpshiftCampaignsiOS()
        {
        }

        private Dictionary<string,object> checkDateTime(Dictionary<string,object> value)
        {
            Dictionary<string,object> valuesDict = new Dictionary<string,object>();
            Dictionary<string,object> dateTimeDict = new Dictionary<string,object>();
            foreach (KeyValuePair<string, object> kvp in value)
            {
                if (kvp.Value.GetType().ToString() == "System.DateTime")
                {
                    double milliseconds = new TimeSpan(((DateTime)kvp.Value).Ticks).TotalMilliseconds;
                    DateTime datetime = new DateTime(1970, 1, 1);
                    double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
                    dateTimeDict.Add(kvp.Key, millisecondsFrom1970);
                } else
                {
                    valuesDict.Add(kvp.Key, kvp.Value);
                }
            }
            valuesDict.Add("__hsDateTime", dateTimeDict);
            return valuesDict;
        }

        public bool AddProperty(string key, int value)
        {
            return hsAddPropertyInteger(key, value);
        }

		public bool AddProperty(string key, long value)
		{
			return hsAddPropertyLong(key, value);
		}

        public bool AddProperty(string key, string value)
        {
            return hsAddPropertyString(key, value);
        }

        public bool AddProperty(string key, bool value)
        {
            return hsAddPropertyBoolean(key, value);
        }

        public bool AddProperty(string key, System.DateTime value)
        {
            double milliseconds = new TimeSpan((value).Ticks).TotalMilliseconds;
            DateTime datetime = new DateTime(1970, 1, 1);
            double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
            return hsAddPropertyDate(key, millisecondsFrom1970);
        }

        public string[] AddProperties(Dictionary<string,object> value)
        {
            Dictionary<string,object> allVlaues = checkDateTime(value);
            List<object> keyValues = (List<object>)Json.Deserialize(hsAddProperties(Json.Serialize(allVlaues)));
            return (string[])keyValues.ConvertAll(x => x.ToString()).ToArray();
            ;
        }

        public void ShowInbox(Dictionary<string, object> configMap)
        {
            hsShowInboxWithOptions(Json.Serialize(configMap));
        }

        public int GetCountOfUnreadMessages()
        {
            return hsGetCountOfUnreadMessages();
        }

		public void RequestUnreadMessagesCount() {
			hsRequestCampaignsUnreadMessagesCount ();
		}

        public void RefetchMessages()
        {
            hsRefetchMessages();
        }

        public void SetInboxMessagesDelegate(IHelpshiftInboxDelegate externalInboxDelegate)
        {
            HelpshiftiOSInboxDelegate internalInboxDelegate = new HelpshiftiOSInboxDelegate(externalInboxDelegate);
            HelpshiftiOSInboxDelegatesHolder.GetInstance().setInboxDelegate(internalInboxDelegate);
        }

		public void SetCampaignsDelegate(IHelpshiftCampaignsDelegate externalCampaignsDelegate)
		{
			hsSetCampaignsDelegate ();
			HelpshiftiOSCampaignsDelegate internalCampaignsDeleagate = new HelpshiftiOSCampaignsDelegate (externalCampaignsDelegate);
			HelpshiftiOSInboxDelegatesHolder.GetInstance().setCampaignsDelegate(internalCampaignsDeleagate);
		}

		public void ShowMessage(string messageIdentifier, Dictionary<string, object> configMap) {
			hsShowInboxMessage (messageIdentifier, Json.Serialize(configMap));
		}
    }
}
#endif
