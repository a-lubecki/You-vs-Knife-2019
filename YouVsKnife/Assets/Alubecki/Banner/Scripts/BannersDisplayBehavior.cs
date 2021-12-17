/**
 * Alubecki Banner
 * © Aurélien Lubecki 2020
 * All Rights Reserved
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.RemoteConfig;
using Firebase.Extensions;
using GoogleMobileAds.Api;


namespace Alubecki.Banner {

    public class BannersDisplayBehavior : MonoBehaviour, IBannerBehaviorClickListener, IBannerRatingBehaviorListener {


        public static readonly string FIR_KEY_BANNER_TYPE = "bannerType";
        public static readonly string FIR_KEY_BANNER_NUMBER = "bannerNumber";
        public static readonly string STORAGE_KEY_CLICKED_BANNER_TYPE = "alubeckiClickedBannerType";
        public static readonly string STORAGE_KEY_CLICKED_BANNER_NUMBER = "alubeckiClickedBannerNumber";
        public static readonly string STORAGE_KEY_CLICKED_RATING_APP_VERSION = "alubeckiClickedBannerRatingAppVersion";


        public bool isDisplayEnabled = true;
        public bool isBannerGameEnabled = true;
        public bool isBannerRatingEnabled = true;
        public bool isBannerAdsEnabled = true;

        [SerializeField] private BannerGameBehavior bannerGameBehavior = null;
        public BannerType defaultBannerType;
        public int defaultBannerNumber;

        [SerializeField] private BannerRatingBehavior bannerRatingBehavior = null;

        [SerializeField] private BannerAdsConfig bannerAdsConfig = null;
        private BannerView adBannerView;

        public bool canDisplayBannerGame {
            get {
                return bannerGameBehavior != null && isBannerGameEnabled;
            }
        }
        public bool canDisplayBannerRating {
            get {
                return bannerRatingBehavior != null && isBannerRatingEnabled;
            }
        }
        public bool canDisplayBannerAds {
            get {
                return bannerAdsConfig != null && isBannerAdsEnabled;
            }
        }

        private bool isFirebaseInitialized;
        private bool isRemoteConfigInitialized;
        private bool isAdMobInitialized;

        public bool isShowingBannerGame {
            get {
                return bannerGameBehavior != null && bannerGameBehavior.gameObject.activeSelf;
            }
        }
        public bool isShowingBannerRating {
            get {
                return bannerRatingBehavior != null && bannerRatingBehavior.gameObject.activeSelf;
            }
        }
        public bool isShowingBannerAds {
            get {
                return adBannerView != null;
            }
        }

        public GameObject goListener;
        public IBannersDisplayBehaviorListener listener {
            get {
                if (goListener == null) {
                    return null;
                }
                return goListener.GetComponent<IBannersDisplayBehaviorListener>();
            }
        }


        protected void Awake() {

            if (bannerGameBehavior != null) {
                bannerGameBehavior.listener = this;
            }

            if (bannerRatingBehavior != null) {
                bannerRatingBehavior.listener = this;
            }
        }

        protected void Start() {

            hideBannerGame();
            hideBannerRating();
            hideBannerAds();

            initFirebase();
            initAdMob();
        }

        protected void OnApplicationQuit() {

            //free memory
            if (adBannerView != null) {
                adBannerView.Destroy();
                adBannerView = null;
            }
        }

        protected void Update() {

            if (!isFirebaseInitialized) {
                return;
            }
            if (canDisplayBannerGame && !isRemoteConfigInitialized) {
                return;
            }
            if (canDisplayBannerAds && !isAdMobInitialized) {
                return;
            }

            if (!isDisplayEnabled) {
                hideBanners();
                return;
            }

            //dynamic hiding
            if (!canDisplayBannerGame) {
                hideBannerGame();
            }
            if (!canDisplayBannerRating) {
                hideBannerRating();
            }
            if (!canDisplayBannerAds) {
                hideBannerAds();
            }

            //dynamic showing
            bool isShown;

            isShown = tryShowBannerGame();
            if (isShown) {
                return;
            }

            isShown = tryShowBannerRating();
            if (isShown) {
                return;
            }

            isShown = tryShowBannerAds();
            if (isShown) {
                return;
            }

            //add other banners if necessary
        }

        private void initFirebase() {

            //default values if data are not fetched
            var defaultValues = new Dictionary<string, object>();
            defaultValues.Add(FIR_KEY_BANNER_TYPE, defaultBannerType.ToString());
            defaultValues.Add(FIR_KEY_BANNER_NUMBER, (long)defaultBannerNumber);
            FirebaseRemoteConfig.SetDefaults(defaultValues);

            //init firebase
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {

                if (task.IsCanceled || task.IsFaulted) {
                    return;
                }

                var dependencyStatus = task.Result;
                if (dependencyStatus != Firebase.DependencyStatus.Available) {
                    return;
                }

                isFirebaseInitialized = true;

                if (canDisplayBannerGame) {
                    fetchRemoteConfig();
                }
            });
        }

        private void fetchRemoteConfig() {

            //fetch remote config
            FirebaseRemoteConfig.FetchAsync().ContinueWithOnMainThread(task => {

                if (task.IsCanceled || task.IsFaulted) {
                    Debug.LogWarning("Remote config fetching failed :\n" + task.Exception);
                } else {
                    //merge local and remote configs
                    FirebaseRemoteConfig.ActivateFetched();
                }

                isRemoteConfigInitialized = true;
            });
        }

        private void initAdMob() {

            MobileAds.SetiOSAppPauseOnBackground(true);

            MobileAds.Initialize(initStatus => {

                isAdMobInitialized = true;
            });
        }

        private void hideBanners() {

            hideBannerGame();
            hideBannerRating();
            hideBannerAds();
        }

        private bool tryShowBannerGame() {

            if (!canDisplayBannerGame) {
                return false;
            }

            if (isShowingBannerGame) {
                return true;
            }

            //get values from config
            var strBannerType = FirebaseRemoteConfig.GetValue(FIR_KEY_BANNER_TYPE).StringValue;
            var strBannerNumber = FirebaseRemoteConfig.GetValue(FIR_KEY_BANNER_NUMBER).LongValue;

            if (string.IsNullOrEmpty(strBannerType)) {
                //wrong value
                return false;
            }

            //get from save to know if must show
            if (strBannerType.Equals(PlayerPrefs.GetString(STORAGE_KEY_CLICKED_BANNER_TYPE))) {
                //already clicked this banner
                return false;
            }

            bool shown = bannerGameBehavior.showBanner(BannerTypeFunctions.FindBannerType(strBannerType), (int)strBannerNumber);
            if (!shown) {
                //something went wrong
                return false;
            }

            hideBanners();

            bannerGameBehavior?.gameObject.SetActive(true);

            //event for the AB testing start
            FirebaseAnalytics.LogEvent("alubecki_banner_game_show");

            listener?.onBannerGameVisibilityUpdate(true);

            return true;
        }

        private void hideBannerGame() {

            if (!isShowingBannerGame) {
                //already hidden
                return;
            }

            bannerGameBehavior?.hideBanner();
            bannerGameBehavior?.gameObject.SetActive(false);

            listener?.onBannerGameVisibilityUpdate(false);
        }

        void IBannerBehaviorClickListener.onBannerClick(BannerType bannerType, int bannerNumber) {

            hideBannerGame();

            //event for the performance of AB testing
            FirebaseAnalytics.LogEvent("alubecki_banner_game_click");

            //save click to disable display the next lauch
            PlayerPrefs.SetString(STORAGE_KEY_CLICKED_BANNER_TYPE, bannerType.ToString());
            PlayerPrefs.SetInt(STORAGE_KEY_CLICKED_BANNER_NUMBER, bannerNumber);

            listener?.onBannerGameClick();
        }

        private bool tryShowBannerRating() {

            if (!canDisplayBannerRating) {
                return false;
            }

            if (isShowingBannerRating) {
                return true;
            }

            //get from save to know if must show
            if (Application.version.Equals(PlayerPrefs.GetString(STORAGE_KEY_CLICKED_RATING_APP_VERSION))) {
                //player has already clicked this banner for the current app version
                return false;
            }

            hideBanners();

            bannerRatingBehavior.gameObject.SetActive(true);

            FirebaseAnalytics.LogEvent("alubecki_banner_rating_show");

            listener?.onBannerRatingVisibilityUpdate(true);

            return true;
        }

        private void hideBannerRating() {

            if (!isShowingBannerRating) {
                //already hidden
                return;
            }

            bannerRatingBehavior?.gameObject.SetActive(false);

            listener?.onBannerRatingVisibilityUpdate(false);
        }

        void IBannerRatingBehaviorListener.onBannerRatingClick() {

            hideBannerRating();

            FirebaseAnalytics.LogEvent("alubecki_banner_rating_click");

            //save click to disable display the next lauch
            PlayerPrefs.SetString(STORAGE_KEY_CLICKED_RATING_APP_VERSION, Application.version);

            listener?.onBannerRatingClick();

            //open rating screen
            var urlToOpen = bannerRatingBehavior.getStoreUrlToOpen();
            if (urlToOpen != null) {
                Application.OpenURL(urlToOpen);
            }
        }

        private bool tryShowBannerAds() {

            if (!canDisplayBannerAds) {
                return false;
            }

            if (isShowingBannerAds) {
                return true;
            }

            //create the banner
            string adUnitId;

            if (Debug.isDebugBuild) {
                adUnitId = "ca-app-pub-3940256099942544/6300978111";//default banner provided by google
            } else {
#if UNITY_ANDROID
                adUnitId = bannerAdsConfig.getAdMobBannerIdAndroid();
#elif UNITY_IOS
                adUnitId = bannerAdsConfig.getAdMobBannerIdIOS();
#endif
            }

            if (string.IsNullOrEmpty(adUnitId)) {
                //bad id
                return false;
            }

            hideBanners();

            adBannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
            adBannerView.OnAdOpening += onBannerAdsClick;

            adBannerView.LoadAd(bannerAdsConfig.newAdRequest());

            FirebaseAnalytics.LogEvent("alubecki_banner_ads_show");

            listener?.onBannerAdsVisibilityUpdate(true);

            return true;
        }

        private void hideBannerAds() {

            if (!isShowingBannerAds) {
                //already hidden
                return;
            }

            adBannerView.Destroy();
            adBannerView = null;

            listener?.onBannerAdsVisibilityUpdate(false);
        }

        private void onBannerAdsClick(object sender, EventArgs args) {

            FirebaseAnalytics.LogEvent("alubecki_banner_ads_click");

            listener?.onBannerAdsClick();
        }

    }


    public interface IBannersDisplayBehaviorListener {

        void onBannerGameVisibilityUpdate(bool isVisible);
        void onBannerGameClick();
        void onBannerRatingVisibilityUpdate(bool isVisible);
        void onBannerRatingClick();
        void onBannerAdsVisibilityUpdate(bool isVisible);
        void onBannerAdsClick();

    }

}
