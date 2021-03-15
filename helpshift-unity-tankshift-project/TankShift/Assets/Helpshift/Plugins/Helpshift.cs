/*
 * Copyright 2015, Helpshift, Inc.
 * All rights reserved
 */
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Helpshift
{
    /// <summary>
    /// The main class which exposes the Helpshift Sdk API for Unity scripts
    /// </summary>
    public class HelpshiftSdk
    {
        /// <summary>
        /// Various enums which are used in the Helpshift APIs
        /// </summary>

        // Response when user closes review alert
        public const String HS_RATE_ALERT_CLOSE = "HS_RATE_ALERT_CLOSE";
        // Response when user goes to conversation screen from review alert
        public const String HS_RATE_ALERT_FEEDBACK = "HS_RATE_ALERT_FEEDBACK";
        // Response when user goes to rate the app
        public const String HS_RATE_ALERT_SUCCESS = "HS_RATE_ALERT_SUCCESS";
        // Response when Helpshift is unable to show the review alert
        public const String HS_RATE_ALERT_FAIL = "HS_RATE_ALERT_FAIL";

        // Dictionary key to be used to supply tags with the meta-data
        public const String HSTAGSKEY = "hs-tags";
        // Dictionary key to be used to attach custom meta-data with config dictionaries
        public const String HSCUSTOMMETADATAKEY = "hs-custom-metadata";

        public const String UNITY_GAME_OBJECT = "unityGameObject";

        public const String ENABLE_IN_APP_NOTIFICATION = "enableInAppNotification";

        public const String ENABLE_DEFAULT_FALLBACK_LANGUAGE = "enableDefaultFallbackLanguage";

        public const String ENABLE_LOGGING = "enableLogging";

        public const String ENABLE_INBOX_POLLING = "enableInboxPolling";

        public const String ENABLE_AUTOMATIC_THEME_SWITCHING = "enableAutomaticThemeSwitching";

        public const String DISABLE_ENTRY_EXIT_ANIMATIONS = "disableEntryExitAnimations";

        public const String DISABLE_ERROR_REPORTING = "disableErrorReporting";

#if UNITY_IOS
        /// Dictionary key to be used to add custom issue fields with config dictionaries.
        public const String HSCUSTOMISSUEFIELDKEY = "customIssueFields";
#elif UNITY_ANDROID
		/// Dictionary key to be used to add custom issue fields with config dictionaries.
		public const String HSCUSTOMISSUEFIELDKEY = "hs-custom-issue-field";
#endif

        // Dictionary key to be used with withTagsMatching
        public const String HSTAGSMATCHINGKEY = "withTagsMatching";

        // Option value for enableContactUs which always shows the ContactUs button
        public const String CONTACT_US_ALWAYS = "always";
        // Option value for enableContactUs which never shows the ContactUs button
        public const String CONTACT_US_NEVER = "never";
        // Option value for enableContactUs which shows the ContactUs button only after searching through FAQs
        public const String CONTACT_US_AFTER_VIEWING_FAQS = "after_viewing_faqs";
        // Option value for enableContactUs which shows the ContactUs button only after marking an answer unhelpful
        public const String CONTACT_US_AFTER_MARKING_ANSWER_UNHELPFUL = "after_marking_answer_unhelpful";

        // Constants which can help detect special message types received in the
        // userRepliedToConversation message handler
        public const String HSUserAcceptedTheSolution = "User accepted the solution";
        public const String HSUserRejectedTheSolution = "User rejected the solution";
        public const String HSUserSentScreenShot = "User sent a screenshot";
        public const String HSUserReviewedTheApp = "User reviewed the app";

        public const String HsFlowTypeDefault = "defaultFlow";
        public const String HsFlowTypeConversation = "conversationFlow";
        public const String HsFlowTypeFaqs = "faqsFlow";
        public const String HsFlowTypeFaqSection = "faqSectionFlow";
        public const String HsFlowTypeSingleFaq = "singleFaqFlow";
        public const String HsFlowTypeNested = "dynamicFormFlow";

        public const String HsCustomContactUsFlows = "customContactUsFlows";

        public const String HsFlowType = "type";
        public const String HsFlowConfig = "config";
        public const String HsFlowData = "data";
        public const String HsFlowTitle = "title";

#if UNITY_IOS || UNITY_ANDROID
        private static HelpshiftSdk instance = null;
#endif

#if UNITY_IOS
        private static HelpshiftiOS nativeSdk = null;
#elif UNITY_ANDROID
        private static HelpshiftAndroid nativeSdk = null;
#endif
        private HelpshiftSdk()
        {
        }


        /// <summary>
        /// Main function which should be used to get the HelpshiftSdk instance.
        /// </summary>
        /// <returns>Singleton HelpshiftSdk instance</returns>
        public static HelpshiftSdk getInstance()
        {
#if UNITY_IOS || UNITY_ANDROID
            if (instance == null)
            {
                instance = new HelpshiftSdk();
#if UNITY_IOS
                nativeSdk = new HelpshiftiOS();
#elif UNITY_ANDROID
                nativeSdk = new HelpshiftAndroid();
#endif
            }
            return instance;
#else
            return null;
#endif
        }

        /// <summary>
        /// Install the HelpshiftSdk with specified apiKey, domainName, appId and config.
        /// When initializing Helpshift you must pass these three tokens. You initialize Helpshift by adding the API call
        /// ideally in the Start method of your game script.
        /// </summary>
        /// <param name="apiKey">This is your developer API Key</param>
        /// <param name="domainName">This is your domain name without any http:// or forward slashes</param>
        /// <param name="appId">This is the unique ID assigned to your app</param>
        /// <param name="config">This is the optional dictionary which contains additional configuration options for the HelpshiftSDK</param>
        public void install(string apiKey, string domainName, string appId, Dictionary<string, object> config=null)
        {
            if(config == null) {
                config = new Dictionary<string, object>();
            }
#if UNITY_IOS || UNITY_ANDROID
            config.Add("sdkType", "unity");
            config.Add("pluginVersion", HelpshiftConfig.pluginVersion);
            config.Add("runtimeVersion", Application.unityVersion);

            nativeSdk.install(apiKey, domainName, appId, config);
#endif
        }

        /// <summary>
        /// Gets the notification count of unread messages for the current conversation.
        /// </summary>
        /// <param name="isAsync">If set to <c>true</c> return value will be fetched from the server
        /// message handler. If set to <c>false</c> return value will have the local unread count.</param>
        /// Result will be returned in the didReceiveUnreadMessagesCount delegate
        public void requestUnreadMessagesCount(Boolean isAsync)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.requestUnreadMessagesCount(isAsync);
#endif
        }

        /// <summary>
        /// Sets the name and email for the current user.
		///
		/// NOTE: This API will not update the name & email provided from login API.
		/// Data from this API will be used to pre-fill name and email fields in the conversation form.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="email">Email.</param>
		[Obsolete]
        public void setNameAndEmail(string userName, string email)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.setNameAndEmail(userName, email);
#endif
        }

        /// <summary>
        /// Sets the user identifier for the current user
        /// </summary>
        /// <param name="identifier">Identifier.</param>
		[Obsolete]
        public void setUserIdentifier(string identifier)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.setUserIdentifier(identifier);
#endif
        }

        /// <summary>
        /// Login the user with specified identifier, name and email.
        /// </summary>
        /// <param name="identifier">Unique user identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="email">Email.</param>
		[Obsolete("Use the login(HelpshiftUser user) api instead.")]
        public void login(string identifier, string name, string email)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.login(identifier, name, email);
#endif
        }

        /// <summary>
        /// Login a user with a given identifier or email in HelpshiftUser
        /// This API introduces support for multiple login in Helpshift SDK. The user is uniquely identified via identifier and email combination.
        /// If any Support session is active, then any login attempt is ignored.
        /// </summary>
        /// <param helpshiftUser="user">HelpshiftUser model for the user to be logged in</param>
        public void login(HelpshiftUser helpshiftUser)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.login(helpshiftUser);
#endif
        }

        /// <summary>
        /// Clears the anonymous user.
        /// </summary>
        public void clearAnonymousUser()
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.clearAnonymousUser();
#endif
        }

        /// <summary>
        /// Logout the current user
        /// </summary>
        public void logout()
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.logout();
#endif
        }

        /// <summary>
        /// Registers the device token with Helpshift.
        /// </summary>
        /// <param name="deviceToken">Device token.</param>
        public void registerDeviceToken(string deviceToken)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.registerDeviceToken(deviceToken);
#endif
        }

        /// <summary>
        /// Add bread crumb.
        /// </summary>
        /// <param name="breadCrumb">Bread crumb.</param>
        public void leaveBreadCrumb(string breadCrumb)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.leaveBreadCrumb(breadCrumb);
#endif
        }

        /// <summary>
        /// Clears the bread crumbs.
        /// </summary>
        public void clearBreadCrumbs()
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.clearBreadCrumbs();
#endif
        }

        /// <summary>
        /// Shows the helpshift conversation screen.
        /// </summary>
        /// <param name="configMap">the optional dictionary which will contain the arguments passed to the
        /// Helpshift session (that will start with this method call).
        /// Supported options are listed here : https://developers.helpshift.com/unity/sdk-configuration-ios/
        /// and https://developers.helpshift.com/unity/sdk-configuration-android/
        public void showConversation(Dictionary<string, object> configMap=null)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.showConversation(configMap);
#endif
        }

        /// <summary>
        /// Shows the helpshift screen with FAQs from a particular section
        /// </summary>
        /// <param name="sectionPublishId">Section publish identifier</param>
        /// <param name="configMap">the dictionary which will contain the arguments passed to the
        /// Helpshift session (that will start with this method call).
        /// Supported options are listed here : https://developers.helpshift.com/unity/sdk-configuration-ios/
        /// and https://developers.helpshift.com/unity/sdk-configuration-android/
        public void showFAQSection(string sectionPublishId, Dictionary<string, object> configMap=null)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.showFAQSection(sectionPublishId, configMap);
#endif
        }

        /// <summary>
        /// Shows the helpshift screen with an FAQ with specified identifier
        /// </summary>
        /// <param name="questionPublishId">Question publish identifier.</param>
        /// <param name="configMap">the dictionary which will contain the arguments passed to the
        /// Helpshift session (that will start with this method call).
        /// Supported options are listed here : https://developers.helpshift.com/unity/sdk-configuration-ios/
        /// and https://developers.helpshift.com/unity/sdk-configuration-android/
        public void showSingleFAQ(string questionPublishId, Dictionary<string, object> configMap=null)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.showSingleFAQ(questionPublishId, configMap);
#endif
        }

        /// <summary>
        /// Shows the helpshift screen with all the FAQ sections.
        /// </summary>
        /// <param name="configMap">the dictionary which will contain the arguments passed to the
        /// Helpshift session (that will start with this method call).
        /// Supported options are listed here : https://developers.helpshift.com/unity/sdk-configuration-ios/
        /// and https://developers.helpshift.com/unity/sdk-configuration-android/
        public void showFAQs(Dictionary<string, object> configMap=null)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.showFAQs(configMap);
#endif
        }

        /// <summary>
        /// Updates the meta data which will be passed on with any conversation started after this call
        /// </summary>
        /// <param name="metaData">Meta data dictionary.</param>
        public void updateMetaData(Dictionary<string, object> metaData)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.updateMetaData(metaData);
#endif
        }

        /// <summary>
        /// Handles the push notification data that is received in the notification packet
        /// </summary>
        /// <param name="pushNotificationData">Push notification data. For iOS this should be the dictionary which is received from APNS.
        /// The structure of the dictionary should be : {"origin" : "helpshift", "aps" : {<notification data>}}
        /// For Android pass in the Dictionary which is received from GCM as is.</param>
        public void handlePushNotification(Dictionary<string, object> pushNotificationData)
        {
#if UNITY_IOS || UNITY_ANDROID
            // Remove null values from dictonary as null is converted to "null" string in json.
            List<string> keysToRemove = new List<String>();
            foreach (KeyValuePair<string, object> keyValuePair in pushNotificationData)
            {
                if (keyValuePair.Value == null)
                {
                    keysToRemove.Add(keyValuePair.Key);
                }
            }

            foreach (string key in keysToRemove)
            {
                pushNotificationData.Remove(key);
            }

            nativeSdk.handlePushNotification(pushNotificationData);
#endif
        }

        /// <summary>
        /// Shows the alert to rate app with URL
        /// </summary>
        /// <param name="url">URL.</param>
#if UNITY_IOS
        [Obsolete("showAlertToRateAppWithURL(string url) API is non operational and deprecated.", false)]
#endif
        public void showAlertToRateAppWithURL(string url)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.showAlertToRateAppWithURL(url);
#endif
        }


        /// <summary>
        /// Sets the SDK language to the given locale
        /// </summary>
        /// <param name="locale">Language locale.</param>
        public void setSDKLanguage(string locale)
        {
#if UNITY_ANDROID || UNITY_IOS
            nativeSdk.setSDKLanguage(locale);
#endif
        }

        /// <summary>
        /// Sets the SDK theme to the given style theme resource name
        /// </summary>
        /// <param name="themeName">theme resource name on Android, theme file name on iOS.</param>
        public void setTheme(string themeName)
        {
#if UNITY_ANDROID || UNITY_IOS
            nativeSdk.setTheme(themeName);
#endif
        }

#if UNITY_IOS

        /// <summary>
        /// Sets the SDK themes for light and dark modes on iOS 13 and above
        /// </summary>
        /// <param name="lightThemeFileName">light theme file name.</param>
        /// <param name="darkThemeFileName">dark theme file name.</param>
        public void setThemes(string lightThemeFileName, string darkThemeFileName)
        {
            nativeSdk.setThemes(lightThemeFileName, darkThemeFileName);
        }

        public void handleLocalNotification(string issueId)
        {
            nativeSdk.handleLocalNotification(issueId);
        }

        public void pauseDisplayOfInAppNotification(bool pauseInApp)
        {
            nativeSdk.pauseDisplayOfInAppNotification(pauseInApp);
        }
#endif
        public void registerDelegates()
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.registerDelegates();
#endif
        }
        public void showDynamicForm(string title, Dictionary<string, object>[] flows)
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.showDynamicForm(title, flows);
#endif
        }

        public void showDynamicForm(string title, Dictionary<string, object>[] flows, Dictionary<string, object> configMap=null)
        {
#if UNITY_IOS
            nativeSdk.showDynamicForm(title, flows, configMap);
#elif UNITY_ANDROID
            nativeSdk.showDynamicForm(title, flows);
#endif
        }

#if UNITY_ANDROID
        // Call on Application.Quit() to clean up required resources in Helpshift SDK.
        public void onApplicationQuit()
        {
            nativeSdk.onApplicationQuit();
        }
#endif

        /// <summary>
        /// Check if there is an  ongoing conversation.
        /// Results are returned in the didCheckIfConversationActive delegate
        /// </summary>
        public void checkIfConversationActive()
        {
#if UNITY_IOS || UNITY_ANDROID
            nativeSdk.checkIfConversationActive();
#endif
        }

#if UNITY_IOS
        /// <summary>
        /// Close the current Helpshift session
        /// If currently any Helpshift session is active, this API will close that session.
        /// Once the Helpshift session is closed, the helpshiftSupportSessionClosed delegate will be called.
        /// Otherwise if any Helpshift session is not active, helpshiftSupportSessionClosed delegate
        /// will be called immediately.
        /// </summary>

        public void closeHelpshiftSupportSession()
        {
            nativeSdk.closeHelpshiftSupportSession();
        }

        /// <summary>
        /// Enable the testing mode for the SDK. This will give additional debug information to help you with SDK integration.
        /// DO NOT enable this in production build of your application.
        /// </summary>

        public void enableTestingMode()
        {
            nativeSdk.enableTestingMode();
        }
#endif
    }

    /// <summary>
    /// Helpshift logger class.
    /// Logs printed with this API will be passed along with any conversation started
    /// here after.
    /// </summary>
    public class HelpshiftLog
    {
        public static int v(String tag, String log)
        {
#if UNITY_IOS
            return HelpshiftiOSLog.v(tag, log);
#elif UNITY_ANDROID
            return HelpshiftAndroidLog.v(tag, log);
#else
            return 0;
#endif
        }

        public static int d(String tag, String log)
        {
#if UNITY_IOS
            return HelpshiftiOSLog.d(tag, log);
#elif UNITY_ANDROID
            return HelpshiftAndroidLog.d(tag, log);
#else
            return 0;
#endif
        }

        public static int i(String tag, String log)
        {
#if UNITY_IOS
            return HelpshiftiOSLog.i(tag, log);
#elif UNITY_ANDROID
            return HelpshiftAndroidLog.i(tag, log);
#else
            return 0;
#endif
        }

        public static int w(String tag, String log)
        {
#if UNITY_IOS
            return HelpshiftiOSLog.w(tag, log);
#elif UNITY_ANDROID
            return HelpshiftAndroidLog.w(tag, log);
#else
            return 0;
#endif
        }

        public static int e(String tag, String log)
        {
#if UNITY_IOS
            return HelpshiftiOSLog.e(tag, log);
#elif UNITY_ANDROID
            return HelpshiftAndroidLog.e(tag, log);
#else
            return 0;
#endif
        }

    }
}
