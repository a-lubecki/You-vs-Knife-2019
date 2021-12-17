/**
 * Alubecki Translations
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */


namespace Alubecki.Tr {

    public sealed class Tr {
        
        public static string get(string key) {
            return TranslationsManager.instance.getTranslation(key);
        }

        public static string get(string key, int pos) {
            return TranslationsManager.instance.getTranslation(key, pos);
        }

        public static string[] arr(string key) {
            return TranslationsManager.instance.getTranslationArray(key);
        }

        public static string[] arr(string key, int pos, int nb) {
            return TranslationsManager.instance.getTranslationArray(key, pos, nb);
        }

    }

}
