using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(HelpshiftConfig))]
public class HelpshiftConfigEditor : Editor {

	bool showHelpshiftConfig = false;

	GUIContent apiKeyLabel = new GUIContent("Api Key [?]:", "Helpshift App Ids can be found at https://example.helpshift.com/admin/settings/download-sdk/");
	GUIContent domainNameLabel = new GUIContent("Domain Name [?]:", "Helpshift's domain name\n For example, 'example.helpshift.com'");
	GUIContent androidAppIdLabel = new GUIContent("Android App Id [?]:", "Helpshift App Ids can be found at https://example.helpshift.com/admin/settings/download-sdk/");
	GUIContent iOSAppIdLabel = new GUIContent("iOS App Id [?]:", "Helpshift App Ids can be found at https://example.helpshift.com/admin/settings/download-sdk/");
	GUIContent enableInAppNotificationLabel = new GUIContent(" Enable In-app Notifications [?]:", "The enableInAppNotification flag controls whether in app notifications will be shown for Helpshift conversation updates");
	GUIContent disableEntryExitAnimationsLabel = new GUIContent(" Disable Entry Exit Animations [?]:", "The disableEntryExitAnimations flag controls whether Helpshift SDK will show animations while entering and exiting Helpshift screens.");
	GUIContent enableInboxPollingLabel = new GUIContent(" Enable Inbox polling [?]:", "Enables/Disables polling for syncing Inbox messages for Campaigns.");
	GUIContent enableLoggingLabel = new GUIContent(" Enable logging on console [?]:", "Enables/Disables console logging for Helpshift SDK.");
	GUIContent screenOrientationLabel = new GUIContent("Default screen orientation [?]:", "The screen orientation of Helpshift SDK screens can be fixed by this flag. The value should be one of the constants available in the android ActivityInfo class for screen orientation. For example the value of the constant ActivityInfo.SCREEN_ORIENTATION_LANDSCAPE");
	GUIContent supportedFileFormatsLabel = new GUIContent ("Supported file formats [?]:", "Comma separated values of the file extensions. " +
													  "If the attachment received in Helpshift conversation cannot be opened by any apps on the device and the attachment extension is in the supported list then the SDK will provide a callback on " +
													  "the 'displayAttachmentFile' method along with the attachment file path. Refer https://developers.helpshift.com/unity/delegates-android/#display-attachment");

	GUIContent enableTypingIndicatorLabel = new GUIContent(" Enable typing indicator on Conversation screen [Deprecated] [?]:", 
															"Enables/Disables graphical indicator shown to the end user if an Agent is currently replying to the same conversation." +
															"\n Note: This config is deprecated from SDK version 4.0.0. Please turn on/off this config from app settings (In App SDK configuration page on Admin dashboard)");

	GUIContent enableContactUsLabel = new GUIContent(" Contact Us [?]:", "The enableContactUs flag controls the visibility of Contact Us button");

	GUIContent gotoConvLabel = new GUIContent(" Go to Conversation [Deprecated] [?]", "Determines which screen a user sees after filing an issue via 'Contact Us'." +
											  "\n Note: This config is applicable only for form based issue filing experience which is deprecated from SDK version 4.0.0");

	GUIContent presentFullScreenLabel = new GUIContent(" Present in Fullscreen on iPad [?]", "The presentFullScreenOniPad flag will determine whether to present support views in UIModalPresentationFullScreen or UIModalPresentationFormSheet modal presentation style in iPad. Only takes effect on iPad.");

	GUIContent requireEmailLabel = new GUIContent(" Require Email [Deprecated] [?]", "The requireEmail option determines whether email is required or optional for starting a new conversation." +
												  "\n Note: This config is applicable only for form based issue filing experience which is deprecated from SDK version 4.0.0");
	GUIContent hideNameAndEmailLabel = new GUIContent(" Hide Name and Email [Deprecated] [?]", "The hideNameAndEmail flag will hide the name and email fields when the user starts a new conversation." +
													  "\n Note: This config is applicable only for form based issue filing experience which is deprecated from SDK version 4.0.0");

	GUIContent enablePrivacyLabel = new GUIContent(" Enable Privacy [?]", "In scenarios where the user attaches objectionable content in the screenshots, it becomes a huge COPPA concern. The enableFullPrivacy option will help solve this problem.");

	GUIContent showSearchLabel = new GUIContent(" Show Search results after New Conversation [Deprecated] [?]", "Use this option to provide better ticket deflection." +
												"\n Note: This config is applicable only for form based issue filing experience which is deprecated from SDK version 4.0.0");

	GUIContent showConversationResolutionLabel = new GUIContent(" Show Conversation Resolution Question [Deprecated] [?]", "Use this option to show conversation resolution question to the user." +
																"\n Note: This config is deprecated from SDK version 4.0.0. Please turn on/off this config from app settings (In App SDK configuration page on Admin dashboard)");

	GUIContent enableDefaultFallbackLabel = new GUIContent(" Enable fallback to default language. [?]", "Use this option to enable fallback to default language that is English for FAQs.");

	GUIContent conversationPrefillLabel = new GUIContent(" Conversation Prefill Text [Deprecated] [?]", "The conversationPrefillText API option prefills a new conversation with the supplied string. " +
														"You can use this option to add crash logs to a new conversation and prompt the user to send those logs as a support ticket. " +
														"You can also use this option to set context depending on where and when in the app showConversation is being launched from." +
														"\n Note: This config is applicable only for form based issue filing experience which is deprecated from SDK version 4.0.0");

	GUIContent showConversationInfoScreenLabel = new GUIContent(" Show Conversation Info screen [?]", "This flag will determine if users can see the Conversation info screen for an ongoing Conversation. If set, the Conversation info icon will be shown in the active Conversation screen.");


	public override void OnInspectorGUI () {

		HelpshiftConfig helpshiftConfig = HelpshiftConfig.Instance;

		EditorGUILayout.LabelField("Install Time Configurations");
		EditorGUILayout.HelpBox("1) Add the Unity game object which will respond to Helpshift callbacks", MessageType.None);
        helpshiftConfig.UnityGameObject = EditorGUILayout.TextField(helpshiftConfig.UnityGameObject);
        EditorGUILayout.HelpBox("2) Filename of the notification icon which you want to display in the notification center. (Android only)", MessageType.None);
        helpshiftConfig.NotificationIcon = EditorGUILayout.TextField(helpshiftConfig.NotificationIcon);
		EditorGUILayout.HelpBox("3) Filename of the large notification icon which you want to display in the notification center. (Android Only)", MessageType.None);
		helpshiftConfig.LargeNotificationIcon = EditorGUILayout.TextField(helpshiftConfig.LargeNotificationIcon);
		EditorGUILayout.HelpBox("4) Filename of the notification sound file which you want to use for Helpshift notifications. (Android only)", MessageType.None);
		helpshiftConfig.NotificationSound = EditorGUILayout.TextField(helpshiftConfig.NotificationSound);
        EditorGUILayout.HelpBox("5) Filename of the Custom Font (Path relative to \"assets\" folder) (Android only)", MessageType.None);
        helpshiftConfig.CustomFont = EditorGUILayout.TextField(helpshiftConfig.CustomFont);
		EditorGUILayout.HelpBox("6) Notification Channel ID for Support notifications (Android only)", MessageType.None);
		helpshiftConfig.SupportNotificationChannel = EditorGUILayout.TextField(helpshiftConfig.SupportNotificationChannel);
		EditorGUILayout.HelpBox("7) Notification Channel ID for Campaigns notifications (Android only)", MessageType.None);
		helpshiftConfig.CampaignsNotificationChannel = EditorGUILayout.TextField(helpshiftConfig.CampaignsNotificationChannel);

        EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(apiKeyLabel);
		helpshiftConfig.ApiKey = EditorGUILayout.TextField(helpshiftConfig.ApiKey);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(domainNameLabel);
		helpshiftConfig.DomainName = EditorGUILayout.TextField(helpshiftConfig.DomainName);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(androidAppIdLabel);
		helpshiftConfig.AndroidAppId = EditorGUILayout.TextField(helpshiftConfig.AndroidAppId);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(iOSAppIdLabel);
        helpshiftConfig.iOSAppId = EditorGUILayout.TextField(helpshiftConfig.iOSAppId);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		helpshiftConfig.EnableInAppNotification = EditorGUILayout.ToggleLeft(enableInAppNotificationLabel, helpshiftConfig.EnableInAppNotification);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		helpshiftConfig.EnableInboxPolling = EditorGUILayout.ToggleLeft(enableInboxPollingLabel, helpshiftConfig.EnableInboxPolling);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		helpshiftConfig.EnableLogging = EditorGUILayout.ToggleLeft(enableLoggingLabel, helpshiftConfig.EnableLogging);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		helpshiftConfig.EnableDefaultFallbackLanguage = EditorGUILayout.ToggleLeft(enableDefaultFallbackLabel, helpshiftConfig.EnableDefaultFallbackLanguage);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		helpshiftConfig.DisableEntryExitAnimations = EditorGUILayout.ToggleLeft(disableEntryExitAnimationsLabel, helpshiftConfig.DisableEntryExitAnimations);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(screenOrientationLabel);
		helpshiftConfig.ScreenOrientation = EditorGUILayout.IntField(helpshiftConfig.ScreenOrientation);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(supportedFileFormatsLabel);
		helpshiftConfig.SupportedFileFormats = EditorGUILayout.TextField(helpshiftConfig.SupportedFileFormats);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("SDK Configurations");

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(enableContactUsLabel);
		EditorGUILayout.EndHorizontal();
		helpshiftConfig.ContactUs = EditorGUILayout.Popup(helpshiftConfig.ContactUs,
		                             new string[]{"Always", "Never", "After viewing FAQs", "After marking answer unhelpful"});

		EditorGUILayout.Space();

		showHelpshiftConfig = EditorGUILayout.Foldout(showHelpshiftConfig, "SDK Configuration flags");
		if (showHelpshiftConfig)
		{
			helpshiftConfig.GotoConversation = EditorGUILayout.ToggleLeft(gotoConvLabel, helpshiftConfig.GotoConversation);
			helpshiftConfig.PresentFullScreenOniPad = EditorGUILayout.ToggleLeft(presentFullScreenLabel, helpshiftConfig.PresentFullScreenOniPad);
			helpshiftConfig.RequireEmail = EditorGUILayout.ToggleLeft(requireEmailLabel, helpshiftConfig.RequireEmail);
			helpshiftConfig.HideNameAndEmail = EditorGUILayout.ToggleLeft(hideNameAndEmailLabel, helpshiftConfig.HideNameAndEmail);
			helpshiftConfig.EnablePrivacy = EditorGUILayout.ToggleLeft(enablePrivacyLabel, helpshiftConfig.EnablePrivacy);
			helpshiftConfig.ShowSearchOnNewConversation = EditorGUILayout.ToggleLeft(showSearchLabel, helpshiftConfig.ShowSearchOnNewConversation);
			helpshiftConfig.ShowConversationResolutionQuestion = EditorGUILayout.ToggleLeft(showConversationResolutionLabel, helpshiftConfig.ShowConversationResolutionQuestion);
			helpshiftConfig.ShowConversationInfoScreen = EditorGUILayout.ToggleLeft(showConversationInfoScreenLabel, helpshiftConfig.ShowConversationInfoScreen);
			helpshiftConfig.EnableTypingIndicator = EditorGUILayout.ToggleLeft(enableTypingIndicatorLabel, helpshiftConfig.EnableTypingIndicator);
		}

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(conversationPrefillLabel);
		helpshiftConfig.ConversationPrefillText = EditorGUILayout.TextField(helpshiftConfig.ConversationPrefillText);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		if (GUILayout.Button ("Save Config")) {
			helpshiftConfig.SaveConfig();
		}
		EditorGUILayout.EndHorizontal();

	}
}
