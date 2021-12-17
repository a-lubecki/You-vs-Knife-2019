/**
 * Alubecki Native
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */


namespace Alubecki.Native {

    public class NativeCallsManager : INativeCaller {


        //singleton
        public static readonly NativeCallsManager instance = new NativeCallsManager();


        private INativeCaller caller;


        private NativeCallsManager() {

#if UNITY_ANDROID
        caller = new NativeCallerAndroid();
#elif UNITY_IOS
        caller = new NativeCallerIOS();
#endif
        }

        public void showAlertDialog(string callbackGameObjectName, string title, string message, string negativeButtonText, string positiveButtonText) {
            caller?.showAlertDialog(callbackGameObjectName, title, message, negativeButtonText, positiveButtonText);
        }

        public void showActionSheetDialog(string callbackGameObjectName, string title, string message, string negativeButtonText, string[] buttonTexts) {
            caller?.showActionSheetDialog(callbackGameObjectName, title, message, negativeButtonText, buttonTexts);
        }

        public void share(string chooserTitle, string subject, string message, string gameUrl, string imagePath) {
            caller?.share(chooserTitle, subject, message, gameUrl, imagePath);
        }

        public string getAppOrigin() {
            return caller?.getAppOrigin();
        }

        public string[] getAppSignatures() {
            return caller?.getAppSignatures();
        }

    }

}