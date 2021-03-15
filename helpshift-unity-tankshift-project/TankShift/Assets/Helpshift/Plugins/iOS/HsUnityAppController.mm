//
//  HsUnityAppController.m
//

#import <Foundation/Foundation.h>
#import <Helpshift/HelpshiftCore.h>
#import <Helpshift/HelpshiftSupport.h>
// Uncomment the following code if you are using withCampaigns build.
/*
#import <Helpshift/HelpshiftAll.h>
*/
#import "UnityAppController.h"
#import <UserNotifications/UserNotifications.h>
#import "Helpshift-Unity.h"

@interface HsUnityAppController : UnityAppController<UNUserNotificationCenterDelegate>

@end

@implementation HsUnityAppController : UnityAppController

- (BOOL) application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions {

    // If you intend to initialize the SDK from Xcode using Objective C, you can initialize Helpshift with custom configuration by uncommenting the code below.

    // Set your custom configuration
    // HelpshiftInstallConfigBuilder *installConfigBuilder = [HelpshiftInstallConfigBuilder new];

    // Uncomment the following code if you are using withCampaigns build.
    /*
       [HelpshiftCore initializeWithProvider:[HelpshiftAll sharedInstance]];
       [HelpshiftCore installForApiKey:@"<your_api_key>" domainName:@"<your_domain_name>.helpshift.com" appID:@"<your_app_id>" withConfig:installConfigBuilder.build];
     */

    // Uncomment the following code if you are using support only build.
    /*
       [HelpshiftCore initializeWithProvider:[HelpshiftSupport sharedInstance]];
       [HelpshiftCore installForApiKey:@"<your_api_key>" domainName:@"<your_domain_name>.helpshift.com" appID:@"<your_app_id>" withConfig:installConfigBuilder.build];
     */

    /*
     Uncomment the following code block to register for push notifications using UNUserNotification framework.
     */
    /*
    UNUserNotificationCenter *center = [UNUserNotificationCenter currentNotificationCenter];
    center.delegate = self;
    [center requestAuthorizationWithOptions:(UNAuthorizationOptionBadge | UNAuthorizationOptionSound | UNAuthorizationOptionAlert)
                          completionHandler:^(BOOL granted, NSError *_Nullable error) {
                              if(error) {
                                  NSLog(@"Error while requesting Notification permissions.");
                              }
                          }];
    [[UIApplication sharedApplication] registerForRemoteNotifications];
    */

    /* Uncomment the following code block to register for push notification using UILocalNotification framework.
     * Please make sure that you are registering only from one framwork, Either from UNUserNotification or from UILocalNotification framework.
     */

    /**
     UIUserNotificationType notificationType = UIUserNotificationTypeBadge | UIUserNotificationTypeAlert;
     UIUserNotificationSettings *notificationSettings = [UIUserNotificationSettings settingsForTypes:notificationType categories:nil];
     [[UIApplication sharedApplication] registerUserNotificationSettings:notificationSettings];
     [[UIApplication sharedApplication] registerForRemoteNotifications];
     */

    if([launchOptions objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey]) {
        NSDictionary *payload = [launchOptions objectForKey:UIApplicationLaunchOptionsRemoteNotificationKey];
        if ([[payload objectForKey:@"origin"] isEqualToString:@"helpshift"]) {
            hsInstallFromCache();
            dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(2 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
                [HelpshiftCore handleNotificationWithUserInfoDictionary:payload
                                                            isAppLaunch:YES
                                                         withController:self.window.rootViewController];
            });
        }
    }
    return [super application:application didFinishLaunchingWithOptions:launchOptions];
}

- (void) application:(UIApplication *)app didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
    [super application:app didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];

    hsInstallFromCache();
    hsRegisterDeviceTokenData(deviceToken);
}

- (void) application:(UIApplication *)application handleEventsForBackgroundURLSession:(NSString *)identifier completionHandler:(void (^)())completionHandler {
    hsInstallFromCache();
    if (![HelpshiftCore handleEventsForBackgroundURLSession:identifier completionHandler:completionHandler]) {
        // Handle events for background url session. Once you have implemented this function in UnityAppController, uncomment
        // the code below and comment the call completionHandler();

        //[super application:application handleEventsForBackgroundURLSession:identifier completionHandler:completionHandler];
        completionHandler();
    }
}

- (void) application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo {
    hsInstallFromCache();
    if (![HelpshiftCore handleNotificationWithUserInfoDictionary:userInfo isAppLaunch:false withController:self.window.rootViewController]) {
        [super application:application didReceiveRemoteNotification:userInfo];
    }
}

- (void) application:(UIApplication *)application didReceiveRemoteNotification:(NSDictionary *)userInfo fetchCompletionHandler:(void (^)(UIBackgroundFetchResult))completionHandler {
    hsInstallFromCache();
    if (![HelpshiftCore handleNotificationWithUserInfoDictionary:userInfo isAppLaunch:false withController:self.window.rootViewController]) {
        [super application:application didReceiveRemoteNotification:userInfo fetchCompletionHandler:completionHandler];
    }
}

- (void) application:(UIApplication *)application didReceiveLocalNotification:(UILocalNotification *)notification {
    hsInstallFromCache();
    if (![HelpshiftCore handleNotificationWithUserInfoDictionary:notification.userInfo
                                                     isAppLaunch:false
                                                  withController:[UIApplication sharedApplication].keyWindow.rootViewController]) {
        [super application:application didReceiveLocalNotification:notification];
    }
}

- (void) application:(UIApplication *)application handleActionWithIdentifier:(NSString *)identifier forRemoteNotification:(NSDictionary *)userInfo completionHandler:(void (^)())completionHandler {
    hsInstallFromCache();

    if(![HelpshiftCore handleNotificationResponseWithActionIdentifier:identifier userInfo:userInfo completionHandler:completionHandler]) {
        // Handle action with identifier. Once you have implemented this function in UnityAppController, uncomment
        // the code below and comment the call completionHandler();

        //[super application:application handleActionWithIdentifier:identifier forRemoteNotification:userInfo completionHandler:completionHandler];
        completionHandler();
    }
}

- (void) application:(UIApplication *)application handleActionWithIdentifier:(NSString *)identifier forLocalNotification:(UILocalNotification *)notification completionHandler:(void (^)())completionHandler {
    hsInstallFromCache();

    if(![HelpshiftCore handleNotificationResponseWithActionIdentifier:identifier userInfo:notification.userInfo completionHandler:completionHandler]) {
        // Handle action with identifier. Once you have implemented this function in UnityAppController, uncomment
        // the code below and comment the call completionHandler();

        //[super application:application handleActionWithIdentifier:identifier forLocalNotification:notification completionHandler:completionHandler];
        completionHandler();
    }
}

/* UNUserNotification delegates implementation starts here.
 * If your app already implements these delegates than copy this code snippet to your implementation and remove this implementation.
 */
- (void) userNotificationCenter:(UNUserNotificationCenter *)center
        willPresentNotification:(UNNotification *)notification
          withCompletionHandler:(void (^)(UNNotificationPresentationOptions options))completionHandler {
    if([[notification.request.content.userInfo objectForKey:@"origin"] isEqualToString:@"helpshift"]) {
        hsInstallFromCache();
        [HelpshiftCore handleNotificationWithUserInfoDictionary:notification.request.content.userInfo
                                                    isAppLaunch:false
                                                 withController:self.window.rootViewController];
    }
    completionHandler(UNNotificationPresentationOptionNone);
}

- (void) userNotificationCenter:(UNUserNotificationCenter *)center didReceiveNotificationResponse:(UNNotificationResponse *)response withCompletionHandler:(void (^)(void))completionHandler {
    if([[response.notification.request.content.userInfo objectForKey:@"origin"] isEqualToString:@"helpshift"]) {
        hsInstallFromCache();
        [HelpshiftCore handleNotificationResponseWithActionIdentifier:response.actionIdentifier
                                                             userInfo:response.notification.request.content.userInfo
                                                    completionHandler:completionHandler];
    }
}
// UNUserNotification delegates implemenation ends here.

// Remove the following code if you are not using faq deeplinks in your application.

- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey,id> *)options {
    if([[url host] isEqualToString:@"helpshift"]) {
        hsInstallFromCache();
        NSArray *components = [[url path] componentsSeparatedByString:@"/"];
        if([components count] == 3) {
            if([[components objectAtIndex:1] isEqualToString:@"section"]) {
                [HelpshiftSupport showFAQSection:[components objectAtIndex:2] withController:[UIApplication sharedApplication].keyWindow.rootViewController withConfig:nil];
            } else if([[components objectAtIndex:1] isEqualToString:@"faq"]) {
                [HelpshiftSupport showSingleFAQ:[components objectAtIndex:2] withController:[UIApplication sharedApplication].keyWindow.rootViewController withConfig:nil];
            }
        }
        return true;
    }
    return [super application:app openURL:url options:options];
}

@end

IMPL_APP_CONTROLLER_SUBCLASS(HsUnityAppController)
