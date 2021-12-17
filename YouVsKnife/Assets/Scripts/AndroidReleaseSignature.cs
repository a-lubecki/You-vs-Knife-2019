/**
 * Hexa Snap
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System;
using System.IO;


[InitializeOnLoad]
public class AndroidReleaseSignature {


    static AndroidReleaseSignature() {

        SetAndroidSignature();
    }


    private static readonly string PATH_ANDROID_BUILD_TXT = "XXXXXXXXXXXXXX";


    public static void SetAndroidSignature() {

        if (!File.Exists(PATH_ANDROID_BUILD_TXT)) {

            EditorUtility.DisplayDialog(
                "Missing Build Config",
                "In the project folder create " + PATH_ANDROID_BUILD_TXT + " then inside write 2 lines:lines\n1. The keystore password\n2. The key password",
                "OK"
            );
            return;
        }

        StreamReader configReader = new StreamReader(PATH_ANDROID_BUILD_TXT);
        string storePassword = configReader.ReadLine();
        string keyPassword = configReader.ReadLine();
        configReader.Close();

        PlayerSettings.keystorePass = storePassword;
        PlayerSettings.keyaliasPass = keyPassword;
    }

}

#endif