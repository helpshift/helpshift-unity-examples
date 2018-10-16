# Helpshift Unity FCM Sample Android Project to handle push notifications in background/killed state. 

A sample android studio project that would generate a plain .jar file to demonstrate use of FCM Unity plugin with Helpshift Unity SDK and handle Helpshift push notifications in app background/killed state.

HelpshiftCustomFCMService class extends an internal service, i.e ListenerService, from FCM Unity plugin. This service is responsible for generating/caching push notifications from FCM in Unity apps. We override this service with HelpshiftCustomFCMService and handle Helpshift's push notifications.

In the Unity project, we will lower the priority of "ListenerService" and increase the priority of HelpshiftCustomFCMService so that all the push notifications are delivered to "HelpshiftCustomFCMService" instead of "ListenerService" by the android system. If the "origin" is not Helpshift, then we will delegate the notification back to "ListenerService"

## Project content:
* Android studio project that compiles a "HelpshiftCustomFCMService.java" class that extends FCM push service.
* "com.google.firebase.firebase-messaging-unity-5.2.1.aar" file from FCM Unity plugin v5.3.0 copied in "hsfcmplugin/libs" folder. This contains the FCM Unity specific code. We will override the "ListenerService" from this library.
* Helpshift Unity plugin 4.0.1 converted to "Helpshift.aar" via the PluginToAAR script copied in "hsfcmplugin/libs" folder. Refer https://github.com/helpshift/helpshift-unity-examples/tree/master/Helpshift%20Unity%20FCM%20Project#how-to-build-project


## How to build and use this project
* This project is to be used along with [Helpshift Unity FCM Sample Project](https://github.com/helpshift/helpshift-unity-examples/tree/master/Helpshift%20Unity%20FCM%20Project)
* Integrate Helpshift Unity Android plugin in your Unity project as documented here : https://developers.helpshift.com/unity/getting-started-android/
* Once the integration is complete then run the plugintoaar script : https://github.com/helpshift/dexter/tree/master/PluginToAAR
	* Make sure to save the config if you are using GUI editor to generate config : https://developers.helpshift.com/unity/getting-started-android/#gui-inspector
	* The script will generate a Helpshift.aar file in "Assets/Plugins/Android/"
* Refer the [Helpshift Unity FCM Sample Project](https://github.com/helpshift/helpshift-unity-examples/tree/master/Helpshift%20Unity%20FCM%20Project) for integrating FCM Unity plugin.
* Open this project, i.e *HelpshiftUnity FCM Background Notification*, in Android studio. Resolve build errors if any.
* Update the "package" attribute in "hsfcmplugin/src/AndroidManifest.xml" to your app Id. (The app Id as used for generating FCM keys.)
* Add the google-services.json file to "hsfcmplugin/google-services.json". Refer : https://firebase.google.com/docs/cloud-messaging/unity/client#set_up_your_app_in_the
* The package attribute in "hsfcmplugin/src/AndroidManifest.xml" should match the "google-services.json" app name.
* Build the app via gradle.
* Unpack the generated "hsfcmplugin/build/outputs/aar/hsfcmplugin-release.aar" file. Copy and rename the "classes.jar" file to "hsfcmplugin.jar" file.
* Copy this "hsfcmplugin.jar" file to the [Helpshift Unity FCM Sample Project](https://github.com/helpshift/helpshift-unity-examples/tree/master/Helpshift%20Unity%20FCM%20Project) project in "Assets/Plugins/Android/".
* Declare the "HelpshiftCustomFCMService" in "Assets/Plugins/Android/AndroidManifest.xml"


	<service android:name="com.helpshift.fcmunity.HelpshiftCustomFCMService" android:exported="false">
        <intent-filter android:priority="100">
                    <action android:name="com.google.firebase.MESSAGING_EVENT" />
        </intent-filter>
    </service>

* We increase the "priority" of our overridden service to get push notifications before FCM's internal service.

* Build the Unity project and test Helpshift's push notification in foreground/background/killed app state.
