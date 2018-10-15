using UnityEngine;
using System;
using System.IO;
#if UNITY_IOS || UNITY_ANDROID
using Helpshift;
#endif
using System.Collections.Generic;
using HSMiniJSON;

#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
#endif
[System.Serializable]
public class HelpshiftConfig : ScriptableObject
{
    private static HelpshiftConfig instance;
    private const string helpshiftConfigAssetName = "HelpshiftConfig";
    private const string helpshiftConfigPath = "Helpshift/Resources";
    public const String pluginVersion = "4.0.1";

    [SerializeField]
    private string
        apiKey;
    [SerializeField]
    private string
        domainName;
    [SerializeField]
    private string
        iosAppId;
    [SerializeField]
    private string
        androidAppId;
    [SerializeField]
    private int
        contactUsOption;
    [SerializeField]
    private bool
        gotoConversation;
    [SerializeField]
    private bool
        presentFullScreen;
    [SerializeField]
    private int
        enableInAppNotification = 2;
    [SerializeField]
    private bool
        requireEmail;
    [SerializeField]
    private bool
        hideNameAndEmail;
    [SerializeField]
    private bool
        enablePrivacy;
    [SerializeField]
    private bool
        showSearchOnNewConversation;
    [SerializeField]
    private int
        showConversationResolutionQuestion = 0;
    [SerializeField]
    private int
        enableDefaultFallbackLanguage = 2;
    [SerializeField]
    private bool
        disableEntryExitAnimations;
    [SerializeField]
    private string
        conversationPrefillText;
	    
    [SerializeField]
    private bool
        enableInboxPolling = true;

    [SerializeField]
    private bool
        enableLogging = false;

	[SerializeField]
	private bool
		enableTypingIndicator = false;

    [SerializeField]
    // Default to ActivityInfo.SCREEN_ORIENTATION_UNSPECIFIED
	    private int
        screenOrientation = -1;

	[SerializeField]
	private bool
		showConversationInfoScreen = false;

	[SerializeField]
	private string
		supportedFileFormats;

#if UNITY_IOS || UNITY_ANDROID
    private string[] contactUsOptions = {Helpshift.HelpshiftSdk.CONTACT_US_ALWAYS,
                Helpshift.HelpshiftSdk.CONTACT_US_NEVER,
                Helpshift.HelpshiftSdk.CONTACT_US_AFTER_VIEWING_FAQS,
                Helpshift.HelpshiftSdk.CONTACT_US_AFTER_MARKING_ANSWER_UNHELPFUL};
#else
        private string[] contactUsOptions = {"always", "never", "after_viewing_faqs", "after_marking_answer_unhelpful"};
#endif
    [SerializeField]
    private string
        unityGameObject;
    [SerializeField]
    private string
        notificationIcon;
    [SerializeField]
    private string
        largeNotificationIcon;
    [SerializeField]
    private string
        notificationSound;
    [SerializeField]
    private string
        customFont;

	[SerializeField]
	private string supportNotificationChannel;

	[SerializeField]
	private string campaignsNotificationChannel;

    public static HelpshiftConfig Instance
    {
        get
        {
            instance = Resources.Load(helpshiftConfigAssetName) as HelpshiftConfig;
            if (instance == null)
            {
                instance = CreateInstance<HelpshiftConfig>();
#if UNITY_EDITOR
                string properPath = Path.Combine(Application.dataPath, helpshiftConfigPath);
                if (!Directory.Exists(properPath))
                {
                    AssetDatabase.CreateFolder("Assets/Helpshift", "Resources");
                }

                string fullPath = Path.Combine(Path.Combine("Assets", helpshiftConfigPath),
                                                               helpshiftConfigAssetName + ".asset"
                );
                AssetDatabase.CreateAsset(instance, fullPath);
#endif
            }
            return instance;
        }
    }

        #if UNITY_EDITOR
    [MenuItem("Helpshift/Edit Config")]
    public static void Edit()
    {
        Selection.activeObject = Instance;
    }

    [MenuItem("Helpshift/Developers Page")]
    public static void OpenAppPage()
    {
        string url = "https://developers.helpshift.com/unity/";
        Application.OpenURL(url);
    }

    [MenuItem("Helpshift/SDK Twitter handler")]
    public static void OpenFacebookGroup()
    {
        string url = "https://twitter.com/helpshiftsdk";
        Application.OpenURL(url);
    }

    [MenuItem("Helpshift/Report an SDK Bug")]
    public static void ReportABug()
    {
        string url = "mailto:support@helpshift.com";
        Application.OpenURL(url);
    }
        #endif

    public bool GotoConversation
    {
        get { return gotoConversation; }
        set
        {
            if (gotoConversation != value)
            {
                gotoConversation = value;
            }
        }
    }

    public int ContactUs
    {
        get { return contactUsOption; }
        set
        {
            if (contactUsOption != value)
            {
                contactUsOption = value;
            }
        }
    }

    public bool PresentFullScreenOniPad
    {
        get { return presentFullScreen; }
        set
        {
            if (presentFullScreen != value)
            {
                presentFullScreen = value;
            }
        }
    }

    public bool EnableInAppNotification
    {
        get { return enableInAppNotification != 0; }
        set
        {
            if (enableInAppNotification != Convert.ToInt32(value))
            {
                enableInAppNotification = Convert.ToInt32(value);
            }
        }
    }

    public bool RequireEmail
    {
        get { return requireEmail; }
        set
        {
            if (requireEmail != value)
            {
                requireEmail = value;
            }
        }
    }

    public bool HideNameAndEmail
    {
        get { return hideNameAndEmail; }
        set
        {
            if (hideNameAndEmail != value)
            {
                hideNameAndEmail = value;
            }
        }
    }

    public bool EnablePrivacy
    {
        get { return enablePrivacy; }
        set
        {
            if (enablePrivacy != value)
            {
                enablePrivacy = value;
            }
        }
    }

    public bool ShowSearchOnNewConversation
    {
        get { return showSearchOnNewConversation; }
        set
        {
            if (showSearchOnNewConversation != value)
            {
                showSearchOnNewConversation = value;
            }
        }
    }

    public bool ShowConversationResolutionQuestion
    {
        get { return showConversationResolutionQuestion != 0; }
        set
        {
            if (showConversationResolutionQuestion != Convert.ToInt32(value))
            {
                showConversationResolutionQuestion = Convert.ToInt32(value);
            }
        }
    }

    public bool EnableDefaultFallbackLanguage
    {
        get { return enableDefaultFallbackLanguage != 0; }
        set
        {
            if (enableDefaultFallbackLanguage != Convert.ToInt32(value))
            {
                enableDefaultFallbackLanguage = Convert.ToInt32(value);
            }
        }
    }

    public bool DisableEntryExitAnimations
    {
        get { return disableEntryExitAnimations; }
        set
        {
            if (disableEntryExitAnimations != value)
            {
                disableEntryExitAnimations = value;
            }
        }
    }

    public string ConversationPrefillText
    {
        get { return conversationPrefillText; }
        set
        {
            if (conversationPrefillText != value)
            {
                conversationPrefillText = value;
            }
        }
    }

    public string ApiKey
    {
        get { return apiKey; }
        set
        {
            if (apiKey != value)
            {
                apiKey = value;
            }
        }
    }

    public string DomainName
    {
        get { return domainName; }
        set
        {
            if (domainName != value)
            {
                domainName = value;
            }
        }
    }

    public string AndroidAppId
    {
        get { return androidAppId; }
        set
        {
            if (androidAppId != value)
            {
                androidAppId = value;
            }
        }
    }

    public string iOSAppId
    {
        get { return iosAppId; }
        set
        {
            if (iosAppId != value)
            {
                iosAppId = value;
            }
        }
    }

    public string UnityGameObject
    {
        get { return unityGameObject; }
        set
        {
            if (unityGameObject != value)
            {
                unityGameObject = value;
            }
        }
    }

    public string NotificationIcon
    {
        get { return notificationIcon; }
        set
        {
            if (notificationIcon != value)
            {
                notificationIcon = value;
            }
        }
    }

    public string LargeNotificationIcon
    {
        get { return largeNotificationIcon; }
        set
        {
            if (largeNotificationIcon != value)
            {
                largeNotificationIcon = value;
            }
        }
    }

    public string NotificationSound
    {
        get { return notificationSound; }
        set
        {
            if (notificationSound != value)
            {
                notificationSound = value;
            }
        }
    }

    public string CustomFont
    {
        get { return customFont; }
        set
        {
            if (customFont != value)
            {
                customFont = value;
            }
        }
    }

	public string SupportNotificationChannel
	{
		get { return supportNotificationChannel; }
		set
		{
			if (supportNotificationChannel != value)
			{
				supportNotificationChannel = value;
			}
		}
	}

	public string CampaignsNotificationChannel
	{
		get { return campaignsNotificationChannel; }
		set
		{
			if (campaignsNotificationChannel != value)
			{
				campaignsNotificationChannel = value;
			}
		}
	}

    public bool EnableInboxPolling
    {
        get { return enableInboxPolling; }
        set
        {
            if (enableInboxPolling != value)
            {
                enableInboxPolling = value;
            }
        }
    }

    public bool EnableLogging
    {
        get { return enableLogging; }
        set
        {
            if (enableLogging != value)
            {
                enableLogging = value;
            }
        }
    }

	public bool EnableTypingIndicator
	{
		get { return enableTypingIndicator; }
		set
		{
			if (enableTypingIndicator != value)
			{
				enableTypingIndicator = value;
			}
		}
	}

    public int ScreenOrientation
    {
        get { return screenOrientation; }
        set
        {
            if (screenOrientation != value)
            {
                screenOrientation = value;
            }
        }
    }

	public string SupportedFileFormats
	{
		get { return supportedFileFormats; }
		set
		{
			if (supportedFileFormats != value)
			{
				supportedFileFormats = value;
			}
		}
	}

	public bool ShowConversationInfoScreen
	{
		get { return showConversationInfoScreen; }
		set
		{
			if (showConversationInfoScreen != value)
			{
				showConversationInfoScreen = value;
			}
		}
	}

    public Dictionary<string, object> InstallConfig
    {
        get { return instance.getInstallConfig(); }
    }

    public Dictionary<string, object> ApiConfig
    {
        get { return instance.getApiConfig(); }
    }

    public void SaveConfig()
    {
#if !UNITY_WEBPLAYER

#if UNITY_EDITOR
        EditorUtility.SetDirty(Instance);
        AssetDatabase.SaveAssets();
        string apiJson = Json.Serialize(instance.ApiConfig);
        string installJson;

        // We save config for iOS and android together whenever there is a change in config.
        // We read the __hs__appId parameter separately for Android and iOS config so that the generated json
        // remains independent of the current platform being built on Unity.

        // Common config
        Dictionary<string, object> installDictionary = instance.InstallConfig;

        // Save the Android app id first
        installDictionary.Add("__hs__appId", instance.AndroidAppId);
        installJson = Json.Serialize(installDictionary);

        string androidSdkPath = Path.Combine(Application.dataPath, "Plugins/Android/helpshift");
        if (Directory.Exists(androidSdkPath))
        {
            string androidPath = Path.Combine(Application.dataPath, "Plugins/Android/helpshift/res/raw/");
            if (!Directory.Exists(androidPath))
            {
                AssetDatabase.CreateFolder("Assets/Plugins/Android/helpshift/res", "raw");
                androidPath = Path.Combine(Application.dataPath, "Plugins/Android/helpshift/res/raw/");
            }
            System.IO.File.WriteAllText(androidPath + "helpshiftapiconfig.json", apiJson);
            System.IO.File.WriteAllText(androidPath + "helpshiftinstallconfig.json", installJson);
        }

        // Save the iOS app id
        installDictionary ["__hs__appId"] = instance.iOSAppId;
        installJson = Json.Serialize(installDictionary);
        string iosPath = Path.Combine(Application.dataPath, "Helpshift/Plugins/iOS/");
        if (Directory.Exists(iosPath))
        {
            System.IO.File.WriteAllText(iosPath + "HelpshiftApiConfig.json", apiJson);
            System.IO.File.WriteAllText(iosPath + "HelpshiftInstallConfig.json", installJson);
        }
#endif
#endif
    }

    public Dictionary<string, object> getApiConfig()
    {
        Dictionary<string, object> configDictionary = new Dictionary<string, object>();
        string enableContactUsString = instance.contactUsOptions [instance.contactUsOption];
        configDictionary.Add("enableContactUs", enableContactUsString);

        configDictionary.Add("gotoConversationAfterContactUs", instance.gotoConversation == true ? "yes" : "no");
        configDictionary.Add("presentFullScreenOniPad", instance.presentFullScreen == true ? "yes" : "no");
        configDictionary.Add("requireEmail", instance.requireEmail == true ? "yes" : "no");
        configDictionary.Add("hideNameAndEmail", instance.hideNameAndEmail == true ? "yes" : "no");
        configDictionary.Add("enableFullPrivacy", instance.enablePrivacy == true ? "yes" : "no");
        configDictionary.Add("showSearchOnNewConversation", instance.showSearchOnNewConversation == true ? "yes" : "no");
        configDictionary.Add("showConversationResolutionQuestion", instance.showConversationResolutionQuestion == 1 ? "yes" : "no");
		configDictionary.Add("enableTypingIndicator", instance.enableTypingIndicator ? "yes" : "no");

        configDictionary.Add("conversationPrefillText", instance.conversationPrefillText);
		configDictionary.Add ("showConversationInfoScreen", instance.showConversationInfoScreen ? "yes" : "no");
        return configDictionary;
    }

    public Dictionary<string, object> getInstallConfig()
    {
        Dictionary<string, object> installDictionary = new Dictionary<string, object>();

        installDictionary.Add("sdkType", "unity");
        installDictionary.Add("pluginVersion", pluginVersion);
        installDictionary.Add("runtimeVersion", Application.unityVersion);
        installDictionary.Add("unityGameObject", instance.unityGameObject);
        installDictionary.Add("notificationIcon", instance.notificationIcon);
        installDictionary.Add("largeNotificationIcon", instance.largeNotificationIcon);
        installDictionary.Add("notificationSound", instance.notificationSound);
        installDictionary.Add("font", instance.customFont);
		installDictionary.Add("supportNotificationChannelId", instance.supportNotificationChannel);
		installDictionary.Add("campaignsNotificationChannelId", instance.campaignsNotificationChannel);
        installDictionary.Add("enableInAppNotification", instance.enableInAppNotification == 0 ? "no" : "yes");
        installDictionary.Add("enableDefaultFallbackLanguage", instance.enableDefaultFallbackLanguage == 0 ? "no" : "yes");
        installDictionary.Add("disableEntryExitAnimations", instance.disableEntryExitAnimations == true ? "yes" : "no");
        installDictionary.Add("__hs__apiKey", instance.ApiKey);
        installDictionary.Add("__hs__domainName", instance.DomainName);
        installDictionary.Add("enableInboxPolling", instance.enableInboxPolling ? "yes" : "no");
        installDictionary.Add("enableLogging", instance.enableLogging ? "yes" : "no");
        installDictionary.Add("screenOrientation", instance.screenOrientation);
		installDictionary.Add ("supportedFileFormats", instance.supportedFileFormats);
        return installDictionary;
    }
}
