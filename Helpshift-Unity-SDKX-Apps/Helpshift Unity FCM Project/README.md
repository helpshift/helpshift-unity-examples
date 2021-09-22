# Helpshift Unity FCM Sample Project

A sample project to demonstrate use of FCM Unity plugin with Helpshift Unity SDK
https://developers.helpshift.com/sdkx-unity/getting-started-android/

## Project content:
* This project is created with Unity 2018.3.0f2 IDE version.
* This project has Helpshift Unity SDKX 10.0.1 plugin which imported via following steps: https://developers.helpshift.com/sdkx-unity/getting-started-android/
* This project has FCM Messaging Unity plugin v6.1.1 which imported via the following steps: https://firebase.google.com/docs/cloud-messaging/unity/client
* Sample code to initialize FCM push notifications with Helpshift SDK is included in HelpshiftExampleScript.cs file.
* Refer the "/Assets/Helpshift/Example/HelpshiftExampleScript.cs" class for how to delegate FCM push via Helpshift SDK
	* Start()
	* InitializeFirebase()
	* OnTokenReceived()
	* OnMessageReceived()

## How to build project
* In **Awake()** method of **HelpshiftExampleScript.cs** file, copy the install credentials from Helpshift dashboard and update the `apiKey`, `domainName` and `appId` which is used in Helpshift install API. For more info refer the initializing step: https://developers.helpshift.com/sdkx-unity/getting-started-android/#initializing 
* Add your google-services.json file to "Assets/Plugins/Android/". Refer : https://firebase.google.com/docs/cloud-messaging/unity/client#add_config_file
* Add example scene to the app "/Assets/Helpshift/Example/HelpshiftExample.unity" and attach "Assets/Helpshift/Example/HelpshiftExampleScript.cs" script to the "background_image" object
* Switch build system for Android build to "Gradle". Build the app and run for Android platform.