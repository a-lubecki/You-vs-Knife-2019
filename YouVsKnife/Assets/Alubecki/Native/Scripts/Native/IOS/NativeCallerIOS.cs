/**
 * Alubecki Native
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

#if UNITY_IOS

using System;
using System.Runtime.InteropServices;


// http://www.theappguruz.com/blog/general-sharing-in-android-ios-in-unity
// https://github.com/ChrisMaire/unity-native-sharing/blob/master/Assets/Plugins/iOS/iOSNativeShare.m


namespace Alubecki.Native {

    public class NativeCallerIOS : INativeCaller {


        [DllImport("__Internal")]
        static extern void call_showAlertDialog(string callbackGameObjectName, string title, string message, string negativeButtonText, string positiveButtonText);

        [DllImport("__Internal")]
        static extern void call_showActionSheetDialog(string callbackGameObjectName, string title, string message, string negativeButtonText, string[] buttonTexts, int buttonTextsCount);

        [DllImport("__Internal")]
        static extern void call_share(string chooserTitle, string subject, string message, string gameUrl, string imagePath);

        [DllImport("__Internal")]
        static extern IntPtr call_getAppOrigin();

        [DllImport("__Internal")]
        static extern IntPtr call_getAppSignatures();


        void INativeCaller.showAlertDialog(string callbackGameObjectName, string title, string message, string negativeButtonText, string positiveButtonText) {

            call_showAlertDialog(callbackGameObjectName, title, message, negativeButtonText, positiveButtonText);
        }

        void INativeCaller.showActionSheetDialog(string callbackGameObjectName, string title, string message, string negativeButtonText, string[] buttonTexts) {

            call_showActionSheetDialog(callbackGameObjectName, title, message, negativeButtonText, buttonTexts, buttonTexts.Length);
        }

        void INativeCaller.share(string chooserTitle, string subject, string message, string gameUrl, string imagePath) {

            call_share(chooserTitle, subject, message, gameUrl, imagePath);
        }

        string INativeCaller.getAppOrigin() {

            return marshalString(call_getAppOrigin());
        }

        string[] INativeCaller.getAppSignatures() {

            string signatures = marshalString(call_getAppSignatures());
            return (signatures != null) ? new string[] { signatures } : new string[0];
        }

        private static string marshalString(IntPtr p) {

            if (p == IntPtr.Zero) {
                return null;
            }

            return Marshal.PtrToStringAnsi(p);
        }

    }

}

#endif
