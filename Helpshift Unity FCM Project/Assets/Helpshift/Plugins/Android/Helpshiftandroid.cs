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
    public class HelpshiftAndroid : IWorkerMethodDispatcher, IDexLoaderListener
    {

        private AndroidJavaClass jc;
        private AndroidJavaObject currentActivity, application;
        private AndroidJavaObject hsHelpshiftClass;
        private AndroidJavaObject hsSupportClass;
        private AndroidJavaClass hsUnityAPIDelegate;
        private HelpshiftInternalLogger hsInternalLogger;

        void unityHSApiCall(string api, params object[] args)
        {
            addHSApiCallToQueue("unityHSApiCallWithArgs", api, args);
        }

        void hsApiCall(string api, params object[] args)
        {
            addHSApiCallToQueue("hsApiCallWithArgs", api, args);
        }

        void hsApiCall(string api)
        {
            addHSApiCallToQueue("hsApiCall", api, null);
        }

        void hsSupportApiCall(string api, params object[] args)
        {
            addHSApiCallToQueue("hsSupportApiCallWithArgs", api, args);
        }

        void hsSupportApiCall(string api)
        {
            addHSApiCallToQueue("hsSupportApiCall", api, null);
        }

        void addHSApiCallToQueue(String methodIdentifier, String api, object[] args)
        {
            HelpshiftWorker.getInstance().enqueueApiCall("support", methodIdentifier, api, args);
        }

        public HelpshiftAndroid()
        {
            this.jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            this.currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            this.application = currentActivity.Call<AndroidJavaObject>("getApplication");
            this.hsUnityAPIDelegate = new AndroidJavaClass("com.helpshift.supportCampaigns.UnityAPIDelegate");
            HelpshiftWorker.getInstance().registerClient("support", this);
            HelpshiftDexLoader.getInstance().loadDex(this, application);
            hsInternalLogger = HelpshiftInternalLogger.getInstance();
        }

        public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
        {
            if (methodIdentifier.Equals("hsApiCallWithArgs"))
            {
                hsHelpshiftClass.CallStatic(api, args);
            } else if (methodIdentifier.Equals("hsApiCall"))
            {
                hsHelpshiftClass.CallStatic(api);
            } else if (methodIdentifier.Equals("hsSupportApiCallWithArgs"))
            {
                hsSupportClass.CallStatic(api, args);
            } else if (methodIdentifier.Equals("hsSupportApiCall"))
            {
                hsSupportClass.CallStatic(api);
            } else if (methodIdentifier.Equals("unityHSApiCallWithArgs"))
            {
                hsUnityAPIDelegate.CallStatic(api, args);
            }
        }

        public void onDexLoaded()
        {
			hsHelpshiftClass = HelpshiftDexLoader.getInstance().getHSDexLoaderJavaClass().CallStatic<AndroidJavaObject>("getHelpshiftUnityAPIInstance");
            hsSupportClass = HelpshiftDexLoader.getInstance().getHSDexLoaderJavaClass().CallStatic<AndroidJavaObject>("getHelpshiftSupportInstance");
        }

        public void install(string apiKey, string domain, string appId, Dictionary<string, object> configMap)
        {
            configMap.Add("sdkType", "unity");
            configMap.Add("pluginVersion", HelpshiftConfig.pluginVersion);
            configMap.Add("runtimeVersion", Application.unityVersion);
            string jsonSerializedConfig = Json.Serialize(configMap);

            hsApiCall("install", new object[] {this.application, apiKey, domain, appId, jsonSerializedConfig});
				
            // Logger will work only after install call is queued.	
            hsInternalLogger.d("Install called  : ApiKey : " + apiKey + ", Domain :" + domain + ", AppId: " + appId
                + ", Config : " + jsonSerializedConfig);
        }

        public void install()
        {
            hsApiCall("install", new object[] {this.application});

            // Logger will work only after install call is queued.	
            hsInternalLogger.d("Install called without config");
        }

        public int getNotificationCount(Boolean isAsync)
        {
            // Wait for queue since we need synchronous call here.
            HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
            hsInternalLogger.d("Call getNotificationCount: isAsync : " + isAsync);
            return this.hsHelpshiftClass.CallStatic<int>("getNotificationCount", isAsync);
        }

		public void requestUnreadMessagesCount(Boolean isAsync)
		{
			hsInternalLogger.d("Call requestUnreadMessagesCount: isAsync : " + isAsync);
            hsApiCall("requestUnreadMessagesCount", isAsync);
        }

		[Obsolete]
        public void setNameAndEmail(string userName, string email)
        {
            hsApiCall("setNameAndEmail", new object[] {userName, email});
        }

		[Obsolete]
        public void setUserIdentifier(string identifier)
        {
            hsApiCall("setUserIdentifier", identifier);
        }

        public void registerDeviceToken(string deviceToken)
        {
            hsInternalLogger.d("Register device token :" + deviceToken);
            hsApiCall("registerDeviceToken", new object [] {this.currentActivity, deviceToken});
        }

        public void leaveBreadCrumb(string breadCrumb)
        {
            hsApiCall("leaveBreadCrumb", breadCrumb);
        }

        public void clearBreadCrumbs()
        {
            hsApiCall("clearBreadCrumbs");
        }

		[Obsolete("Use the login(HelpshiftUser user) api instead.")]
        public void login(string identifier, string userName, string email)
        {
            hsInternalLogger.d("Login called : " + userName);
            hsApiCall("login", new object[] {identifier, userName, email});
        }

		public void login(HelpshiftUser helpshiftUser)
		{
			hsInternalLogger.d("Login called : " + helpshiftUser.name);
			hsApiCall("loginHelpshiftUser", new object[] {jsonifyHelpshiftUser(helpshiftUser)});
		}

		public void clearAnonymousUser()
		{
			hsApiCall("clearAnonymousUser");
		}

        public void logout()
        {
            hsApiCall("logout");
        }

        public void showConversation(Dictionary<string, object> configMap)
        {
	    hsApiCall("showConversationUnity", new object [] {this.currentActivity, Json.Serialize(cleanConfig(configMap))});
        }

        public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap)
        {
            hsApiCall("showFAQSectionUnity", new object[]
            {
                this.currentActivity,
                sectionPublishId,
				Json.Serialize(cleanConfig(configMap))
            });
        }

        public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap)
        {
            hsApiCall("showSingleFAQUnity", new object[]
            {
                this.currentActivity,
                questionPublishId,
                Json.Serialize(cleanConfig(configMap))
            });
        }

        public void showFAQs(Dictionary<string, object> configMap)
        {
            hsApiCall("showFAQsUnity", new object [] { this.currentActivity, Json.Serialize(cleanConfig(configMap))});
        }

        public void showConversation()
        {
            hsApiCall("showConversationUnity", new object[] {this.currentActivity, null});
        }

        public void showFAQSection(string sectionPublishId)
        {
            hsApiCall("showFAQSectionUnity", new object[] {this.currentActivity, sectionPublishId, null});
        }

        public void showSingleFAQ(string questionPublishId)
        {
            hsApiCall("showSingleFAQUnity", new object[] {this.currentActivity, questionPublishId, null});
        }

        public void showFAQs()
        {
            hsApiCall("showFAQsUnity", new object[] {this.currentActivity, null});
        }

        public void showConversationWithMeta(Dictionary<string, object> configMap)
        {
            hsApiCall("showConversationWithMetaUnity", new object[]
            {
                this.currentActivity,
				Json.Serialize(cleanConfig(configMap))
            });
        }

        public void showFAQSectionWithMeta(string sectionPublishId, Dictionary<string, object> configMap)
        {
            hsApiCall("showFAQSectionWithMetaUnity", new object[]
            {
                this.currentActivity,
                sectionPublishId,
				Json.Serialize(cleanConfig(configMap))
            });
        }

        public void showSingleFAQWithMeta(string questionPublishId, Dictionary<string, object> configMap)
        {
            hsApiCall("showSingleFAQWithMetaUnity", new object[]
            {
                this.currentActivity,
                questionPublishId,
 			Json.Serialize(cleanConfig(configMap))
            });
        }

        public void showFAQsWithMeta(Dictionary<string, object> configMap)
        {
            hsApiCall("showFAQsWithMetaUnity", new object[]{this.currentActivity, Json.Serialize(cleanConfig(configMap))});
        }

        public void updateMetaData(Dictionary<string, object> metaData)
        {
            hsApiCall("setMetaData", Json.Serialize(metaData));
        }

		private Dictionary<string, object> cleanConfig(Dictionary<string, object> configMap)
		{
	    	if (configMap.ContainsKey("customIssueFields")) {
				configMap ["hs-custom-issue-field"] = configMap["customIssueFields"];
				configMap.Remove ("customIssueFields");
	    	}
			return configMap;
		}

        public void handlePushNotification(string issueId)
        {
            // Handle issueId via the new api for handling push using dictionary.
            Dictionary<string, object> pushNotificationData = new Dictionary<string, object>();
            pushNotificationData.Add("issue_id", issueId);

            hsInternalLogger.d("Handle push notification : issueId " + issueId);
            handlePushNotification(pushNotificationData);
        }

        public void handlePushNotification(Dictionary<string, object> pushNotificationData)
        {
            hsInternalLogger.d("Handle push notification : data :" + pushNotificationData.ToString());
            unityHSApiCall("handlePush", new object[] {this.currentActivity, Json.Serialize(pushNotificationData)});
        }

        public void showAlertToRateAppWithURL(string url)
        {
            hsApiCall("showAlertToRateApp", url);
        }

        public void registerDelegates()
        {
            hsInternalLogger.d("Registering delegates");
            hsApiCall("registerDelegates");
        }

        public void registerForPushWithGcmId(string gcmId)
        {
            hsInternalLogger.d("Registering for push notification : GCM ID : " + gcmId);
            hsApiCall("registerGcmKey", new object[] {gcmId, this.currentActivity});
        }

        public void setSDKLanguage(string locale)
        {
            hsApiCall("setSDKLanguage", new object[] {locale});
        }

        public void showDynamicForm(string title, Dictionary<string, object>[] flows)
        {
            hsApiCall("showDynamicFormFromDataJson", new object[]
            {
                this.currentActivity,
				title,
                Json.Serialize(flows)
            });
        }

        public bool isConversationActive()
        {
            // Wait for queue since we need synchronous call here.
            HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
            return this.hsSupportClass.CallStatic<bool>("isConversationActive");
        }

		public void checkIfConversationActive()
		{
			this.hsHelpshiftClass.CallStatic("checkIfConversationActive");
		}

        public void onApplicationQuit()
        {
            hsInternalLogger.d("onApplicationQuit");
            HelpshiftWorker.getInstance().onApplicationQuit();
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

    public class HelpshiftAndroidLog : IDexLoaderListener, IWorkerMethodDispatcher
    {
        private static AndroidJavaObject logger = null;
        private static HelpshiftAndroidLog helpshiftAndroidLog = new HelpshiftAndroidLog();

        private HelpshiftAndroidLog()
        {
        }

        public void resolveAndCallApi(string methodIdentifier, string api, object[] args)
        {

        }

        public void onDexLoaded()
        {
            HelpshiftAndroidLog.logger = HelpshiftDexLoader.getInstance().getHSDexLoaderJavaClass().CallStatic<AndroidJavaObject>("getHelpshiftLogInstance");
        }

        private static void initLogger()
        {
            if (HelpshiftAndroidLog.logger == null)
            {
                HelpshiftWorker.getInstance().registerClient("helpshiftandroidlog", helpshiftAndroidLog);
                HelpshiftDexLoader.getInstance().registerListener(helpshiftAndroidLog);
            }
        }

        public static int v(String tag, String log)
        {
            initLogger();
            HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
            return HelpshiftAndroidLog.logger.CallStatic<int>("v", new object[] {tag, log});
        }

        public static int d(String tag, String log)
        {
            initLogger();
            HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
            return HelpshiftAndroidLog.logger.CallStatic<int>("d", new object[] {tag, log});
        }

        public static int i(String tag, String log)
        {
            initLogger();
            HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
            return HelpshiftAndroidLog.logger.CallStatic<int>("i", new object[] {tag, log});
        }

        public static int w(String tag, String log)
        {
            initLogger();
            HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
            return HelpshiftAndroidLog.logger.CallStatic<int>("w", new object[] {tag, log});
        }

        public static int e(String tag, String log)
        {
            initLogger();
            HelpshiftWorker.getInstance().synchronousWaitForApiCallQueue();
            return HelpshiftAndroidLog.logger.CallStatic<int>("e", new object[] {tag, log});
        }
    }
}
#endif
