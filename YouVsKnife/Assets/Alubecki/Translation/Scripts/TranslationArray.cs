/**
 * Alubecki Translations
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;
using System.Collections.Generic;
using UnityEngine;


namespace Alubecki.Tr {

    public sealed class TranslationsArray {


        public SystemLanguage language { get; private set; }

        private Dictionary<string, string[]> translations = new Dictionary<string, string[]>();


        public TranslationsArray(SystemLanguage language, Dictionary<string, string[]> translations) {

            this.language = language;
            this.translations = translations ?? throw new ArgumentException();
        }


        public string[] getTranslationArray(string key) {

            if (!translations.ContainsKey(key)) {
                throw new ArgumentException("Translation \"" + key + "\" not found for language " + language);
            }

            return translations[key];
        }

        public string[] getTranslationArray(string key, int pos, int nb) {

            string[] res = new string[nb];
            Array.Copy(getTranslationArray(key), pos, res, 0, nb);

            return res;
        }

        public string getTranslation(string key, int pos) {

            return getTranslationArray(key)[pos];
        }

        public string getTranslation(string key) {

            return getTranslationArray(key)[0];
        }

    }

}