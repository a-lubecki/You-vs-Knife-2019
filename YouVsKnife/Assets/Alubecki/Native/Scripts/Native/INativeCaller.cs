/**
 * Alubecki Native
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */


namespace Alubecki.Native {

    public interface INativeCaller {

        void showAlertDialog(string callbackGameObjectName, string title, string message, string negativeButtonText, string positiveButtonText);

        void showActionSheetDialog(string callbackGameObjectName, string title, string message, string negativeButtonText, string[] buttonTexts);

        void share(string chooserTitle, string subject, string message, string gameUrl, string imagePath);

        string getAppOrigin();

        string[] getAppSignatures();

    }

}
