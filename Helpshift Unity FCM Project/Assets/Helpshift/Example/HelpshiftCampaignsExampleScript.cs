using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;
using Helpshift.Campaigns;

#if UNITY_IOS || UNITY_ANDROID
using Helpshift;
using HSMiniJSON;
#endif

public class HelpshiftCampaignsExampleScript : MonoBehaviour
{

#if UNITY_IOS || UNITY_ANDROID
    private HelpshiftSdk _support;
    private HelpshiftCampaigns _campaigns;
    private HelpshiftInbox _inbox;

    // Use this for initialization

    void Start () {
        _support = HelpshiftSdk.getInstance();
        _campaigns = HelpshiftCampaigns.getInstance ();
        _inbox = HelpshiftInbox.getInstance ();
        _support.install();
        //_support.registerForPush("<gcm-key>");
        _campaigns.SetInboxMessagesDelegate (new HelpshiftExample.InboxDelegate ());
        _inbox.SetInboxMessageDelegate (new HelpshiftExample.InboxDelegate ());
        _inbox.SetInboxPushNotificationDelegate (new HelpshiftExample.InboxPushNotificationDelegate ());
		_campaigns.SetCampaignsDelegate (new HelpshiftExample.CampaignsDelegate ());

		_campaigns.RequestUnreadMessagesCount ();
    }

    public void onAddPropertiesClick() {
        Dictionary<string,object> propertyDict = new Dictionary<string,object> ();
        propertyDict.Add ("DTestBoolKey", true);
        propertyDict.Add ("DTestIntKey", 10);
        propertyDict.Add ("DTestStringKey", "Helpshift");
        propertyDict.Add ("DTestDateKey", DateTime.Now);
		propertyDict.Add ("DTestLongKey", 7890342412332322231L);

        string[] result = _campaigns.AddProperties(propertyDict);
        foreach(var item in result)
            {
                Debug.Log ("Result is : " + item.ToString());
            }
    }

    public void onAddPropertyIntegerClick() {
        GameObject inputFieldGo = GameObject.FindGameObjectWithTag("property_int");
        InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
        try
            {
                bool result = _campaigns.AddProperty("TestIntKey", Convert.ToInt32(inputFieldCo.text));
                Debug.Log ("Result is : " + result.ToString());
                Debug.Log ("Property added : " + inputFieldCo.text);
            }
        catch (FormatException e)
            {
                Debug.Log("Input string is not valid : " + e);
            }
    }

	
	public void onAddPropertyLongClick() {
		GameObject inputFieldGo = GameObject.FindGameObjectWithTag("property_long");
		InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
		try
		{
			bool result = _campaigns.AddProperty("TestLongKey", Convert.ToInt64(inputFieldCo.text));
			Debug.Log ("Result is : " + result.ToString());
			Debug.Log ("Property added : " + inputFieldCo.text);
		}
		catch (FormatException e)
		{
			Debug.Log("Input string is not valid : " + e);
		}
	}

	
	public void onAddPropertyDateClick() {
		bool result = _campaigns.AddProperty("TestDateKey", DateTime.Now);
		Debug.Log ("Result is : " + result.ToString());
    }

    public void onAddPropertyBooleanClick() {
        GameObject inputFieldGo = GameObject.FindGameObjectWithTag("property_bool");
        InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
        try
            {
                bool result = _campaigns.AddProperty("TestBoolKey",Convert.ToBoolean(inputFieldCo.text));
                Debug.Log ("Result is : " + result.ToString());
                Debug.Log ("Property added : " + inputFieldCo.text);
            }
        catch (FormatException e)
            {
                Debug.Log("Input string is not valid : " + e);
            }
    }

    public void onAddPropertyStringClick() {
        GameObject inputFieldGo = GameObject.FindGameObjectWithTag("property_string");
        InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
        try
            {
                bool result = _campaigns.AddProperty("TestStringKey", Convert.ToString(inputFieldCo.text));
                Debug.Log ("Result is : " + result.ToString());
                Debug.Log ("Property added : " + inputFieldCo.text);
            }
        catch (FormatException e)
            {
                Debug.Log("Input string is not valid : " + e);
            }
    }

    public void onShowInboxClicked() {
        Dictionary<string,object> config = new Dictionary<string,object> ();
        _campaigns.ShowInbox(config);
    }

	public void onLogInboxDataClicked ()
	{
		List<HelpshiftInboxMessage> messages = _inbox.GetAllInboxMessages ();
		
		if (messages == null || messages.Count == 0) {
			Debug.Log ("No inbox messages");
			return;
		}
		
		String messageCountStr = "Message count : " + messages.Count;
		Debug.Log ("Message count: " + messageCountStr);
		
		foreach (HelpshiftInboxMessage message in messages) {
			String messageStr = "\n " + message.ToString ();
			messageStr += " \n CoverImage path : " + message.GetCoverImageFilePath ();
			messageStr += " \n IconImage path : " + message.GetIconImageFilePath ();
			int actionCount = message.GetCountOfActions ();
			
			for (int i = 0; i < actionCount; i++) {
				String actionsData = "\n Action Data : " + i;
				actionsData += "\n \t Title : " + message.GetActionTitle (i);
				actionsData += "\t  Data : " + message.GetActionData (i);
				actionsData += "\n \t  Color : " + message.GetActionTitleColor (i);
				actionsData += "\t  Type : " + message.GetActionType (i);
				actionsData += "\t  Is Goal completion : " + message.IsActionGoalCompletion (i);
				messageStr += actionsData;
			}
			Debug.Log ("Inbox Data : " + messageStr);
		}
	}

    public void Login() {
        _support.login ("user-1", "user.1", "user1@helpshift.com");
    }

    public void Logout() {
        _support.logout ();
    }

    public void onBackToSupportClick() {
        Application.LoadLevel(0);
    }
#endif
}
