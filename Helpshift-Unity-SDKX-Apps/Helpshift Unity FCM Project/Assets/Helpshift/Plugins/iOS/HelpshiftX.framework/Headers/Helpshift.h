//
//  Helpshift.h
//  HelpshiftX
//
//  Copyright © 2019 Helpshift. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "HelpshiftDelegate.h"
NS_ASSUME_NONNULL_BEGIN

/// Key to be used for providing username in the [Helpshift loginUser:] method
static NSString *_Nonnull const HelpshiftUserName = @"userName";

/// Key to be used for providing user identifier in the loginUser: method
static NSString *_Nonnull const HelpshiftUserIdentifier = @"userId";

/// Key to be used for providing user email in the loginUser: method
static NSString *_Nonnull const HelpshiftUserEmail = @"userEmail";

/// Key to be used for providing user's authentication token in the loginUser: method
static NSString *_Nonnull const HelpshiftUserAuthToken = @"userAuthToken";

/// Classes which provides access to all Helpshift APIs
@interface Helpshift : NSObject

/// The delegate to be set to in order to receive events from Helpshift
@property (nonatomic, weak) id<HelpshiftDelegate> delegate;

/** Returns an instance of Helpshift
 *
 * When using Helpshift, use below method to get the singleton instance of this class.
 * You can set the delegate using `Helpshift.sharedInstance.delegate = yourDelegate;`
 *
 */
+ (Helpshift *) sharedInstance;

/** Initialize helpshift support
 *
 * When initializing Helpshift you must pass these two tokens. You initialize Helpshift by adding the following lines in the implementation file for your AppDelegate, ideally at the top of `application:didFinishLaunchingWithOptions`. This method can throw the `InstallException` asynchronously if the install keys are not in the correct format.
 *  @param platformId This is your platform id API Key
 *  @param domain This is your domain name
 *  @param config This is the config dictionary represents the configuration parameters for the installation
 */
+ (void) installWithPlatformId:(NSString *)platformId
    domain:(NSString *)domain
    config:(nullable NSDictionary *)config;

/**
 *  This method pauses/restarts the display in-app notification.
 *
 *  @param shouldPauseInAppNotification the boolean value to pause/restart inapp nofitications
 */
+ (void) pauseDisplayOfInAppNotification:(BOOL)shouldPauseInAppNotification;

/**
 * Logs in a given user. This method accepts an `NSDictionary` whose keys are one of `HelpshiftUserName`, `HelpshiftUserIdentifier`, `HelpshiftUserEmail`, `HelpshiftUserAuthToken`
 * These are string constants defined in `Helpshift.h` file.
 *
 * @param userDetails The `NSDictionary` representing the user details
 */
+ (void) loginUser:(NSDictionary<NSString *, NSString *> *)userDetails;

/**
 * Logs out the previously logged in user
 */
+ (void) logout;

/**
 * Call this API if you need to clear the anonymous user data on login
 */
+ (void) clearAnonymousUserOnLogin;

/**
 * Shows the helpshift support conversation screen.
 *
 * @param viewController The `UIViewController` on which the conversation screen is to be shown
 * @param config An `NSDictionary` which represents the configuration that needs to be set to the conversation
 */
+ (void) showConversationWith:(UIViewController *)viewController config:(nullable NSDictionary *)config;

/**
 * Show the helpshift help center screen.
 *
 * @param viewController The `UIViewController` on which the help center screen is to be shown
 * @param config An `NSDictionary` which represents the configuration that needs to be set to the help center
 */
+ (void) showFAQsWith:(UIViewController *)viewController config:(nullable NSDictionary *)config;

/**
 * Show the helpshift help center screen with FAQs from a particular section
 *
 * @param faqSectionPublishID Publish ID of FAQ section which is shown in the FAQ page on the admin side
 * (__yourcompanyname__.helpshift.com/admin/faq/).
 * @param viewController The `UIViewController` on which the help center screen is to be shown
 * @param config An `NSDictionary` which represents the configuration that needs to be set to the help center
 */
+ (void) showFAQSection:(NSString *)faqSectionPublishID with:(UIViewController *)viewController config:(nullable NSDictionary *)config;

/**
 * Show the helpshift help center screen with a single FAQ
 *
 * @param faqPublishID Publish ID of FAQ which is shown when you expand a single FAQ on admin side
 * (__yourcompanyname__.helpshift.com/admin/faq/)
 * @param viewController The `UIViewController` on which the help center screen is to be shown
 * @param config An `NSDictionary` which represents the configuration that needs to be set to the help center
 */
+ (void) showSingleFAQ:(NSString *)faqPublishID with:(UIViewController *)viewController config:(nullable NSDictionary *)config;

/** Change the SDK language. By default, the device's prefered language is used.
 * The call will fail in the following cases :
 * 1. If a Helpshift session is already active at the time of invocation
 * 2. Language code is incorrect
 * @param languageCode the string representing the language code. For example, use 'fr' for French.  Passing `nil` will unset any language set by previous invocation of this API.
 */
+ (void) setLanguage:(nullable NSString *)languageCode;

/** Register the deviceToken to enable push notifications
 * To enable push notifications in the Helpshift iOS SDK, set the Push Notifications’ deviceToken using this method inside your `application:didRegisterForRemoteNotificationsWithDeviceToken` in the AppDelegate.
 *  @param deviceToken The deviceToken received from the push notification servers.
 */
+ (void) registerDeviceToken:(NSData *)deviceToken;

/**
 *  Pass along the userInfo dictionary (received with a `UNNotification`) for the Helpshift SDK to handle
 *  @param userInfo   dictionary contained in the `UNNotification` object received in AppDelegate.
 *  @param viewController The `UIViewController` on which you want the Helpshift SDK stack to be shown
 *  @param isAppLaunch    A `BOOL` indicating whether the app was lanuched from a killed state. This parameter should ideally only be true in case when called from app's didFinishLaunchingWithOptions AppDelegate.
 *  @return BOOL value indicating whether Helpshift SDK handled this notification.
 */
+ (BOOL) handleNotificationWithUserInfoDictionary:(NSDictionary *)userInfo isAppLaunch:(BOOL)isAppLaunch withController:(UIViewController *)viewController;

/**
 * Fetches the unread messages count from local or remote based on `shouldFetchFromServer` flag.
 * Invokes the `[HelpshiftDelegate handleHelpshiftEvent:withData:]` with appropriate data as response.
 *
 * @param shouldFetchFromServer Should fetch unread message count from server.
 */
+ (void) requestUnreadMessageCount:(BOOL)shouldFetchFromServer;

@end

NS_ASSUME_NONNULL_END
