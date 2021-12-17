/**
 * Alubecki Banner
 * © Aurélien Lubecki 2020
 * All Rights Reserved
 */

using System;
using UnityEngine;
using UnityEngine.UI;


namespace Alubecki.Banner {

    public class BannerGameBehavior : MonoBehaviour {


        [SerializeField] private BannerInfo[] bannerInfoArray = null;
        public IBannerBehaviorClickListener listener;

        private RawImage touchZone;

        private BannerType currentBannerType;
        private int currentBannerNumber;

        private float scaleMultiplier = 1;


        protected void Awake() {

            touchZone = GetComponent<RawImage>();

            //resize banner for small screens
            resizeBanner();

            hideBanner();
        }

        private void resizeBanner() {

            scaleMultiplier = Screen.width / 1000f;

            if (scaleMultiplier < 0.5f) {
                scaleMultiplier = 0.5f;
            } else if (scaleMultiplier > 1) {
                scaleMultiplier = 1;
            }

            var size = GetComponent<RectTransform>().sizeDelta;
            size.y *= scaleMultiplier;
            GetComponent<RectTransform>().sizeDelta = size;
        }

        private BannerInfo getCurrentBannerInfo() {

            foreach (var info in bannerInfoArray) {

                if (currentBannerType != info.bannerType) {
                    continue;
                }

                return info;
            }

            //not found
            return null;
        }

        public bool showBanner(BannerType bannerType, int bannerNumber) {

            hideBanner();

            currentBannerType = bannerType;
            currentBannerNumber = bannerNumber;

            touchZone.enabled = true;

            try {
                var go = GameObject.Instantiate(
                    getCurrentBannerInfo().prefabs[currentBannerNumber],
                    transform,
                    false
                );

                var scale = go.transform.localScale;
                scale.x *= scaleMultiplier;
                scale.y *= scaleMultiplier;
                go.transform.localScale = scale;

            } catch (Exception e) {
                Debug.LogWarning("The banner couldn't be shown : " + bannerType + " / " + bannerNumber + "\n" + e);
                return false;
            }

            return true;
        }

        public void hideBanner() {

            currentBannerType = BannerType.UNKNOWN;
            currentBannerNumber = 0;

            touchZone.enabled = false;

            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void onBannerClick() {

            var bannerInfo = getCurrentBannerInfo();
            if (bannerInfo == null) {
                return;
            }

            listener?.onBannerClick(currentBannerType, currentBannerNumber);

            Application.OpenURL(bannerInfo.urlToOpen);
        }

    }


    public interface IBannerBehaviorClickListener {

        void onBannerClick(BannerType bannerType, int bannerNumber);

    }

}
