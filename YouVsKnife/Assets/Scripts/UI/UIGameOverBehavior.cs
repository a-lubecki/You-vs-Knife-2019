/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;
using Alubecki.Tr;


public class UIGameOverBehavior : MonoBehaviour, IUIBehavior {


    [SerializeField] private ScoreBehavior scoreBehavior = null;
    [SerializeField] private TranslatedTextRenderer rendererScores = null;
    [SerializeField] private SoundsBehavior soundsBehavior = null;

    private Animation animContent;


    protected void Awake() {

        animContent = GetComponent<Animation>();
    }

    public void setContentAnimated(bool animated) {
        animContent.playAutomatically = animated;
    }

    void IUIBehavior.showUI() {
        gameObject.SetActive(true);
    }

    void IUIBehavior.hideUI() {
        gameObject.SetActive(false);
    }

    void IUIBehavior.updateUI() {

        rendererScores.renderText(string.Format(Tr.get(rendererScores.getTranslationKey()), scoreBehavior.getScore(), scoreBehavior.getMaxScore()));
    }

    public void playSoundKnifeAppear() {

        soundsBehavior.playKnifeAppear();
    }

    public void playSoundButtonAppear() {

        soundsBehavior.playButtonAppear();
    }

}