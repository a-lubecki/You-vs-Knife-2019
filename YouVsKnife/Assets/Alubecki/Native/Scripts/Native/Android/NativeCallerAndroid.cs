/**
 * Alubecki Native
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

#if UNITY_ANDROID

using UnityEngine;


// http://www.theappguruz.com/blog/android-native-popup-using-unity
// http://www.theappguruz.com/blog/ios-native-popup-using-unity

// http://eppz.eu/blog/unity-android-plugin-tutorial-3/


namespace Alubecki.Native {

    public class NativeCallerAndroid : INativeCaller {


        static readonly string ANDROID_PROJECT_PACKAGE = "com.alubecki.nativeunity";


        void INativeCaller.showAlertDialog(string callbackGameObjectName, string title, string message, string negativeButtonText, string positiveButtonText) {

            callAndroidStatic(
                new AndroidStaticMethod(ANDROID_PROJECT_PACKAGE + ".NativePopupManager", "showAlertDialog"),
                callbackGameObjectName, title, message, negativeButtonText, positiveButtonText);
        }

        void INativeCaller.showActionSheetDialog(string callbackGameObjectName, string title, string message, string negativeButtonText, string[] buttonTexts) {

            callAndroidStatic(
                new AndroidStaticMethod(ANDROID_PROJECT_PACKAGE + ".NativePopupManager", "showActionSheetDialog"),
                callbackGameObjectName, title, message, negativeButtonText, buttonTexts);
        }

        void INativeCaller.share(string chooserTitle, string subject, string message, string gameUrl, string imagePath) {

            callAndroidStatic(new AndroidStaticMethod(ANDROID_PROJECT_PACKAGE + ".NativeShareManager", "share"),
                chooserTitle, subject, message, gameUrl, imagePath);
        }

        string INativeCaller.getAppOrigin() {

            return callAndroidStaticSynchronous<string>(new AndroidStaticMethod(ANDROID_PROJECT_PACKAGE + ".NativeUtils", "getAppOrigin"));
        }

        string[] INativeCaller.getAppSignatures() {

            return callAndroidStaticSynchronous<string[]>(new AndroidStaticMethod(ANDROID_PROJECT_PACKAGE + ".NativeUtils", "getAppSignatures"));
        }

        static ReturnType callAndroidStaticSynchronous<ReturnType>(AndroidStaticMethod method, params object[] args) {

            try {

                AndroidJavaObject bridge = new AndroidJavaObject(method.className);

                return bridge.CallStatic<ReturnType>(method.methodName, args);

            } catch (System.Exception ex) {
                Debug.LogWarning(ex.Message);
            }

            return default;
        }

        static void callAndroidStatic(AndroidStaticMethod method, params object[] args) {

            try {

                AndroidJavaObject bridge = new AndroidJavaObject(method.className);

                AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

                currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                    bridge.CallStatic(method.methodName, args);
                }));

            } catch (System.Exception ex) {
                Debug.LogWarning("Native call to "+ method.className + "::" + method.methodName + " failed " + ex.Message);
            }
        }
    }


    struct AndroidStaticMethod {

        public readonly string className;
        public readonly string methodName;

        public AndroidStaticMethod(string className, string methodName) {
            this.className = className;
            this.methodName = methodName;
        }

    }

}

#endif
