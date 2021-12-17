/**
 * Alubecki Translations
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace Alubecki.Tr {

    public sealed class TranslationsManager {


        //singleton
        public static readonly TranslationsManager instance = new TranslationsManager();


        public SystemLanguage currentLanguage { get; private set; }

        private Dictionary<SystemLanguage, TranslationsArray> translationArrays;


        public void init(SystemLanguage defaultLanguage, Dictionary<SystemLanguage, TranslationsArray> translationArrays) {

            this.translationArrays = translationArrays ?? throw new ArgumentException();

            updateCurrentLanguage(defaultLanguage);
        }

        private void assertTranslationsNotNull() {

            if (translationArrays == null) {
                throw new InvalidOperationException("You must init the TranslationsManager before selecting the language");
            }
        }

        public bool hasLanguage(SystemLanguage language) {

            assertTranslationsNotNull();

            return translationArrays.ContainsKey(language);
        }

        public SystemLanguage[] getLanguages() {

            assertTranslationsNotNull();

            return translationArrays.Keys.ToArray();
        }

        public void updateCurrentLanguage(SystemLanguage language) {

            assertTranslationsNotNull();

            if (!hasLanguage(language)) {
                throw new InvalidOperationException("Unsupported language " + language);
            }

            currentLanguage = language;
        }

        private TranslationsArray getCurrentTranslations() {

            assertTranslationsNotNull();

            return translationArrays[currentLanguage];
        }

        public string[] getTranslationArray(string key) {
            return getCurrentTranslations().getTranslationArray(key);
        }

        public string[] getTranslationArray(string key, int pos, int nb) {
            return getCurrentTranslations().getTranslationArray(key, pos, nb);
        }

        public string getTranslation(string key, int pos) {
            return getCurrentTranslations().getTranslation(key, pos);
        }

        public string getTranslation(string key) {
            return getCurrentTranslations().getTranslation(key);
        }

    }

}