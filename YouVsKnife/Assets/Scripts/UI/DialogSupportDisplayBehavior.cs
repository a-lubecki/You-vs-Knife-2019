/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;
using Alubecki.Banner;


public class DialogSupportDisplayBehavior : MonoBehaviour, IBannersDisplayBehaviorListener {


    [SerializeField] private GameObject goDialog = null;


    void IBannersDisplayBehaviorListener.onBannerGameVisibilityUpdate(bool isVisible) {
        //do nothing
    }

    void IBannersDisplayBehaviorListener.onBannerGameClick() {
        //do nothing
    }

    void IBannersDisplayBehaviorListener.onBannerRatingVisibilityUpdate(bool isVisible) {
        //do nothing
    }

    void IBannersDisplayBehaviorListener.onBannerRatingClick() {

        //show thank dialog after rating
        goDialog.SetActive(true);
    }

    void IBannersDisplayBehaviorListener.onBannerAdsVisibilityUpdate(bool isVisible) {
        //do nothing
    }

    void IBannersDisplayBehaviorListener.onBannerAdsClick() {
        //do nothing
    }

}
