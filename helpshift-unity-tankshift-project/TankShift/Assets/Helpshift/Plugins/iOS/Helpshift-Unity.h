#import <Helpshift/HelpshiftSupport.h>

extern "C" void UnitySendMessage(const char *, const char *, const char *);

/* Initialize helpshift support (with Optional Arguments)
 *  When initializing Helpshift you must pass these three tokens along with a json encoded dictionary of additional configuration options.
 */
extern "C" void hsInstallForApiKeyWithOptions(const char *apiKey, const char *domainName, const char *appId, const char *optionsDictionaryString);

extern "C" void hsInstallFromCache();

/** Show the helpshift conversation screen (with Optional Arguments)
 *
 * To show the Helpshift conversation screen with optional arguments you will need to pass the name of the viewcontroller on which the conversation screen will show up and an options dictionary. If you do not want to pass any options then just pass nil which will take on the default options.
 *
 * @param optionsDictionaryString the dictionary which will contain the arguments passed to the Helpshift conversation session (that will start with this method call).
 *
 * Please check the docs for available options.
 *
 * @available Available in SDK version 4.0.0 or later
 */

extern "C" void hsShowConversationWithOptions(const char *optionsDictionaryString);

/** Show the helpshift screen with faqs from a particular section
 *
 * To show the Helpshift screen for showing a particular faq section you need to pass the publish-id of the faq section and the name of the viewcontroller on which the faq section screen will show up. For example from inside a viewcontroller you can call the Helpshift faq section screen by passing the argument “self” for the viewController parameter. If you do not want to pass any options then just pass nil which will take on the default options.
 *
 * @param faqSectionPublishID the publish id associated with the faq section which is shown in the FAQ page on the admin side (__yourcompanyname__.helpshift.com/admin/faq/).
 * @param optionsDictionaryString the dictionary which will contain the arguments passed to the Helpshift session (that will start with this method call).
 *
 * @available Available in SDK version 2.0.0 or later
 */

extern "C" void hsShowFAQSectionWithOptions(const char *faqSectionPublishID, const char *optionsDictionaryString);

/** Show the helpshift screen with a single faq
 *
 * To show the Helpshift screen for showing a single faq you need to pass the publish-id of the faq and the name of the viewcontroller on which the faq screen will show up. For example from inside a viewcontroller you can call the Helpshift faq section screen by passing the argument “self” for the viewController parameter. If you do not want to pass any options then just pass nil which will take on the default options.
 *
 * @param faqPublishID the publish id associated with the faq which is shown when you expand a single FAQ (__yourcompanyname__.helpshift.com/admin/faq/)
 * @param optionsDictionaryString the dictionary which will contain the arguments passed to the Helpshift session (that will start with this method call).
 *
 * @available Available in SDK version 4.0.0 or later
 */

extern "C" void hsShowSingleFAQWithOptions(const char *faqPublishID, const char *optionsDictionaryString);

/** Show the support screen with only the faqs (with Optional Arguments)
 *
 * To show the Helpshift screen with only the faq sections with search with optional arguments, you can use this api. This screen will not show the issues reported by the user. If you do not want to pass any options then just pass nil which will take on the default options.
 *
 * @param optionsDictionaryString the dictionary which will contain the arguments passed to the Helpshift faqs screen session (that will start with this method call).
 *
 * Please check the docs for available options.
 *
 * @available Available in SDK version 2.0.0 or later
 */
extern "C" void hsShowFAQsWithOptions(const char *optionsDictionaryString);

/*
 * Set the unique udentifier for your users.
 */
extern "C" void hsSetUserIdentifier(const char *userIdentifier);

/*
 * Set the user-name for your users
 */
extern "C" void hsSetNameAndEmail(const char *userName, const char *email);

/*
 * Add extra debug information regarding user-actions.
 * You can add additional debugging statements to your code, and see exactly what the user was doing right before they reported the issue.
 */
extern "C" void hsLeaveBreadCrumb(const char *breadCrumb);

/*
 * Clears Breadcrumbs list.
 *
 * Breadcrumbs list stores upto 100 latest actions. You'll receive those in every Issue.
 * If for some reason you want to clear previous messages, you can use this method.
 */
extern "C" void hsClearBreadCrumbs(void);

/** Get the notification count for replies to new conversations.
 *
 *
 * If you want to show your user notifications for replies on any ongoing conversation, you can get the notification count asynchronously by implementing the HelpshiftSupportDelegate in your respective .h and .m files.
 * This will return a notification count in the
 * - (void) didReceiveUnreadMessagesCount:(NSInteger)count
 * count delegate method.
 *
 * @param isRemote get the count from network or from the local DB
 *
 * @available Available in SDK version 6.1.0 or later
 */
extern "C" void hsRequestUnreadMessagesCount(bool isRemote);

/*
 * Register the deviceToken to enable push notifications
 *
 * To enable push notifications in the Helpshift iOS SDK, set the Push Notifications’ deviceToken using this method.
 */

extern "C" void hsRegisterDeviceToken(const char *deviceToken);

extern "C" void hsRegisterDeviceTokenData(NSData *deviceTokenData);

/*
 * Forward the push notification for the Helpshift lib to handle
 *
 */

extern "C" void hsHandleRemoteNotification(const char *notificationDataCharString);

/*
 * Forward the local notification for the Helpshift lib to handle
 *
 * To show support on Notification opened, call handleLocalNotificationForIssue when you receive a local notification with the value of
 * the “origin” field is “helpshift” in the userInfo dictionary of the notification object.
 * Pass the issue_id received from the notification dictionary to this api to launch the appropriate issue.
 *
 */

extern "C" void hsHandleLocalNotificationForIssue(const char *issue_id);

/*
 * Set the metaData which will be sent along with the reported issue
 *
 * This api is meant to be called from the updateMetaData message handler in the Unity script.
 *
 */
extern "C" void hsSetMetaData(const char *metaDictionaryString);

/* To pause and restart the display of inapp notification
 *
 * When this method is called with boolean value YES, inapp notifications are paused and not displayed.
 * To restart displaying inapp notifications pass the boolean value NO.
 *
 * @param pauseInApp the boolean value to pause/restart inapp nofitications
 *
 */

extern "C" void hsPauseDisplayOfInAppNotification(bool pauseInApp);

/* Show alert for app rating
 *
 * To manually show an alert for app rating, you need automated reviews disabled in admin.
 * Also, if there is an ongoing conversation, the review alert will not show up.
 *
 * @param url the app store URL for your app
 */

extern "C" void hsShowAlertToRateAppWithURL(const char *url);

extern "C" void hsLogin(const char *identifier, const char *name, const char *email);

extern "C" void hsLoginWithUser(const char *helpshiftUser);

extern "C" void hsLogout();

extern "C" void hsClearAnonymousUser();

extern "C" void hsSetLanguage(const char *locale);

extern "C" void hsSetTheme(const char *themeFileName);

extern "C" void hsSetThemes(const char *lightThemeFileName, const char *darkThemeFileName);

extern "C" void hsShowDynamicForm(const char *, const char *);

extern "C" void hsShowDynamicFormWithConfig(const char *title, const char *flowsJson, const char *config);

/**
 *  API to log messages to console and upload as metadata when filing an issue.
 *
 *  @param log The log message
 */
extern "C" void hsLog(const char *log);

/**
 *  API to check if there is an ongoing conversation
 *
 *  Result will be returned in the didCheckIfConversationActive delegate
 */
extern "C" void hsCheckIfConversationActive();

/**
 *  API to close the ongoing support session
 *
 */
extern "C" void hsCloseHelpshiftSupportSession();

/*
 * API for enabling testing mode of the SDK
 */
extern "C" void hsEnableTestingMode();

/**
 * Function pointer for calling C# methods from Objective C. Used for calling support only specific delegate methods
 * in C#. Campaigns delegate methods are handled by hsUnityMessageCallback function pointer.
 */
extern "C" typedef void (*hsUnitySupportMessageCallback)(const char * /* object name */, const char * /* method_name */, const char * /*parameter*/);

extern "C" void hsRegisterUnitySupportMessageCallback(hsUnitySupportMessageCallback unitySupportMessageCallback);

/*
 * This method returns the function pointer of hsUnityMessageCallback type to C#. Used for campaigns delegate methods
 */
extern "C" typedef void (*hsUnityMessageCallback)(const char * /* method_name <hs_separator> parameters */);

@interface HelpshiftUnity : NSObject

@end

/* Class interface for Inbox callback bridge.
 *
 * This class is the callback bridge between C# and objc.
 */
@interface HsUnityCallbackBridge : NSObject

@property hsUnityMessageCallback unityMessageCallback;

@end

#ifdef HELPSHIFT

#include "HelpshiftCampaigns.h"

#pragma mark Campaigns Inbox callback message

/* Class interface for Inbox delegate listener.
 *
 * Listens to the delegates of ObjC.
 */
@interface HsInboxDelegateListener : NSObject<HelpshiftInboxDelegate>

/* Register For Delegate
 *
 * Register ts class for listeing to delegates
 */
+ (void) registerForDelegate;

@end


/* Class interface for Inbox notification delegate listener.
 *
 * Listens to the delegates of ObjC.
 */
@interface HsInboxNotificationDelegateListener : NSObject<HelpshiftInboxNotificationDelegate>

/* Register For Delegate
 *
 * Register ts class for listeing to delegates
 */
+ (void) registerForDelegate;

@end

/* Class interface for Helpshift Campaigns Delegate listener.
 *
 * Listens to the delegates of ObjC.
 */
@interface HsCampaignsDelegateListener : NSObject<HelpshiftCampaignsDelegate>

/* Register For Delegate
 *
 * Register ts class for listeing to delegates
 */
+ (void) registerForDelegate;

@end

#pragma mark Inbox Delegates register methods
/* Set inbox message delegate
 *
 * Register the inbox message delegates when set from C#.
 */
extern "C" void hsSetInboxMessageDelegate();

/* Set inbox push notification delegate.
 *
 * Register the inbox push notification message delegates when set from C#.
 */
extern "C" void hsSetInboxPushNotificationDelegate();

extern "C" void hsSetCampaignsDelegate();

#pragma mark Data APIs
/* Mark inbox message as read.
 *
 * Manuall mark an inbox message as read.
 *
 * @param messageID the message identifier for campaigns message.
 */
extern "C" void hsMarkInboxMessageAsRead(const char *messageId);

/* Mark inbox message as seen.
 *
 * Manually mark an inbox message as seen.
 *
 * @param messageID the message identifier for campaigns message.
 */
extern "C" void hsMarkInboxMessageAsSeen(const char *messageId);

/* Delete inbox message.
 *
 * Manually delete an inbox message.
 *
 * @param messageID the message identifier for campaigns message.
 */
extern "C" void hsDeleteInboxMessage(const char *messageId);

/* Get inbox message.
 *
 * Get the instance of inbox message.
 *
 * @param messageID the message identifier for campaigns message.
 */
extern "C" char *hsGetInboxMessage(const char *messageId);

/* Get all inbox message.
 *
 * Get array of all the inbox messages present.
 *
 */
extern "C" char *hsGetAllInboxMessages();

/* Show inbox message.
 *
 * Manually show an inbox message to the user.
 *
 * @param messageID the message identifier for campaigns message.
 * @param optionsDictionaryString the config for showing the campaigns message.
 */
extern "C" void hsShowInboxMessage(const char *messageId, const char *optionsDictionaryString);

/* Execute action at index.
 *
 * Manually execute an action of inbox message.
 *
 * @param messageID the message identifier for campaigns message.
 * @param index the action index for campaigns message.
 */
extern "C" void hsExecuteActionAtIndex(const char *identifier, int index);

extern "C" bool hsAddPropertyInteger(const char *key,int value);

extern "C" bool hsAddPropertyLong(const char *key,long long value);

extern "C" bool hsAddPropertyString(const char *key,const char *value);

extern "C" bool hsAddPropertyBoolean(const char *key,bool value);

extern "C" bool hsAddPropertyDate(const char *key,double value);

extern "C" char *hsAddProperties(const char *value);

extern "C" void hsShowInboxWithOptions(const char *optionsDictionaryString);

extern "C" void hsShowMessageWithIdAndOptions(const char *messageId, const char *optionsDictionaryString);

extern "C" int hsGetCountOfUnreadMessages();

extern "C" void hsRequestCampaignsUnreadMessagesCount();

extern "C" void hsRefetchMessages();

extern "C" void hsGetCoverImage(const char *messageId);

#endif /* ifdef HELPSHIFT */
