/**
 * Alubecki Banner
 * © Aurélien Lubecki 2020
 * All Rights Reserved
 */

using System;
using UnityEngine;


namespace Alubecki.Banner {


    [Serializable]
    public class BannerInfo {

        public BannerType bannerType;
        public String urlToOpen;
        public GameObject[] prefabs;

    }

}
