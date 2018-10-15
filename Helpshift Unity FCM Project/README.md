# Helpshift Unity FCM Sample Project

A sample project to demonstrate use of FCM Unity plugin with Helpshift Unity SDK
https://developers.helpshift.com/unity/getting-started-ios/

## Project content:
* This project has FCM Unity plugin v5.3.0 already imported. 
* This project has Helpshift Unity plugin 4.0.1 already imported.
* Sample code for using FCM push notifications with Helpshift SDK. Refer : "/Assets/Helpshift/Example/HelpshiftExampleScript.cs"

## How to build project
* Integrate Helpshift Unity Android plugin as documented here : https://developers.helpshift.com/unity/getting-started-android/
* Once the integration is complete then run the plugintoaar script : https://github.com/helpshift/dexter/tree/master/PluginToAAR
	* Make sure to save the config if you are using GUI editor to generate config : https://developers.helpshift.com/unity/getting-started-android/#gui-inspector
	* The script will generate a Helpshift.aar file in "Assets/Plugins/Android/"
* Refer the FCM Unity plugin documentation for integration steps.
* All the Firebase initialization code is in "/Assets/Helpshift/Example/HelpshiftExampleScript.cs"
* Add your google-services.json file to "Assets/Plugins/Android/". Refer : https://firebase.google.com/docs/cloud-messaging/unity/client#set_up_your_app_in_the
* Refer the "/Assets/Helpshift/Example/HelpshiftExampleScript.cs" class for how to delegate FCM push via Helpshift SDK
	* Start()
	* InitializeFirebase()
	* OnTokenReceived()
	* OnMessageReceived()
* Build the app and run for Android platform.
