using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
#if UNITY_IOS || UNITY_ANDROID
using Helpshift;
using HSMiniJSON;
#endif

public class HelpshiftExampleScript : MonoBehaviour
{

#if UNITY_IOS || UNITY_ANDROID
    private HelpshiftSdk _support;
    public void updateMetaData(string nothing)
    {
        Debug.Log("Update metadata ************************************************************");
        Dictionary<string, object> configMap = new Dictionary<string, object>();
        configMap.Add("user-level", "21");
        configMap.Add("hs-tags", new string[] { "Tag-1" });
        _support.updateMetaData(configMap);
    }

    public void helpshiftSessionBegan(string message)
    {
        Debug.Log("Helpshift Support Session Began ************************************************************");
    }

    public void helpshiftSessionEnded(string message)
    {
        Debug.Log("Helpshift Support Session ended ************************************************************");
    }

    public void alertToRateAppAction(string result)
    {
        Debug.Log("User action on alert :" + result);
    }

    public void didReceiveNotificationCount(string count)
    {
        Debug.Log("Notification async count : " + count);
    }

    public void didReceiveInAppNotificationCount(string count)
    {
        Debug.Log("In-app Notification count : " + count);
    }

    public void conversationEnded()
    {
        Debug.Log("Helpshift conversation ended.");
    }

    public void didReceiveUnreadMessagesCount(string count)
    {
        Debug.Log("Helpshift Unread message count : " + count);
    }

    public void didCheckIfConversationActive(string active)
    {
        Debug.Log("Helpshift conversation active status : " + active);
    }

    public void displayAttachmentFile(string path)
    {
        if (path.StartsWith("content://", StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Helpshift downloaded attachment with Uri : " + path);
        } else
        {
            Debug.Log("Helpshift downloaded attachment with filePath : " + path);
        }
    }

    /// <summary>
    /// Conversation delegates
    /// </summary>

    public void newConversationStarted(string message)
    {
        Debug.Log("Helpshift conversation started.");
    }

    public void userRepliedToConversation(string newMessage)
    {
        Debug.Log("Helpshift user replied to conversation.");
    }

    public void userCompletedCustomerSatisfactionSurvey(string json)
    {
        Dictionary<string, object> csatInfo = (Dictionary<string, object>)Json.Deserialize(json);
        Debug.Log("Customer satisfaction information : " + csatInfo);
    }

    public void authenticationFailed(string serializedJSONUserData)
    {
        HelpshiftUser user = HelpshiftJSONUtility.getHelpshiftUser(serializedJSONUserData);
        HelpshiftAuthFailureReason reason = HelpshiftJSONUtility.getAuthFailureReason(serializedJSONUserData);
        Debug.Log("Authentication failed : " + user.identifier + " " + user.authToken + " , Reason : " + reason.ToString());
    }

    public void userClickOnAction(string serializedJSONUserActionData)
    {
        HelpshiftUserAction userAction = HelpshiftJSONUtility.getUserActionData(serializedJSONUserActionData);
        Debug.Log("User clicked on action: Type: " + userAction.actionType + ", Data: " + userAction.actionData);
    }

    void Awake()
    {
        _support = HelpshiftSdk.getInstance();
        string apiKey = "<your_api_key>";
        string domainName = "<your_domain_name>";
        string appId;
#if UNITY_ANDROID
		appId = "your_android_app_id";
#elif UNITY_IOS
        appId = "your_ios_app_id";
#endif
        _support.install(apiKey, domainName, appId, getInstallConfig());
    }

    // Use this for initialization
    void Start()
    {
        _support.registerDelegates();
        _support.requestUnreadMessagesCount(true);
        /*HelpshiftUser user = new HelpshiftUser.Builder (<user-identifier>, <user-email>)
			.setAuthToken (<user-auth-token>)
			.setName (<user-name>)
			.build ();

        _support.login (user);*/
        /*
         * Uncomment the below line to update helpshift theme at runtime
         */
        // _support.setTheme("Helpshift.Theme.DayNight.Light");
    /*
    * showAlertToRateAppWithURL API is non operational and deprecated for iOS
    * with Helpshift Unity Plugin v2.9.0 to know more https://developers.helpshift.com/unity/reviews-and-feedback-ios/#review-reminder-changes
    */
#if UNITY_IOS
    	GameObject appReviewButton;
    	appReviewButton = GameObject.FindGameObjectWithTag("app_review_button");
    	appReviewButton.SetActive(false);
#endif
    }

    public void onShowFAQsClick()
    {
        Debug.Log("Show FAQs clicked !!");
        _support.showFAQs();
    }
    public void onCustomContactUsClick()
    {
        Dictionary<string, object>[] flows = getDynamicFlows();
        Dictionary<string, object> faqConfig = new Dictionary<string, object>();
        faqConfig.Add(HelpshiftSdk.HsCustomContactUsFlows, flows);
        _support.showFAQs(faqConfig);
    }

    protected Dictionary<string, object>[] getDynamicFlows()
    {
        Dictionary<string, object> conversationFlow = new Dictionary<string, object>();
        conversationFlow.Add(HelpshiftSdk.HsFlowType, HelpshiftSdk.HsFlowTypeConversation);
        conversationFlow.Add(HelpshiftSdk.HsFlowTitle, "Converse");
        Dictionary<string, object> conversationConfig = new Dictionary<string, object>();
        conversationConfig.Add("conversationPrefillText", "This is from dynamic");
        conversationConfig.Add("hideNameAndEmail", "yes");
        conversationConfig.Add("showSearchOnNewConversation", "yes");
        conversationFlow.Add(HelpshiftSdk.HsFlowConfig, conversationConfig);

        Dictionary<string, object> faqsFlow = new Dictionary<string, object>();
        faqsFlow.Add(HelpshiftSdk.HsFlowType, HelpshiftSdk.HsFlowTypeFaqs);
        faqsFlow.Add(HelpshiftSdk.HsFlowTitle, "FAQs");

        Dictionary<string, object> faqSectionFlow = new Dictionary<string, object>();
        faqSectionFlow.Add(HelpshiftSdk.HsFlowType, HelpshiftSdk.HsFlowTypeFaqSection);
        faqSectionFlow.Add(HelpshiftSdk.HsFlowTitle, "FAQ section");
        faqSectionFlow.Add(HelpshiftSdk.HsFlowData, "1509");

        Dictionary<string, object> faqFlow = new Dictionary<string, object>();
        faqFlow.Add(HelpshiftSdk.HsFlowType, HelpshiftSdk.HsFlowTypeSingleFaq);
        faqFlow.Add(HelpshiftSdk.HsFlowTitle, "FAQ");
        faqFlow.Add(HelpshiftSdk.HsFlowData, "2998");

        Dictionary<string, object> nestedFlow = new Dictionary<string, object>();
        nestedFlow.Add(HelpshiftSdk.HsFlowType, HelpshiftSdk.HsFlowTypeNested);
        nestedFlow.Add(HelpshiftSdk.HsFlowTitle, "Next form");
        nestedFlow.Add(HelpshiftSdk.HsFlowData, new Dictionary<string, object>[]
        {
            conversationFlow,
            faqsFlow,
            faqSectionFlow,
            faqFlow
        });

        Dictionary<string, object>[] flows = new Dictionary<string, object>[] {
            conversationFlow,
            faqsFlow,
            faqSectionFlow,
            faqFlow,
            nestedFlow
        };

        return flows;
    }

    public void onShowDynamicClick()
    {
        _support.showDynamicForm("This is a dynamic form", getDynamicFlows());
    }

    public void onShowConversationClick()
    {
        Debug.Log("Show Conversation clicked !!");
#if UNITY_ANDROID
        _support.showConversation(getApiConfig());
#elif UNITY_IOS
        _support.showConversation();
#endif
    }

    public void onShowFAQSectionClick()
    {
        GameObject inputFieldGo = GameObject.FindGameObjectWithTag("faq_section_id");
        InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
        try
        {
            Convert.ToInt16(inputFieldCo.text);
            _support.showFAQSection(inputFieldCo.text);
        }
        catch (FormatException e)
        {
            Debug.Log("Input string is not a sequence of digits : " + e);
        }
    }

    public void onShowFAQClick()
    {
        GameObject inputFieldGo = GameObject.FindGameObjectWithTag("faq_id");
        InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
        try
        {
            Convert.ToInt16(inputFieldCo.text);
            _support.showSingleFAQ(inputFieldCo.text);
        }
        catch (FormatException e)
        {
            Debug.Log("Input string is not a sequence of digits : " + e);
        }
    }

    public void onShowReviewReminderClick()
    {
#if UNITY_ANDROID
        _support.showAlertToRateAppWithURL("market://details?id=com.RunnerGames.game.YooNinja_Lite");
#endif
    }

    /**
        Helper method to build config dictionary for Helpshift Unity SDK FAQ/Conversation APIs
     */
    private Dictionary<string, object> getApiConfig()
    {
        Dictionary<string, object> configDictionary = new Dictionary<string, object>();
        /*
        Possible values:
        CONTACT_US_ALWAYS, CONTACT_US_NEVER, CONTACT_US_AFTER_VIEWING_FAQS, CONTACT_US_AFTER_MARKING_ANSWER_UNHELPFUL
         */
        configDictionary.Add("enableContactUs", HelpshiftSdk.CONTACT_US_AFTER_VIEWING_FAQS);
        configDictionary.Add("gotoConversationAfterContactUs", "no"); // Possible options:  "yes", "no"
        configDictionary.Add("requireEmail", "no"); // Possible options:  "yes", "no"
        configDictionary.Add("hideNameAndEmail", "no"); // Possible options:  "yes", "no"
        configDictionary.Add("enableFullPrivacy", "no"); // Possible options:  "yes", "no"
        configDictionary.Add("showSearchOnNewConversation", "no"); // Possible options:  "yes", "no"
        configDictionary.Add("showConversationResolutionQuestion", "yes"); // Possible options:  "yes", "no"
        configDictionary.Add("enableTypingIndicator", "yes"); // Possible options:  "yes", "no"
        configDictionary.Add("showConversationInfoScreen", "yes"); // Possible options:  "yes", "no"
        return configDictionary;
    }

    /**
        Helper method to build config dictionary for Helpshift Unity SDK Install API
     */
    private Dictionary<string, object> getInstallConfig()
    {
        Dictionary<string, object> installDictionary = new Dictionary<string, object>();
        installDictionary.Add(HelpshiftSdk.UNITY_GAME_OBJECT, "background_image");
        installDictionary.Add(HelpshiftSdk.ENABLE_IN_APP_NOTIFICATION, "yes"); // Possible options:  "yes", "no"
        installDictionary.Add(HelpshiftSdk.ENABLE_DEFAULT_FALLBACK_LANGUAGE, "yes"); // Possible options:  "yes", "no"
        installDictionary.Add(HelpshiftSdk.DISABLE_ENTRY_EXIT_ANIMATIONS, "no"); // Possible options:  "yes", "no"
        installDictionary.Add(HelpshiftSdk.ENABLE_INBOX_POLLING, "no"); // Possible options:  "yes", "no"
        installDictionary.Add(HelpshiftSdk.ENABLE_LOGGING, "yes"); // Possible options:  "yes", "no"
        installDictionary.Add(HelpshiftSdk.ENABLE_AUTOMATIC_THEME_SWITCHING, "yes"); // Possible options:  "yes", "no"
        installDictionary.Add(HelpshiftSdk.DISABLE_ERROR_REPORTING, "no"); // Possible options:  "yes", "no"
        installDictionary.Add("screenOrientation", -1); // Possible options:  SCREEN_ORIENTATION_LANDSCAPE=0, SCREEN_ORIENTATION_PORTRAIT=1, SCREEN_ORIENTATION_UNSPECIFIED = -1
        installDictionary.Add("supportedFileFormats", new string[] { "token" }); // If you want that all files with extension token should be handled by your app.
        return installDictionary;
    }
#endif
}
