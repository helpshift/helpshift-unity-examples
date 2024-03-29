/*
 * Copyright 2015, Helpshift, Inc.
 * All rights reserved
 */

#if UNITY_ANDROID
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HSMiniJSON;
using System.Linq;
using System.Threading;

namespace Helpshift
{
    public class HelpshiftCampaignsAndroid
    {

        private AndroidJavaClass hsCampaignsClass, hsCampaignsWrapperClass;
        private AndroidJavaObject currentActivity, application;

        private AndroidJavaObject convertToJavaHashMap(Dictionary<string, object> configD)
        {
            AndroidJavaObject config_Hashmap = new AndroidJavaObject("java.util.HashMap");
            if (configD != null)
            {
                Dictionary<string, object> configDict = (from kv in configD where kv.Value != null select kv).ToDictionary(kv => kv.Key, kv => kv.Value);
                IntPtr method_Put = AndroidJNIHelper.GetMethodID(config_Hashmap.GetRawClass(), "put",
                                                                "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
                object[] args = new object[2];
                args [0] = args [1] = null;
                foreach (KeyValuePair<string, object> kvp in configDict)
                {
					if (kvp.Key == null || kvp.Value == null) {
						continue;
					}

                    using (AndroidJavaObject k = new AndroidJavaObject("java.lang.String", kvp.Key))
                    {
                        args [0] = k;
						string valueType = kvp.Value.GetType().ToString();
                        if (valueType == "System.String")
                        {
                            args [1] = new AndroidJavaObject("java.lang.String", kvp.Value);
						} else if (valueType == "System.Int32")
                        {
                            args [1] = new AndroidJavaObject("java.lang.Integer", kvp.Value);
						} else if (valueType == "System.Int64")
						{
							args [1] = new AndroidJavaObject("java.lang.Long", kvp.Value);
						} else if (valueType == "System.Boolean")
                        {
                            args [1] = new AndroidJavaObject("java.lang.Boolean", kvp.Value);
						} else if (valueType == "System.DateTime")
                        {
                            double milliseconds = new TimeSpan(((DateTime)kvp.Value).Ticks).TotalMilliseconds;
                            DateTime datetime = new DateTime(1970, 1, 1);
                            double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
                            AndroidJavaObject objDouble = new AndroidJavaObject("java.lang.Double", millisecondsFrom1970);
                            long longDate = objDouble.Call<long>("longValue");
                            args [1] = new AndroidJavaObject("java.util.Date", longDate);
                        }
                        if (args [1] != null)
                        {
                            AndroidJNI.CallObjectMethod(config_Hashmap.GetRawObject(),
                                                        method_Put, AndroidJNIHelper.CreateJNIArgArray(args));
                        }
                    }
                }
            }
            return config_Hashmap;
        }

        public HelpshiftCampaignsAndroid()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            this.currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
			hsCampaignsClass = new AndroidJavaClass ("com.helpshift.campaigns.Campaigns");
            hsCampaignsWrapperClass = new AndroidJavaClass ("com.helpshift.campaigns.HelpshiftUnityCampaigns");
        }

        bool hsCampaignsApiCallAndReturnBool(string api, params object[] args)
        {
            if (args != null)
            {
                return hsCampaignsClass.CallStatic<bool>(api, args);
            } else
            {
                return hsCampaignsClass.CallStatic<bool>(api);
            }
        }

        int hsCampaignsApiCallAndReturnInt(string api, params object[] args)
        {
            if (args != null)
            {
                return hsCampaignsClass.CallStatic<int>(api, args);
            } else
            {
                return hsCampaignsClass.CallStatic<int>(api);
            }
        }

        AndroidJavaObject hsCampaignsApiCallAndReturnObject(string api, params object[] args)
        {
            if (args != null)
            {
                return hsCampaignsClass.CallStatic<AndroidJavaObject>(api, args);
            } else
            {
                return hsCampaignsClass.CallStatic<AndroidJavaObject>(api);
            }
        }

        public bool AddProperty(string key, int value)
        {
            return  hsCampaignsApiCallAndReturnBool("addProperty", new object[]
            {
                key,
                new AndroidJavaObject("java.lang.Integer", value)
            });
        }

		public bool AddProperty(string key, long value)
		{
			return  hsCampaignsApiCallAndReturnBool("addProperty", new object[]
			{
				key,
				new AndroidJavaObject("java.lang.Long", value)
			});
		}


        public bool AddProperty(string key, string value)
        {
            return hsCampaignsApiCallAndReturnBool("addProperty", new object[]
            {
                key,
                new AndroidJavaObject("java.lang.String", value)
            });
        }

        public bool AddProperty(string key, bool value)
        {
            return hsCampaignsApiCallAndReturnBool("addProperty", new object[]
            {
                key,
                new AndroidJavaObject("java.lang.Boolean", value)
            });
        }

        public bool AddProperty(string key, System.DateTime value)
        {
            double milliseconds = new TimeSpan((value).Ticks).TotalMilliseconds;
            DateTime datetime = new DateTime(1970, 1, 1);
            double millisecondsFrom1970 = milliseconds - (new TimeSpan(datetime.Ticks)).TotalMilliseconds;
            AndroidJavaObject objDouble = new AndroidJavaObject("java.lang.Double", millisecondsFrom1970);
            long longDate = objDouble.Call<long>("longValue");
            return hsCampaignsApiCallAndReturnBool("addProperty", new object[]
            {
                key,
                new AndroidJavaObject("java.util.Date", longDate)
            });
        }

        public string[] AddProperties(Dictionary<string,object> value)
        {
            AndroidJavaObject keys = (hsCampaignsApiCallAndReturnObject("addProperties", new object[] {convertToJavaHashMap(value)}));
            string[] keyValues = AndroidJNIHelper.ConvertFromJNIArray<string[]>(keys.GetRawObject());
            return keyValues;
        }

        public void ShowInbox(Dictionary<string, object> configMap)
        {
			hsCampaignsClass.CallStatic("showInbox", new object [] {this.currentActivity});
        }

        public int GetCountOfUnreadMessages()
        {
            return hsCampaignsApiCallAndReturnInt("getCountOfUnreadMessages", null);
        }

		public void RequestUnreadMessagesCount()
		{
			hsCampaignsWrapperClass.CallStatic("requestUnreadMessagesCount");
		}

		public void ShowMessage(String messageIdentifier, Dictionary<string, object> configMap)
        {
			hsCampaignsClass.CallStatic("showMessage", new object [] {messageIdentifier, this.currentActivity});
        }

        public void SetInboxMessagesDelegate(IHelpshiftInboxDelegate inboxDelegate)
        {
            HelpshiftAndroidInboxDelegate internalDelegate = new HelpshiftAndroidInboxDelegate(inboxDelegate);
			hsCampaignsClass.CallStatic("setInboxMessageDelegate", new object[]{internalDelegate});
        }

		public void SetCampaignsDelegate(IHelpshiftCampaignsDelegate campaignsDelegate)
		{
			HelpshiftAndroidCampaignsDelegate internalDelegate = new HelpshiftAndroidCampaignsDelegate(campaignsDelegate);
			hsCampaignsWrapperClass.CallStatic("setCampaignsDelegate", new object[]{internalDelegate});
		}
    }
}
#endif
