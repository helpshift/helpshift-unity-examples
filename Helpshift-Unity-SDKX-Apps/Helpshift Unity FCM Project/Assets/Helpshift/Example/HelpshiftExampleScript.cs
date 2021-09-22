using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
#if UNITY_IOS || UNITY_ANDROID
using Helpshift;
#endif

namespace HelpshiftExample
{

    public class HelpshiftExampleScript : MonoBehaviour
    {

        private String HELPSHIFT_PREFIX = "Helpshift";

#if UNITY_IOS || UNITY_ANDROID

        private HelpshiftSdk _helpshiftX;

        void Awake()
        {
            _helpshiftX = HelpshiftSdk.GetInstance();
            string domainName = "gayatri.helpshift.com";
            string platformId;
#if UNITY_ANDROID
            platformId = "gayatri_platform_20181018063833353-6d863ba814cb367";
#elif UNITY_IOS
            platformId = "<your-app-ios-platform-id>";
#endif
            _helpshiftX.Install(platformId, domainName, GetInstallConfig());
            Debug.Log("Unity - Awake called");
            _helpshiftX.SetHelpshiftEventsListener(new HSEventsListener());
        }

        void Start() {
            InitializeFirebase();
        }        

        public void ShowConversation()
        {
#if UNITY_ANDROID || UNITY_IOS
            _helpshiftX.ShowConversation(GetConversationConfig());
            Debug.Log("Helpshift - ShowConversation called");
#endif
        }

        public void ShowFAQs()
        {
#if UNITY_ANDROID || UNITY_IOS
            _helpshiftX.ShowFAQs(GetConversationConfig());
            Debug.Log("Helpshift - ShowFAQs called");
#endif
        }

        public void ShowFAQSection()
        {
#if UNITY_ANDROID || UNITY_IOS
            GameObject inputFieldGo = GameObject.FindGameObjectWithTag("sectionid");
            InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
            try
            {
                Convert.ToInt16(inputFieldCo.text);
                _helpshiftX.ShowFAQSection(inputFieldCo.text, GetConversationConfig());
                Debug.Log("Helpshift - ShowFAQSection called");
            }
            catch (FormatException e)
            {
                Debug.Log("Input string is not a sequence of digits : " + e);
            }
#endif
        }

        public void ShowSingleFAQ()
        {
#if UNITY_ANDROID || UNITY_IOS
            GameObject inputFieldGo = GameObject.FindGameObjectWithTag("faqidd");
            InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
            try
            {
                Convert.ToInt16(inputFieldCo.text);
                _helpshiftX.ShowSingleFAQ(inputFieldCo.text, GetConversationConfig());
                Debug.Log("Helpshift - ShowSingleFAQ called");
            }
            catch (FormatException e)
            {
                Debug.Log("Input string is not a sequence of digits : " + e);
            }
#endif
        }


        public void RequestUnreadCount()
        {
#if UNITY_ANDROID || UNITY_IOS
            _helpshiftX.RequestUnreadMessageCount(true);
            Debug.Log("Helpshift - RequestUnreadCount called");
#endif
        }


        public void Login()
        {
#if UNITY_ANDROID || UNITY_IOS
            _helpshiftX.Login(GetUserDetails());
            Debug.Log("Helpshift - Login called");
#endif

        }

        public void Logout()
        {
#if UNITY_ANDROID || UNITY_IOS
            _helpshiftX.Logout();
            Debug.Log("Helpshift - Logout called");
#endif
        }

        public void CustomizedApi()
        {
#if UNITY_ANDROID || UNITY_IOS
            // Call any custom api you want from here.
            //_helpshiftX.ClearAnonymousUserOnLogin();
            //_helpshiftX.SetSDKLanguage("Fr");
            Debug.Log("Helpshift - CustomizedApi called");
#endif
        }

        private Dictionary<string, string> GetUserDetails()
        {
            Dictionary<string, string> userDetails = new Dictionary<string, string>
            {
                { "userId", "<your-user-id>" },
                { "userEmail", "<your-user-email>" },
                { "userName", "<your-username>" }
            };
            // uncomment and pass the valid token here to check the user authentication feature
            // userDetails.Add("userAuthToken", "<some_random_token_to_check>");
            return userDetails;
        }

        private Dictionary<string, object> GetConversationConfig()
        {
            Dictionary<string, object> config = new Dictionary<string, object>
            {
                { "tags", new String[] { "foo", "bar" } },
                { "customIssueFields", GetCifs() }
            };
            return config;
        }


        public static Dictionary<string, object> GetCifs()
        {
            Dictionary<string, string> joiningDate = new Dictionary<string, string>();
            joiningDate.Add("type", "dt");
            joiningDate.Add("value", "1505927361535");

            Dictionary<string, string> stockLevel = new Dictionary<string, string>();
            stockLevel.Add("type", "n");
            stockLevel.Add("value", "1505");

            Dictionary<string, string> employeeName = new Dictionary<string, string>();
            employeeName.Add("type", "sl");
            employeeName.Add("value", "Bugs helpshift");

            Dictionary<string, string> isPro = new Dictionary<string, string>();
            isPro.Add("type", "b");
            isPro.Add("value", "true");

            Dictionary<string, object> cifDictionary = new Dictionary<string, object>();
            cifDictionary.Add("joining_date", joiningDate);
            cifDictionary.Add("stock_level", stockLevel);
            cifDictionary.Add("employee_name", employeeName);
            cifDictionary.Add("is_pro", isPro);
            return cifDictionary;
        }

        private Dictionary<string, object> GetInstallConfig()
        {

            Dictionary<string, object> installDictionary = new Dictionary<string, object>();
            installDictionary.Add(HelpshiftSdk.ENABLE_LOGGING, true);
#if UNITY_ANDROID
            //installDictionary.Add(HelpshiftSdk.NOTIFICATION_SOUND_ID, R.raw.custom_notification);
            //installDictionary.Add(HelpshiftSdk.NOTIFICATION_ICON, R.drawable.hs__conversation_image);
            //installDictionary.Add(HelpshiftSdk.NOTIFICATION_CHANNEL_ID, SampleAppConstants.CHANNEL_ID);
            //installDictionary.Add(HelpshiftSdk.NOTIFICATION_LARGE_ICON, R.drawable.airplane);
#endif
            return installDictionary;
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

        _helpshiftX.RegisterPushToken (token.Token);
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
            _helpshiftX.HandlePushNotification (hsPushData);
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
        
#endif
    }
}