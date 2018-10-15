/*
 * Copyright 2015, Helpshift, Inc.
 * All rights reserved
 */

#if UNITY_IPHONE
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HSMiniJSON;

namespace Helpshift
{
    public class HelpshiftiOS
    {
        [DllImport ("__Internal")]
        private static extern void hsInstall();
        [DllImport ("__Internal")]
        private static extern void hsInstallForApiKey(string apiKey, string domainName, string appId);
        [DllImport ("__Internal")]
        private static extern void hsInstallForApiKeyWithOptions(string apiKey, string domainName, string appId, string jsonOptionsDict);

        [DllImport ("__Internal")]
        private static extern void hsShowFAQs();
        [DllImport ("__Internal")]
        private static extern void hsShowFAQsWithOptions(string jsonOptionsDict);
        [DllImport ("__Internal")]
        private static extern void hsShowFAQsWithMeta(string jsonOptionsDict);

        [DllImport ("__Internal")]
        private static extern void hsShowFAQSection(string id);
        [DllImport ("__Internal")]
        private static extern void hsShowFAQSectionWithOptions(string id, string jsonOptionsDict);
        [DllImport ("__Internal")]
        private static extern void hsShowFAQSectionWithMeta(string id, string jsonOptionsDict);

        [DllImport ("__Internal")]
        private static extern void hsShowConversation();
        [DllImport ("__Internal")]
        private static extern void hsShowConversationWithOptions(string jsonOptionsDict);
        [DllImport ("__Internal")]
        private static extern void hsShowConversationWithMeta(string jsonOptionsDict);

        [DllImport ("__Internal")]
        private static extern void hsShowSingleFAQ(string id);
        [DllImport ("__Internal")]
        private static extern void hsShowSingleFAQWithOptions(string id, string jsonOptionsDict);
        [DllImport ("__Internal")]
        private static extern void hsShowSingleFAQWithMeta(string id, string jsonOptionsDict);

        [DllImport ("__Internal")]
        private static extern void hsSetUserIdentifier(string userIdentifier);
        [DllImport ("__Internal")]
        private static extern void hsSetNameAndEmail(string userName, string email);
        [DllImport ("__Internal")]
        private static extern void hsRegisterDeviceToken(string deviceToken);
        [DllImport ("__Internal")]
        private static extern int  hsGetNotificationCountFromRemote(bool isRemote);
        [DllImport ("__Internal")]
        private static extern void hsLeaveBreadCrumb(string breadCrumb);
        [DllImport ("__Internal")]
        private static extern void hsClearBreadCrumbs();
        [DllImport ("__Internal")]
        private static extern void hsHandleLocalNotificationForIssue(string issueID);
        [DllImport ("__Internal")]
        private static extern void hsHandleRemoteNotificationForIssue(string issueID);
        [DllImport ("__Internal")]
        private static extern void hsHandleRemoteNotification(string issueID);
        [DllImport ("__Internal")]
        private static extern void hsSetMetaData(string meta);
        [DllImport ("__Internal")]
        private static extern void hsPauseDisplayOfInAppNotification(bool pauseInApp);
        [DllImport ("__Internal")]
        private static extern void hsShowAlertToRateAppWithURL(string url);
        [DllImport ("__Internal")]
        private static extern void hsLogin(string identifier, string name, string email);
		[DllImport ("__Internal")]
		private static extern void hsLoginWithUser(string helpshiftUser);
        [DllImport ("__Internal")]
        private static extern void hsLogout();
        [DllImport ("__Internal")]
		private static extern void hsClearAnonymousUser();
		[DllImport ("__Internal")]
        private static extern void hsShowDynamicForm(string title, string flowsJson);
        [DllImport ("__Internal")]
        private static extern bool hsIsConversationActive();

		[DllImport ("__Internal")]
		private static extern void hsSetLanguage(string language);
		[DllImport ("__Internal")]
		private static extern void hsRequestUnreadMessagesCount(bool isRemote);
		[DllImport ("__Internal")]
		private static extern void hsCheckIfConversationActive();


        public HelpshiftiOS()
        {
        }

        [Obsolete("Use HsUnityAppController.mm to initialize the Helpshift SDK",false)]
        public void install()
        {
            hsInstall();
        }
        [Obsolete("Use HsUnityAppController.mm to initialize the Helpshift SDK",false)]
        public void install(string apiKey, string domain, string appId, Dictionary<string, object> configMap)
        {
            if (configMap == null)
            {
                hsInstallForApiKey(apiKey, domain, appId);
            } else
            {
                hsInstallForApiKeyWithOptions(apiKey, domain, appId, Json.Serialize(configMap));
            }
        }
        [Obsolete("Use HsUnityAppController.mm to initialize the Helpshift SDK",false)]
        public void install(string apiKey, string domain, string appId)
        {
            hsInstallForApiKey(apiKey, domain, appId);
        }

        public int getNotificationCount(bool isRemote)
        {
            return hsGetNotificationCountFromRemote(isRemote);
        }

		public void requestUnreadMessagesCount(bool isRemote)
		{
			hsRequestUnreadMessagesCount(isRemote);
		}

		[Obsolete]
		public void setNameAndEmail(string userName, string email)
        {
            hsSetNameAndEmail(userName, email);
        }

		[Obsolete]
        public void setUserIdentifier(string identifier)
        {
            hsSetUserIdentifier(identifier);
        }

		[Obsolete("Use the login(HelpshiftUser user) api instead.")]
        public void login(string identifier, string name, string email)
        {
            hsLogin(identifier, name, email);
        }

		public void login(HelpshiftUser helpshiftUser) {
			hsLoginWithUser(jsonifyHelpshiftUser(helpshiftUser));
		}
        public void logout()
        {
            hsLogout();
        }

		public void clearAnonymousUser() {
			hsClearAnonymousUser ();
		}

        public void registerDeviceToken(string deviceToken)
        {
            hsRegisterDeviceToken(deviceToken);
        }

        public void leaveBreadCrumb(string breadCrumb)
        {
            hsLeaveBreadCrumb(breadCrumb);
        }

        public void clearBreadCrumbs()
        {
            hsClearBreadCrumbs();
        }

        public void showConversation(Dictionary<string, object> configMap)
        {
            hsShowConversationWithOptions(Json.Serialize(configMap));
        }
        public void showConversation()
        {
            hsShowConversation();
        }
        public void showConversationWithMeta(Dictionary<string, object> configMap)
        {
            hsShowConversationWithMeta(Json.Serialize(configMap));
        }

        public void showFAQs()
        {
            hsShowFAQs();
        }
        public void showFAQs(Dictionary<string, object> configMap)
        {
            hsShowFAQsWithOptions(Json.Serialize(configMap));
        }
        public void showFAQsWithMeta(Dictionary<string, object> configMap)
        {
            hsShowFAQsWithMeta(Json.Serialize(configMap));
        }

        public void showFAQSection(string sectionPublishId)
        {
            hsShowFAQSection(sectionPublishId);
        }
        public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap)
        {
            hsShowFAQSectionWithOptions(sectionPublishId, Json.Serialize(configMap));
        }
        public void showFAQSectionWithMeta(string sectionPublishId, Dictionary<string, object> configMap)
        {
            hsShowFAQSectionWithMeta(sectionPublishId, Json.Serialize(configMap));
        }

        public void showSingleFAQ(string questionPublishId)
        {
            hsShowSingleFAQ(questionPublishId);
        }
        public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap)
        {
            hsShowSingleFAQWithOptions(questionPublishId, Json.Serialize(configMap));
        }
        public void showSingleFAQWithMeta(string questionPublishId, Dictionary<string, object> configMap)
        {
            hsShowSingleFAQWithMeta(questionPublishId, Json.Serialize(configMap));
        }

        public void updateMetaData(Dictionary<string, object> metaData)
        {
            hsSetMetaData(Json.Serialize(metaData));
        }

        public void handlePushNotification(string issueId)
        {
            Dictionary<string, object> pushNotificationData = new Dictionary<string, object>();
            pushNotificationData.Add("issue_id", issueId);
            handlePushNotification(pushNotificationData);
        }

        public void handleLocalNotification(string issueId)
        {
            hsHandleLocalNotificationForIssue(issueId);
        }

        public void handlePushNotification(Dictionary<string, object> pushNotificationData)
        {
            hsHandleRemoteNotification(Json.Serialize(pushNotificationData));
        }

        public void pauseDisplayOfInAppNotification(bool pauseInApp)
        {
            hsPauseDisplayOfInAppNotification(pauseInApp);
        }

        public void showAlertToRateAppWithURL(string url)
        {
            hsShowAlertToRateAppWithURL(url);
        }

		public void setSDKLanguage(string locale)
		{
			hsSetLanguage(locale);
		}

        public void showDynamicForm(string title, Dictionary<string, object>[] flowsData)
        {
            hsShowDynamicForm(title, Json.Serialize(flowsData));
        }

        public bool isConversationActive()
        {
            return hsIsConversationActive();
        }

		public void checkIfConversationActive()
		{
			hsCheckIfConversationActive();
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

    public class HelpshiftiOSLog
    {

        [DllImport ("__Internal")]
        private static extern void hsLog(string log);

        private HelpshiftiOSLog()
        {
        }

        public static int v(String tag, String log)
        {
            hsLog("HelpshiftLog:Verbose::" + tag + "::" + log);
            return 0;
        }

        public static int d(String tag, String log)
        {
            hsLog("HelpshiftLog:Debug::" + tag + "::" + log);
            return 0;
        }

        public static int i(String tag, String log)
        {
            hsLog("HelpshiftLog:Info::" + tag + "::" + log);
            return 0;
        }

        public static int w(String tag, String log)
        {
            hsLog("HelpshiftLog:Warn::" + tag + "::" + log);
            return 0;
        }

        public static int e(String tag, String log)
        {
            hsLog("HelpshiftLog:Error::" + tag + "::" + log);
            return 0;
        }

    }

}
#endif
