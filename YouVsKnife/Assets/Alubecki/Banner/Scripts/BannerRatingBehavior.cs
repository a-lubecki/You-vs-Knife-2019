/**
 * Alubecki Banner
 * © Aurélien Lubecki 2020
 * All Rights Reserved
 */

using UnityEngine;


namespace Alubecki.Banner {

    public class BannerRatingBehavior : MonoBehaviour {


        public string androidPackage = "TODO"; //ex: com.youvsknife
        public string appStoreId = "TODO"; //ex: 1489040779

        public IBannerRatingBehaviorListener listener = null;


        public void onBannerRatingClick() {

            listener?.onBannerRatingClick();
        }

        public string getStoreUrlToOpen() {

#if UNITY_ANDROID

            if (string.IsNullOrEmpty(androidPackage)) {
                return null;
            }

            return "market://details?id=" + androidPackage;

#elif UNITY_IOS

            if (string.IsNullOrEmpty(appStoreId)) {
                return null;
            }

            return "itms://apps.apple.com/app/id" + appStoreId;

#else
            return null;
#endif
        }

    }


    public interface IBannerRatingBehaviorListener {

        void onBannerRatingClick();

    }

}