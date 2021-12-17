/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;
using Alubecki.Tr;


public class UIGameBehavior : MonoBehaviour, IUIBehavior {


    [SerializeField] private ScoreBehavior scoreBehavior = null;
    [SerializeField] private TranslatedTextRenderer rendererScore = null;


    void IUIBehavior.showUI() {
        gameObject.SetActive(true);
    }

    void IUIBehavior.hideUI() {
        gameObject.SetActive(false);
    }

    void IUIBehavior.updateUI() {

        rendererScore.renderText(string.Format(Tr.get(rendererScore.getTranslationKey()), scoreBehavior.getScore()));
    }

}