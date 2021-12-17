/**
 * Alubecki Native
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;
using System.Collections;
using System.IO;
using UnityEngine;


namespace Alubecki.Native {

    public class NativePopupManager : MonoBehaviour {


        private static NativePopupManager currentInstance;

        public static NativePopupManager instance {
            get {
                if (currentInstance == null) {
                    throw new InvalidOperationException("The NativePopupManager is not init or has been destroyed");
                }

                return currentInstance;
            }
        }


        [SerializeField] private GameObject goNativePopupManagerListener = null;
        public INativePopupManagerListener listener {
            get {
                if (goNativePopupManagerListener == null) {
                    return null;
                }
                return goNativePopupManagerListener.GetComponent<INativePopupManagerListener>();
            }
        }

        private Action cancelAction;
        private Action[] actions;

        [SerializeField] private string sharedScreenshotName = "shared-image.png";
        private string sharedScreenshotPath {
            get {
                return Application.persistentDataPath + "/" + sharedScreenshotName;
            }
        }


        protected void Awake() {
            DontDestroyOnLoad(gameObject);
            currentInstance = this;
        }

        protected void OnDestroy() {
            currentInstance = null;
        }

        public void show(string title, string message, string cancelMessage, Action cancelAction) {

            show(title, message, cancelMessage, cancelAction, null, (Action[])null);
        }

        public void show(string title, string message, string cancelMessage, Action cancelAction, string buttonMessage, Action action) {

            show(title, message, cancelMessage, cancelAction, new string[] { buttonMessage }, new Action[] { action });
        }

        public void show(string title, string message, string cancelMessage, Action cancelAction, string[] buttonsMessage, Action[] actions) {

            if (buttonsMessage == null) {

                if (actions != null && actions.Length > 0) {
                    throw new ArgumentException();
                }

            } else if (actions == null) {

                if (buttonsMessage != null && buttonsMessage.Length > 0) {
                    throw new ArgumentException();
                }

            } else if (actions.Length != buttonsMessage.Length) {
                throw new ArgumentException();
            }

            if (cancelMessage == null) {
                cancelMessage = listener?.getDefaultCancelMessage(this);
            }

            this.cancelAction = cancelAction;
            this.actions = actions;

            //notify if need to pause the game for example
            listener?.onPopupShown(this);

            if (actions == null || actions.Length <= 1) {

                string button = null;
                if (buttonsMessage != null && buttonsMessage.Length == 1) {
                    button = buttonsMessage[0];
                }

                NativeCallsManager.instance.showAlertDialog(name, title, message, cancelMessage, button);

            } else {

                NativeCallsManager.instance.showActionSheetDialog(name, title, message, cancelMessage, buttonsMessage);
            }
        }

        public void onPopupButtonClick(string buttonId) {

            //callback called from native code
            Action currentAction = null;

            int pos = int.Parse(buttonId);
            if (pos < 0) {

                currentAction = cancelAction;

            } else {

                if (actions != null) {
                    currentAction = actions[pos];
                }
            }

            //free actions array for memory management before calling the delegate
            actions = null;
            cancelAction = null;

            //call delegate
            currentAction?.Invoke();
        }

        public void shareText(string chooserTitle, string subject, string message, string gameUrl) {

            //notify if need to pause the game for example
            listener?.onPopupShown(this);

            NativeCallsManager.instance.share(
                chooserTitle,
                subject,
                message,
                gameUrl,
                sharedScreenshotPath
            );
        }

        public void shareScreenshot(string chooserTitle, string subject, string message, string gameUrl, Action completion) {

            //erase the last screenshot
            try {
                if (File.Exists(sharedScreenshotPath)) {
                    File.Delete(sharedScreenshotPath);
                }

            } catch (Exception e) {
                Debug.LogWarning(e);
            }

            //asynchronous capture
            StartCoroutine(captureScreenshot(() => {

                //notify if need to pause the game for example
                listener?.onPopupShown(this);

                completion?.Invoke();

                NativeCallsManager.instance.share(
                    chooserTitle,
                    subject,
                    message,
                    gameUrl,
                    sharedScreenshotPath
                );
            }));
        }

        private IEnumerator captureScreenshot(Action completion) {

            yield return new WaitForEndOfFrame();

            Texture2D screenImage = new Texture2D(Screen.width, Screen.height);

            //Get Image from screen
            screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            screenImage.Apply();

            //Save image to file
            File.WriteAllBytes(sharedScreenshotPath, screenImage.EncodeToPNG());

            completion();
        }

    }

    public interface INativePopupManagerListener {

        string getDefaultCancelMessage(NativePopupManager manager);

        void onPopupShown(NativePopupManager manager);

    }

}