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
using System.Collections;
using System.Threading;

namespace Helpshift
{
    public class HelpshiftAndroid
    {

        private AndroidJavaClass jc;
        private AndroidJavaObject currentActivity, application;
        private AndroidJavaClass hsHelpshiftClass;


        public HelpshiftAndroid()
        {
            this.jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            this.currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            this.application = currentActivity.Call<AndroidJavaObject>("getApplication");
			this.hsHelpshiftClass = new AndroidJavaClass("com.helpshift.HelpshiftUnityAPI");
        }

        public void install(string apiKey, string domain, string appId, Dictionary<string, object> configMap)
        {
            string jsonSerializedConfig = Json.Serialize(configMap);
			hsHelpshiftClass.CallStatic ("install", new object[] { this.application, apiKey, domain, appId, jsonSerializedConfig });

			HelpshiftInternalLogger.d("Install called : Domain : " + domain + ", Config : " + jsonSerializedConfig);
        }

		public void requestUnreadMessagesCount(Boolean isAsync)
		{
			HelpshiftInternalLogger.d("Call requestUnreadMessagesCount: isAsync : " + isAsync);

			hsHelpshiftClass.CallStatic("requestUnreadMessagesCount", isAsync);
        }

		[Obsolete]
        public void setNameAndEmail(string userName, string email)
        {
			hsHelpshiftClass.CallStatic ("setNameAndEmail", new object[] { userName, email });
        }

		[Obsolete]
        public void setUserIdentifier(string identifier)
        {
			hsHelpshiftClass.CallStatic("setUserIdentifier", identifier);
        }

        public void registerDeviceToken(string deviceToken)
        {
			HelpshiftInternalLogger.d("Register device token :" + deviceToken);
			hsHelpshiftClass.CallStatic("registerDeviceToken", new object [] {this.currentActivity, deviceToken});
        }

        public void leaveBreadCrumb(string breadCrumb)
        {
			hsHelpshiftClass.CallStatic("leaveBreadCrumb", breadCrumb);
        }

        public void clearBreadCrumbs()
        {
			hsHelpshiftClass.CallStatic("clearBreadCrumbs");
        }

		[Obsolete("Use the login(HelpshiftUser user) api instead.")]
        public void login(string identifier, string userName, string email)
        {
			HelpshiftInternalLogger.d("Login called : " + userName);
			hsHelpshiftClass.CallStatic("login", new object[] {identifier, userName, email});
        }

		public void login(HelpshiftUser helpshiftUser)
		{
			HelpshiftInternalLogger.d("Login called : " + helpshiftUser.name);
			hsHelpshiftClass.CallStatic("loginHelpshiftUser", new object[] {jsonifyHelpshiftUser(helpshiftUser)});
		}

		public void clearAnonymousUser()
		{
			hsHelpshiftClass.CallStatic("clearAnonymousUser");
		}

        public void logout()
        {
			hsHelpshiftClass.CallStatic("logout");
        }

        private string serializeApiConfig(Dictionary<string, object> configMap) {
            if (configMap != null) {
                return Json.Serialize(cleanConfig(configMap));
            }
            return null;
        }
        public void showConversation(Dictionary<string, object> configMap)
        {
			hsHelpshiftClass.CallStatic("showConversationUnity", new object [] {this.currentActivity, serializeApiConfig(configMap)});
        }

        public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap)
        {

			hsHelpshiftClass.CallStatic("showFAQSectionUnity", new object[]{this.currentActivity, sectionPublishId, serializeApiConfig(configMap)});
        }

        public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap)
        {
			hsHelpshiftClass.CallStatic("showSingleFAQUnity", new object[]{
					this.currentActivity,
					questionPublishId,
					serializeApiConfig(configMap)
				});
        }

        public void showFAQs(Dictionary<string, object> configMap)
        {
			hsHelpshiftClass.CallStatic("showFAQsUnity", new object [] { this.currentActivity, serializeApiConfig(configMap)});
        }

        public void updateMetaData(Dictionary<string, object> metaData)
        {
			hsHelpshiftClass.CallStatic("setMetaData", Json.Serialize(metaData));
        }

		private Dictionary<string, object> cleanConfig(Dictionary<string, object> configMap)
		{
	    	if (configMap.ContainsKey("customIssueFields")) {
				configMap ["hs-custom-issue-field"] = configMap["customIssueFields"];
				configMap.Remove ("customIssueFields");
	    	}
			return configMap;
		}

        public void handlePushNotification(Dictionary<string, object> pushNotificationData)
        {
			HelpshiftInternalLogger.d("Handle push notification : data :" + pushNotificationData.ToString());
			hsHelpshiftClass.CallStatic("handlePush", new object[] {this.currentActivity, Json.Serialize(pushNotificationData)});
        }

        public void showAlertToRateAppWithURL(string url)
        {
			hsHelpshiftClass.CallStatic("showAlertToRateApp", url);
        }

        public void registerDelegates()
        {
			HelpshiftInternalLogger.d("Registering delegates");

			hsHelpshiftClass.CallStatic("registerDelegates");
        }

        public void setSDKLanguage(string locale)
        {
			hsHelpshiftClass.CallStatic("setSDKLanguage", new object[] {locale});
        }

        public void setTheme(string themeResourceName)
        {
            hsHelpshiftClass.CallStatic("setTheme", new object[] { themeResourceName });
        }

        public void showDynamicForm(string title, Dictionary<string, object>[] flows)
        {
			hsHelpshiftClass.CallStatic("showDynamicFormFromDataJson", new object[]
				{
					this.currentActivity,
					title,
					Json.Serialize(flows)
				});
        }

		public void checkIfConversationActive()
		{
			this.hsHelpshiftClass.CallStatic("checkIfConversationActive");
		}

        public void onApplicationQuit()
        {
			HelpshiftInternalLogger.d("onApplicationQuit");
        }

		private string jsonifyHelpshiftUser(HelpshiftUser helpshiftUser) {
			Dictionary<string, string> helpshiftUserDataMap = new Dictionary<string, string> ();
			helpshiftUserDataMap.Add ("identifier", helpshiftUser.identifier);
			helpshiftUserDataMap.Add ("email", helpshiftUser.email);
			helpshiftUserDataMap.Add ("name", helpshiftUser.name);
			helpshiftUserDataMap.Add ("authToken", helpshiftUser.authToken);
			return Json.Serialize (helpshiftUserDataMap);
		}
    }

    public class HelpshiftAndroidLog
    {
		private static AndroidJavaClass logger = new AndroidJavaClass("com.helpshift.support.Log");

        private HelpshiftAndroidLog()
        {
        }

        public static int v(String tag, String log)
        {
			return logger.CallStatic<int>("v", new object[] {tag, log});
        }

        public static int d(String tag, String log)
        {
            return logger.CallStatic<int>("d", new object[] {tag, log});
        }

        public static int i(String tag, String log)
        {
            return logger.CallStatic<int>("i", new object[] {tag, log});
        }

        public static int w(String tag, String log)
        {
            return logger.CallStatic<int>("w", new object[] {tag, log});
        }

        public static int e(String tag, String log)
        {
            return logger.CallStatic<int>("e", new object[] {tag, log});
        }
    }
}
#endif
