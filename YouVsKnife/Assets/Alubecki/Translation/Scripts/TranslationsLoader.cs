/**
 * Alubecki Translations
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;
using System.Collections.Generic;
using UnityEngine;


namespace Alubecki.Tr {

    internal class TranslationsLoader : MonoBehaviour {


        [SerializeField] private SystemLanguage fallbackLanguage = SystemLanguage.English;

        [SerializeField] private SystemLanguage editorDebugLanguage = SystemLanguage.English;

        [SerializeField] private TranslationsBundle[] translationsByLanguage = null;


        protected void Awake() {

            if (translationsByLanguage == null) {
                throw new InvalidOperationException();
            }

            var m = TranslationsManager.instance;

            m.init(fallbackLanguage, loadAllTranslationsBundles());

#if UNITY_EDITOR
            m.updateCurrentLanguage(editorDebugLanguage);
#else
            var l = Application.systemLanguage;
            if (m.hasLanguage(l)) {
                m.updateCurrentLanguage(l);
            }
#endif
        }

        private Dictionary<SystemLanguage, TranslationsArray> loadAllTranslationsBundles() {

            var res = new Dictionary<SystemLanguage, TranslationsArray>();

            foreach (var b in translationsByLanguage) {

                var trArray = loadTranslationsBundle(b);
                if (trArray == null) {
                    throw new InvalidOperationException();
                }

                res.Add(b.language, trArray);
            }

            return res;
        }

        private TranslationsArray loadTranslationsBundle(TranslationsBundle b) {

            var language = b.language;

            var res = new Dictionary<string, string[]>();

            foreach (var asset in b.assets) {

                var trPart = loadTranslationsAsset(language, asset);
                if (trPart == null) {
                    throw new InvalidOperationException();
                }

                //merge dictionaries
                foreach (var e in trPart) {

                    if (res.ContainsKey(e.Key)) {
                        throw new InvalidOperationException("The key " + e.Key + "is duplicated for " + language);
                    }

                    res.Add(e.Key, e.Value);
                }
            }

            return new TranslationsArray(language, res);
        }

        private Dictionary<string, string[]> loadTranslationsAsset(SystemLanguage language, TextAsset textAsset) {

            if (textAsset == null) {
                throw new ArgumentException("Translation asset null for " + language);
            }

            Dictionary<string, string[]> res = new Dictionary<string, string[]>();

            string[] newTranslations = textAsset.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            string key;
            List<string> noBlankTrList = new List<string>();

            int nbLines = newTranslations.Length;
            for (int i = 0; i < nbLines; i++) {

                //replace the \\n by a real line break
                string[] tr = newTranslations[i].Replace("\\n", "\n").Replace("\r", "").Split('\t');

                if (tr.Length < 2) {
                    throw new InvalidOperationException("No value found for translation : " + tr.Length + " for " + language);
                }

                key = tr[0];

                if (key.Length <= 0) {
                    throw new InvalidOperationException("Empty key for " + language);
                }

                if (res.ContainsKey(key)) {
                    throw new InvalidOperationException("Duplicate key " + key + " for " + language);
                }

                //remove the blank parts from tr
                noBlankTrList.Clear();

                int nbTr = tr.Length;
                for (int part = 1; part < nbTr; part++) {

                    string trPart = tr[part];
                    if (trPart.Length <= 0) {
                        break;
                    }

                    noBlankTrList.Add(trPart);
                }

                int nbNoBlankTr = noBlankTrList.Count;
                if (nbNoBlankTr < 1) {
                    throw new InvalidOperationException("No translation found : " + key + " for " + language);
                }

                res.Add(key, noBlankTrList.ToArray());
            }

            return res;
        }

    }

}