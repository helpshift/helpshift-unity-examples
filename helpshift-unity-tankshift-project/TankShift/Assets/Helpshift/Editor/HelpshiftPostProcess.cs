#if UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using HsUnityEditor.iOS.Xcode;
using HsUnityEditor.iOS.Xcode.Extensions;
public class HelpshiftPostProcess : MonoBehaviour
{
    // Set PostProcess priority to a high number to ensure that the this is started last.
    [PostProcessBuild(900)]
    public static void OnPostprocessBuild(BuildTarget target, string path)
    {
        // Don't do anything if build target is not iOS
        if (target != BuildTarget.iOS)
        {
            return;
        }

        // Create a PBXProject object for the generated Xcode project
        PBXProject project = new PBXProject();
        string pbxProjectPath = PBXProject.GetPBXProjectPath(path);
        project.ReadFromFile(pbxProjectPath);

        // GUID of main app target. This is the target whose build settings we will be modifying.
        string unityAppTargetGuid = project.TargetGuidByName("Unity-iPhone");
        // GUID of UnityFramework target. This target is only present in Unity 2019.3 and above.
        // We need to add our app controller header file to this target, if present. Else we add
        // it to app target.
        string unityFrameworkTargetGuid = project.TargetGuidByName("UnityFramework");

        // Add Helpshift-Unity.h header file to project, if not already added.
        string unityHeaderProjectPath = "Libraries/Helpshift/Plugins/iOS/Helpshift-Unity.h";
        string unityHeaderGuid = project.FindFileGuidByProjectPath(unityHeaderProjectPath);
        if (unityHeaderGuid == null)
        {
            string unityHeaderDiskPath = Application.dataPath + "/Helpshift/Plugins/iOS/Helpshift-Unity.h";
            project.AddFile(unityHeaderDiskPath, unityHeaderProjectPath, PBXSourceTree.Absolute);
        }

        // Add HsUnityAppController.mm file to project, if not already added.
        string unityAppControllerProjectPath = "Libraries/Helpshift/Plugins/iOS/HsUnityAppController.mm";
        string unityAppControllerGuid = project.FindFileGuidByProjectPath(unityHeaderProjectPath);
        if (unityAppControllerGuid == null)
        {
            string unityAppControllerDiskPath = Application.dataPath + "/Helpshift/Plugins/iOS/HsUnityAppController.mm";
            unityAppControllerGuid = project.AddFile(unityAppControllerDiskPath, unityAppControllerProjectPath, PBXSourceTree.Absolute);
            project.AddFileToBuild(unityFrameworkTargetGuid ?? unityAppTargetGuid, unityAppControllerGuid);
        }

        // Add Helpshift.framework to project, if not already added.
        string frameworkProjectPath = "Frameworks/Helpshift/Plugins/iOS/Helpshift.framework";
        string frameworkGuid = project.FindFileGuidByProjectPath(frameworkProjectPath);
        if (frameworkGuid == null)
        {
            // Normal Helpshift.framework not added in project. Check if bitcode framework is added.
            frameworkProjectPath = "Frameworks/Helpshift/Plugins/iOS/Bitcode/Helpshift.framework";
            frameworkGuid = project.FindFileGuidByProjectPath(frameworkProjectPath);
        }
        if (frameworkGuid == null)
        {
            string frameworkDiskPath = Application.dataPath + "/Helpshift/Plugins/iOS/Helpshift.framework";
            if (!Directory.Exists(frameworkDiskPath))
            {
                // Normal framework not found on disk. Check if bitcode framework is found.
                frameworkDiskPath = Application.dataPath + "/Helpshift/Plugins/iOS/Bitcode/Helpshift.framework";
            }
            frameworkGuid = project.AddFile(frameworkDiskPath, frameworkProjectPath, PBXSourceTree.Absolute);
            project.AddFileToBuild(unityFrameworkTargetGuid ?? unityAppTargetGuid, frameworkGuid);
            // If framework is added to project via this script, we also need to set the Framework Search Path to path of framework on disk.
            project.AddBuildProperty(unityFrameworkTargetGuid ?? unityAppTargetGuid, "FRAMEWORK_SEARCH_PATHS", frameworkDiskPath.Replace("/Helpshift.framework", ""));
        }

        // Embed Helpshift.framework into main app target
        PBXProjectExtensions.AddFileToEmbedFrameworks(project, unityAppTargetGuid, frameworkGuid);

        // Add script phase to strip simulator slices from Helpshift.framework in app target
        if (!project.isShellScriptAdded(unityAppTargetGuid, "HS Strip Simulator Slices"))
        {
            project.AddShellScriptBuildPhase(unityAppTargetGuid,
            "HS Strip Simulator Slices",
            "/bin/sh",
            "bash \"${BUILT_PRODUCTS_DIR}/${FRAMEWORKS_FOLDER_PATH}/Helpshift.framework/strip_frameworks.sh\"");
        }

        // Set Always Embed Swift Standard Libraries to YES for app target
        project.SetBuildProperty(unityAppTargetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

        // Add @executable_path/Frameworks to Runpath Search Paths for app target
        if (!project.IsBuildPropertyAdded(unityAppTargetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks"))
        {
            project.AddBuildProperty(unityAppTargetGuid, "LD_RUNPATH_SEARCH_PATHS", "@executable_path/Frameworks");
        }

        /**
         * Uncomment this line if you want to add custom themes for Helpshift SDK
         * Refer https://developers.helpshift.com/unity/design-ios/ to know more
         */
        // Add Helpshift theming plist files to main app target
        // HelpshiftPostProcess.AddHelpshiftCustomThemes(project);

        // Save modified Xcode project
        project.WriteToFile(pbxProjectPath);

        // Enable remote notifications
        string preprocessorPath = path + "/Classes/Preprocessor.h";
        string text = File.ReadAllText(preprocessorPath);
        text = text.Replace("UNITY_USES_REMOTE_NOTIFICATIONS 0", "UNITY_USES_REMOTE_NOTIFICATIONS 1");
        File.WriteAllText(preprocessorPath, text);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality",
        "IDE0051:Remove unused private members",
        Justification = "Call to this method can be uncommented in OnPostprocessBuild() method")]
    private static void AddHelpshiftCustomThemes(PBXProject project)
    {
        // Relative path to HelpshiftCustomThemes group in Xcode project. The group doesn't exist yet.
        // It will be created when we call project.AddFile()
        string customThemesFolderProjectPath = "Libraries/Helpshift/Plugins/iOS/HelpshiftCustomThemes";

        // Absolute path to HelpshiftCustomThemes folder
        // Application.dataPath gives $(PROJECT_ROOT)/Assets. We append the rest of the path
        string customThemesFolderDiskPath = Application.dataPath + "/Helpshift/Plugins/iOS/HelpshiftCustomThemes";

        // For each plist file in HelpshiftCustomThemes folder outside the project
        // First add the plist file to project's HelpshiftCustomThemes group
        // Then add the file guid returned in first step to main target's build phase
        DirectoryInfo dirInfo = new DirectoryInfo(customThemesFolderDiskPath);
        foreach (FileInfo plistInfo in dirInfo.GetFiles("*.plist"))
        {
            string guid = project.AddFile(plistInfo.FullName, customThemesFolderProjectPath + "/" + plistInfo.Name);
            project.AddFileToBuild(project.TargetGuidByName("Unity-iPhone"), guid);
        }
    }
}
#endif
