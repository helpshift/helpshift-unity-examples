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
        [DllImport("__Internal")]
        private static extern void hsInstallForApiKeyWithOptions(string apiKey, string domainName, string appId, string jsonOptionsDict);

        [DllImport("__Internal")]
        private static extern void hsShowFAQsWithOptions(string jsonOptionsDict);

        [DllImport("__Internal")]
        private static extern void hsShowFAQSectionWithOptions(string id, string jsonOptionsDict);

        [DllImport("__Internal")]
        private static extern void hsShowConversationWithOptions(string jsonOptionsDict);

        [DllImport("__Internal")]
        private static extern void hsShowSingleFAQWithOptions(string id, string jsonOptionsDict);

        [DllImport("__Internal")]
        private static extern void hsSetUserIdentifier(string userIdentifier);
        [DllImport("__Internal")]
        private static extern void hsSetNameAndEmail(string userName, string email);
        [DllImport("__Internal")]
        private static extern void hsRegisterDeviceToken(string deviceToken);

        [DllImport("__Internal")]
        private static extern void hsLeaveBreadCrumb(string breadCrumb);
        [DllImport("__Internal")]
        private static extern void hsClearBreadCrumbs();
        [DllImport("__Internal")]
        private static extern void hsHandleLocalNotificationForIssue(string issueID);
        [DllImport("__Internal")]
        private static extern void hsHandleRemoteNotification(string issueID);
        [DllImport("__Internal")]
        private static extern void hsSetMetaData(string meta);
        [DllImport("__Internal")]
        private static extern void hsPauseDisplayOfInAppNotification(bool pauseInApp);
        [DllImport("__Internal")]
        private static extern void hsShowAlertToRateAppWithURL(string url);
        [DllImport("__Internal")]
        private static extern void hsLogin(string identifier, string name, string email);
        [DllImport("__Internal")]
        private static extern void hsLoginWithUser(string helpshiftUser);
        [DllImport("__Internal")]
        private static extern void hsLogout();
        [DllImport("__Internal")]
        private static extern void hsClearAnonymousUser();
        [DllImport("__Internal")]
        private static extern void hsShowDynamicForm(string title, string flowsJson);
        [DllImport("__Internal")]
        private static extern void hsShowDynamicFormWithConfig(string title, string flowsJson, string configMap);

        [DllImport("__Internal")]
        private static extern void hsSetLanguage(string language);
        [DllImport("__Internal")]
        private static extern void hsRequestUnreadMessagesCount(bool isRemote);
        [DllImport("__Internal")]
        private static extern void hsCheckIfConversationActive();

        [DllImport("__Internal")]
        private static extern void hsCloseHelpshiftSupportSession();

        [DllImport("__Internal")]
        private static extern void hsEnableTestingMode();

        [DllImport("__Internal")]
        private static extern void hsSetTheme(string themeFileName);

        [DllImport("__Internal")]
        private static extern void hsSetThemes(string lightThemeFileName, string darkThemeFileName);

        public HelpshiftiOS()
        {
        }

        private string serializeDictionary(Dictionary<string, object> configMap) {
            if(configMap == null)
            {
                configMap = new Dictionary<string, object>();
            }
            return Json.Serialize(configMap);
        }

        public void install(string apiKey, string domain, string appId, Dictionary<string, object> configMap)
        {
            hsInstallForApiKeyWithOptions(apiKey, domain, appId, serializeDictionary(configMap));
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

        public void login(HelpshiftUser helpshiftUser)
        {
            hsLoginWithUser(jsonifyHelpshiftUser(helpshiftUser));
        }
        public void logout()
        {
            hsLogout();
        }

        public void clearAnonymousUser()
        {
            hsClearAnonymousUser();
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
            hsShowConversationWithOptions(serializeDictionary(configMap));
        }

        public void showFAQs(Dictionary<string, object> configMap)
        {
            hsShowFAQsWithOptions(serializeDictionary(configMap));
        }

        public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap)
        {
            hsShowFAQSectionWithOptions(sectionPublishId, serializeDictionary(configMap));
        }

        public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap)
        {
            hsShowSingleFAQWithOptions(questionPublishId, serializeDictionary(configMap));
        }

        public void updateMetaData(Dictionary<string, object> metaData)
        {
            hsSetMetaData(serializeDictionary(metaData));
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

        public void registerDelegates()
        {
            HelpshiftiOSDelegate.RegisterUnitySupportMessageCallback();
        }

        public void setSDKLanguage(string locale)
        {
            hsSetLanguage(locale);
        }

        public void setTheme(string themeFileName)
        {
            hsSetTheme(themeFileName);
        }

        public void setThemes(string lightThemeFileName, string darkThemeFileName)
        {
            hsSetThemes(lightThemeFileName, darkThemeFileName);
        }

        public void showDynamicForm(string title, Dictionary<string, object>[] flowsData)
        {
            hsShowDynamicForm(title, Json.Serialize(flowsData));
        }

        public void showDynamicForm(string title, Dictionary<string, object>[] flows, Dictionary<string, object> configMap)
        {
            hsShowDynamicFormWithConfig(title, Json.Serialize(flows), serializeDictionary(configMap));
        }

        public void checkIfConversationActive()
        {
            hsCheckIfConversationActive();
        }

        public void closeHelpshiftSupportSession()
        {
            hsCloseHelpshiftSupportSession();
        }

        public void enableTestingMode()
        {
            hsEnableTestingMode();
        }

        private string jsonifyHelpshiftUser(HelpshiftUser helpshiftUser)
        {
            Dictionary<string, string> helpshiftUserDataMap = new Dictionary<string, string>();
            helpshiftUserDataMap.Add("identifier", helpshiftUser.identifier);
            helpshiftUserDataMap.Add("email", helpshiftUser.email);
            helpshiftUserDataMap.Add("name", helpshiftUser.name);
            helpshiftUserDataMap.Add("authToken", helpshiftUser.authToken);
            return Json.Serialize(helpshiftUserDataMap);
        }

    }

    public class HelpshiftiOSLog
    {

        [DllImport("__Internal")]
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
