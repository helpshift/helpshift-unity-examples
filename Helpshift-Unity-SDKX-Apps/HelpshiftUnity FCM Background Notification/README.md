# Helpshift Unity SDK X Sample Android Project to demonstrate handling of FCM push notifications in background/killed state. 

A sample android studio project that would generate a plain .jar file to demonstrate use of FCM Unity plugin with Helpshift Unity SDK X and handle Helpshift push notifications in app background/killed state.

HelpshiftCustomFCMService class extends an internal service, i.e ListenerService, from FCM Unity plugin. This service is responsible for generating/caching push notifications from FCM in Unity apps. We override this service with HelpshiftCustomFCMService and handle Helpshift's push notifications.

In the Unity project, we will lower the priority of "ListenerService" and increase the priority of HelpshiftCustomFCMService so that all the push notifications are delivered to "HelpshiftCustomFCMService" instead of "ListenerService" by the android system. If the "origin" is not Helpshift, then we will delegate the notification back to "ListenerService"

## Project content:
* Android studio project that compiles a "HelpshiftCustomFCMService.java" class that extends FCM push service.
* "com.google.firebase.firebase-messaging-unity-6.1.1.aar" file from FCM Unity plugin v6.1.1 copied in "hsfcmplugin/libs" folder. This contains the FCM Unity specific code. We will override the "ListenerService" from this library.
* Helpshift Unity SDK X plugin v10.0.1 "Helpshift.aar" file copied in "hsfcmplugin/libs" folder to resolve **HelpshiftUnity** class. Refer https://github.com/helpshift/helpshift-unity-examples/tree/master/Helpshift-Unity-SDKX-Apps/Helpshift%20Unity%20FCM%20Project#how-to-build-project


## How to build and use this project
* This project is to be used along with [Helpshift Unity FCM Sample Project](https://github.com/helpshift/helpshift-unity-examples/tree/master/Helpshift-Unity-SDKX-Apps/Helpshift%20Unity%20FCM%20Project#helpshift-unity-fcm-sample-project). First, integrate the **Helpshift Unity FCM Sample Project** which contains Helpshift Unity SDK X integration with FCM and FCM push is working fine in the app foreground.
* Open this project, i.e *HelpshiftUnity FCM Background Notification*, in Android studio. Resolve build errors if any.
* Optional steps to modify package name: 
	*	If you want to update package name for "hsfcmplugin" lib than modify the "package" attribute in "hsfcmplugin/src/AndroidManifest.xml" but it should be different to application ID.
	* If you updated the package name than update the same package name in "hsfcmplugin/google-services.json" file
* Build the app via following gradle command : `./gradlew clean build`
* Unpack the generated "hsfcmplugin/build/outputs/aar/hsfcmplugin-release.aar" or "hsfcmplugin/build/outputs/aar/hsfcmplugin.aar" file. Copy and rename the "classes.jar" file to "hsfcmplugin.jar" file.
* Copy this "hsfcmplugin.jar" file to the [Helpshift Unity FCM Sample Project](https://github.com/helpshift/helpshift-unity-examples/tree/master/Helpshift-Unity-SDKX-Apps/Helpshift%20Unity%20FCM%20Project) project in "Assets/Plugins/Android/".
* Notice that we have declared *HelpshiftCustomFCMService* service in "Assets/Plugins/Android/helpshift-fcm-service/AndroidManifest.xml"

```
	<service android:name="com.helpshift.fcmunity.HelpshiftCustomFCMService" android:exported="false">
        <intent-filter android:priority="100">
                    <action android:name="com.google.firebase.MESSAGING_EVENT" />
        </intent-filter>
    </service>
```

* We are increasing the "priority" of our overridden service to get push notifications before FCM's internal service.

* Build the Unity project and test Helpshift's push notification in foreground/background/killed app state.
