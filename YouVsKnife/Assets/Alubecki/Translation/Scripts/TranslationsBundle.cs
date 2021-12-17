/**
 * Alubecki Translations
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;
using UnityEngine;


namespace Alubecki.Tr {

    [Serializable]
    internal struct TranslationsBundle {

        public SystemLanguage language;
        public TextAsset[] assets;

    }

}