using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
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
	private static string HELPSHIFT_PREFIX = "Helpshift Log : ";
	Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    public void updateMetaData(string nothing)
    {
        Debug.Log("Update metadata ************************************************************");
        Dictionary<string, object> configMap = new Dictionary<string, object>();
        configMap.Add("user-level", "21");
        configMap.Add("hs-tags", new string [] {"Tag-1"});
        _support.updateMetaData(configMap);
    }

    public void helpshiftSessionBegan(string message)
    {
        Debug.Log("Session Began ************************************************************");
    }

    public void helpshiftSessionEnded(string message)
    {
        Debug.Log("Session ended ************************************************************");
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

	public void authenticationFailed(string serializedJSONUserData) {
		HelpshiftUser user = HelpshiftJSONUtility.getHelpshiftUser (serializedJSONUserData);
		HelpshiftAuthFailureReason reason = HelpshiftJSONUtility.getAuthFailureReason (serializedJSONUserData);
		Debug.Log ("Authentication failed : " + user.identifier + " " + user.authToken +" , Reason : " + reason.ToString ());
	}

    // Use this for initialization
    void Start()
    {
		Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
			dependencyStatus = task.Result;
			if (dependencyStatus == Firebase.DependencyStatus.Available) {
				InitializeFirebase();
			} else {
				Debug.LogError(
					"Could not resolve all Firebase dependencies: " + dependencyStatus);
			}
		});


        _support = HelpshiftSdk.getInstance();
        _support.install();

#if UNITY_ANDROID
        _support.registerDelegates();
#endif
        _support.requestUnreadMessagesCount(true);
        /*HelpshiftUser user = new HelpshiftUser.Builder (<user-identifier>, <user-email>)
			.setAuthToken (<user-auth-token>)
			.setName (<user-name>)
			.build ();

        _support.login (user);*/
    }

	// Setup message event handlers.
	void InitializeFirebase() {
		// Prevent the app from requesting permission to show notifications
		// immediately upon being initialized. Since it the prompt is being
		// suppressed, we must manually display it with a call to
		// RequestPermission() elsewhere.
		Firebase.Messaging.FirebaseMessaging.TokenRegistrationOnInitEnabled = false;
		Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
		Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
		Debug.Log("Firebase Messaging Initialized");

		// This will display the prompt to request permission to receive
		// notifications if the prompt has not already been displayed before. (If
		// the user already responded to the prompt, thier decision is cached by
		// the OS and can be changed in the OS settings).
		Firebase.Messaging.FirebaseMessaging.RequestPermissionAsync().ContinueWith(task => {
			LogTaskCompletion(task, "RequestPermissionAsync");
		});
	}

	public virtual void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) {
		Debug.Log(HELPSHIFT_PREFIX + " Received Registration Token: " + token.Token);
		Debug.Log(HELPSHIFT_PREFIX + " Registering device token with Helpshift SDK");

		_support.registerDeviceToken (token.Token);
		Debug.Log(HELPSHIFT_PREFIX + " Helpshift device token registered");
	}


	public virtual void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) {
		Debug.Log(HELPSHIFT_PREFIX + " Received a new message");

		IDictionary<string, string> pushData = e.Message.Data;

		/// Check if the notification origin is Helpshift
		if (pushData.ContainsKey ("origin") && pushData ["origin"].Equals ("helpshift")) {
			Dictionary<string, object> hsPushData = new Dictionary<string, object> ();
			Debug.Log(HELPSHIFT_PREFIX + " Received a new message for Helpshift");
			foreach (string key in pushData.Keys) {
				hsPushData.Add (key, pushData [key]);
			}

			// Handle the notification with Helpshift SDK.
			_support.handlePushNotification (hsPushData);
			return;
		}

		///
		/// Handle notification from other sources
		/// 
	}

	// Log the result of the specified task, returning true if the task
	// completed successfully, false otherwise.
	protected bool LogTaskCompletion(Task task, string operation) {
		bool complete = false;
		if (task.IsCanceled) {
			Debug.Log(operation + " canceled.");
		} else if (task.IsFaulted) {
			Debug.Log(operation + " encounted an error.");
			foreach (Exception exception in task.Exception.Flatten().InnerExceptions) {
				string errorCode = "";
				Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
				if (firebaseEx != null) {
					errorCode = String.Format("Error.{0}: ",
						((Firebase.Messaging.Error)firebaseEx.ErrorCode).ToString());
				}
				Debug.Log(errorCode + exception.ToString());
			}
		} else if (task.IsCompleted) {
			Debug.Log(operation + " completed");
			complete = true;
		}
		return complete;
	}

	// End our messaging session when the program exits.
	public void OnDestroy() {
		Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
		Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
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
        _support.showConversation();
    }

    public void onShowFAQSectionClick()
    {
        GameObject inputFieldGo = GameObject.FindGameObjectWithTag("faq_section_id");
        InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
        try
        {
            Convert.ToInt16(inputFieldCo.text);
            _support.showFAQSection(inputFieldCo.text);
        } catch (FormatException e)
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
        } catch (FormatException e)
        {
            Debug.Log("Input string is not a sequence of digits : " + e);
        }
    }

    public void onShowReviewReminderClick()
    {
#if UNITY_IOS
		_support.showAlertToRateAppWithURL("");
#elif UNITY_ANDROID
        _support.showAlertToRateAppWithURL("");
#endif
    }

    public void onCampaignsTabClick()
    {
        Application.LoadLevel(1);
    }
	#endif
}
