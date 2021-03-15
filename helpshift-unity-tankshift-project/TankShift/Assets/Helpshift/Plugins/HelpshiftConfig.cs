using UnityEngine;
using System;
using System.IO;
#if UNITY_IOS || UNITY_ANDROID
using Helpshift;
#endif
using System.Collections.Generic;
using HSMiniJSON;

#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
#endif
[System.Serializable]
public class HelpshiftConfig : ScriptableObject
{
    private static HelpshiftConfig instance;
    private const string helpshiftConfigAssetName = "HelpshiftConfig";
    private const string helpshiftConfigPath = "Helpshift/Resources";
    public const String pluginVersion = "5.5.3";

    public static HelpshiftConfig Instance
    {
        get
        {
            instance = Resources.Load(helpshiftConfigAssetName) as HelpshiftConfig;
            if (instance == null)
            {
                instance = CreateInstance<HelpshiftConfig>();
#if UNITY_EDITOR
                string properPath = Path.Combine(Application.dataPath, helpshiftConfigPath);
                if (!Directory.Exists(properPath))
                {
                    AssetDatabase.CreateFolder("Assets/Helpshift", "Resources");
                }

                string fullPath = Path.Combine(Path.Combine("Assets", helpshiftConfigPath),
                                                               helpshiftConfigAssetName + ".asset"
                );
                AssetDatabase.CreateAsset(instance, fullPath);
#endif
            }
            return instance;
        }
    }

#if UNITY_EDITOR
    [MenuItem("Helpshift/Developers Page")]
    public static void OpenAppPage()
    {
        string url = "https://developers.helpshift.com/unity/";
        Application.OpenURL(url);
    }

    [MenuItem("Helpshift/SDK Twitter handler")]
    public static void OpenFacebookGroup()
    {
        string url = "https://twitter.com/helpshiftsdk";
        Application.OpenURL(url);
    }

    [MenuItem("Helpshift/Report an SDK Bug")]
    public static void ReportABug()
    {
        string url = "mailto:support@helpshift.com";
        Application.OpenURL(url);
    }
#endif
}
