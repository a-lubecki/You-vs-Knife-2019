/**
 * Alubecki Native
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

#if UNITY_IPHONE || UNITY_IOS || UNITY_TVOS

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;


public class EntitlementsPostProcess : ScriptableObject {


    [PostProcessBuild(999)]
    public static void OnPostProcess(BuildTarget buildTarget, string buildPath) {
        
        //write plist file
        string plistPath = Path.Combine(buildPath, "Info.plist");

        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        var text = "The game needs access to the photos to save screenshots.";
        plist.root.SetString("NSPhotoLibraryUsageDescription", text);
        plist.root.SetString("NSPhotoLibraryAddUsageDescription", text);

        File.WriteAllText(plistPath, plist.WriteToString());
    }

}
#endif
