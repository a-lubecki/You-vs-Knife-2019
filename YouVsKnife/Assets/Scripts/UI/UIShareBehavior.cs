/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;
using Alubecki.Tr;


public class UIShareBehavior : MonoBehaviour, IUIBehavior {


    [SerializeField] private ScoreBehavior scoreBehavior = null;
    [SerializeField] private TranslatedTextRenderer rendererScores = null;


    void IUIBehavior.showUI() {
        gameObject.SetActive(true);
    }

    void IUIBehavior.hideUI() {
        gameObject.SetActive(false);
    }

    void IUIBehavior.updateUI() {

        rendererScores.renderText(string.Format(Tr.get(rendererScores.getTranslationKey()), scoreBehavior.getScore(), scoreBehavior.getMaxScore()));
    }

}