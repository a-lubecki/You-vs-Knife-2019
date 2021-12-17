/**
 * Alubecki Banner
 * © Aurélien Lubecki 2020
 * All Rights Reserved
 */

using System;


namespace Alubecki.Banner {

    public enum BannerType {

        UNKNOWN,
        HEXA_SNAP,
        YOU_VS_KNIFE,

    }

    public static class BannerTypeFunctions {

        public static BannerType FindBannerType(string tag) {

            foreach (var bannerType in (BannerType[])Enum.GetValues(typeof(BannerType))) {

                if (bannerType.ToString().Equals(tag)) {
                    //found
                    return bannerType;
                }
            }

            //not found
            return BannerType.UNKNOWN;
        }

    }

}
