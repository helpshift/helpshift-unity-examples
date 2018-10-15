#if UNITY_IOS
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

using System.Diagnostics;
using System.Collections;
using System.IO;
using Helpshift;
public class HelpshiftPostProcess : MonoBehaviour
{
	// Set PostProcess priority to a high number to ensure that the this is started last.
	[PostProcessBuild(900)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuildProject)
	{
        const string helpshift_plugin_path = "Assets/Helpshift";

        // Current path while executing the script is
        // the project root folder.

		Process myCustomProcess = new Process();
		myCustomProcess.StartInfo.FileName = "python";
		myCustomProcess.StartInfo.Arguments = string.Format(helpshift_plugin_path + "/Editor/HSPostprocessBuildPlayer " + "\"" + pathToBuildProject + "\"" + " " + target + " " + Application.unityVersion);
		myCustomProcess.StartInfo.UseShellExecute = false;
		myCustomProcess.StartInfo.RedirectStandardOutput = false;
		myCustomProcess.Start();
		myCustomProcess.WaitForExit();
		string preprocessorPath = pathToBuildProject + "/Classes/Preprocessor.h";
         	string text = File.ReadAllText(preprocessorPath);
         	text = text.Replace("UNITY_USES_REMOTE_NOTIFICATIONS 0", "UNITY_USES_REMOTE_NOTIFICATIONS 1");
         	File.WriteAllText(preprocessorPath, text);
	}

}
#endif
