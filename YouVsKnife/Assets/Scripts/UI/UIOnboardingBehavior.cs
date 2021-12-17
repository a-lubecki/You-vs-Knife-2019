/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;
using Alubecki.Tr;


public class UIOnboardingBehavior : MonoBehaviour, IUIBehavior {


    [SerializeField] private ScoreBehavior scoreBehavior = null;
    [SerializeField] private TranslatedTextRenderer rendererMaxScore = null;


    void IUIBehavior.showUI() {
        gameObject.SetActive(true);
    }

    void IUIBehavior.hideUI() {
        gameObject.SetActive(false);
    }

    void IUIBehavior.updateUI() {

        rendererMaxScore.renderText(string.Format(Tr.get(rendererMaxScore.getTranslationKey()), scoreBehavior.getMaxScore()));
    }

}