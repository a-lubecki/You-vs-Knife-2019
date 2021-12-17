/**
 * Alubecki Translations
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace Alubecki.Tr {

    public class TranslatedTextRenderer : MonoBehaviour {


        [SerializeField]
        string translationKey;

        public string getTranslationKey() {
            return translationKey;
        }

        [SerializeField]
        int positionInTrArray = 0;

        public bool wasRenderedOnce { get; private set; }


        void Awake() {

            renderTranslatedText();

            wasRenderedOnce = true;
        }

        public void renderTranslatedText(string translationKey, int positionInTrArray) {

            this.translationKey = translationKey;
            this.positionInTrArray = positionInTrArray;

            renderTranslatedText();
        }

        public void renderTranslatedText() {
            
            var translatedText = Tr.get(translationKey, positionInTrArray);

            renderText(translatedText);
        }

        public void renderText(string text) {
            
            foreach (var d in drivers) {
                
                var t = d.getComponent(this);
                if (t != null) {
                    //text component found
                    d.setText(t, text);
                    break;
                }
            }
        }


        private static readonly TextDriver[] drivers = {

            new TextDriver {
                getComponent = (b) => {
                    return b.GetComponent<Text>();
                },
                setText = (c, translatedText) => {
                    (c as Text).text = translatedText;
                }
            },

            new TextDriver {
                getComponent = (b) => {
                    return b.GetComponent<TextMesh>();
                },
                setText = (c, translatedText) => {
                    (c as TextMesh).text = translatedText;
                }
            },

            new TextDriver {
                getComponent = (b) => {
                    return b.GetComponent<TextMeshPro>();
                },
                setText = (c, translatedText) => {
                    (c as TextMeshPro).text = translatedText;
                }
            },

            new TextDriver {
                getComponent = (b) => {
                    return b.GetComponent<TextMeshProUGUI>();
                },
                setText = (c, translatedText) => {
                    (c as TextMeshProUGUI).text = translatedText;
                }
            },

            new TextDriver {
                getComponent = (b) => {
                    return b.GetComponent<GUIText>();
                },
                setText = (c, translatedText) => {
                    (c as GUIText).text = translatedText;
                }
            },

        };

    }


    struct TextDriver {

        public Func<MonoBehaviour, Component> getComponent; //this, component
        public Action<Component, string> setText; //<component, translatedText>
        
    }

}
