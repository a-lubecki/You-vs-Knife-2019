/**
 * Hexa Snap
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;
using GoogleMobileAds.Api;


public class BannerAdsConfig : MonoBehaviour {


    [SerializeField] private string androidBannerId = null;
    [SerializeField] private string iosBannerId = null;

    [SerializeField] private string[] testDeviceIds = null;


    public string getAdMobBannerIdAndroid() {
        return androidBannerId;
    }

    public string getAdMobBannerIdIOS() {
        return iosBannerId;
    }

    public AdRequest newAdRequest() {

        var b = newAdRequestBuilder()
            .AddExtra("npa", "1") //remove ads personalization (GDPR)
            .AddTestDevice(AdRequest.TestDeviceSimulator);

        if (testDeviceIds != null) {

            foreach (var id in testDeviceIds) {
                b.AddTestDevice(id);
            }
        }

        return b.Build();
    }

    protected virtual AdRequest.Builder newAdRequestBuilder() {
        //override if necessary
        return new AdRequest.Builder();
    }

}
