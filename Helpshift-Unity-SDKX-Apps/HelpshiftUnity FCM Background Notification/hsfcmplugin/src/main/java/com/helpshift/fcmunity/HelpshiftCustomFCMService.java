package com.helpshift.fcmunity;

import android.util.Log;

import com.google.firebase.messaging.RemoteMessage;
import com.google.firebase.messaging.cpp.ListenerService;
import com.helpshift.unityproxy.HelpshiftUnity;

import java.util.Map;

// Extend ListenerService which is a FCM internal service specifically written to handle Unity platform.
// By extending this service we ensure to keep the FCM plugin functionality as it is and only override in case of Helpshift notifications.
// We will reduce the "priority" of ListenerService in Unity project and keep the priority higher for HelpshiftCustomFCMService
public class HelpshiftCustomFCMService extends ListenerService {
    private static final String TAG = "Helpshift Native";

    @Override
    public void onMessageReceived(RemoteMessage message) {
        String from = message.getFrom();
        Map<String, String> data = message.getData();
        Log.d(TAG, "onMessageReceived, from: " + from + ", data: " + data);
        String origin = data.get("origin");
        if (origin != null && origin.equals("helpshift")) {
            Log.d(TAG, "Helpshift Notification received");
            // Handle push with native Helpshift java apis directly.
            HelpshiftUnity.handlePush(this, data);
        } else {
            Log.d(TAG, "Notification from non-helpshift service");
            // Call super to forward the notification to the default FCM service.
            super.onMessageReceived(message);
        }
    }
}