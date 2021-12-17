/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;
using System.Collections;
using Alubecki.Native;
using Alubecki.Tr;
using Alubecki.Banner;


public class GameManager : MonoBehaviour, IScoreListener, IInGameListener {


    [SerializeField] private CameraBehavior cameraBehavior = null;
    [SerializeField] private InGameBehavior inGameBehavior = null;
    [SerializeField] private UIOnboardingBehavior uiOnboarding = null;
    [SerializeField] private UIGameBehavior uiGame = null;
    [SerializeField] private UIGameOverBehavior uiGameOver = null;
    [SerializeField] private UIShareBehavior uiShare = null;
    [SerializeField] private BannersDisplayBehavior bannersDisplayBehavior = null;

    private IUIBehavior[] uiBehaviors;


    protected void Awake() {

        uiBehaviors = new IUIBehavior[] {
            uiOnboarding,
            uiGame,
            uiGameOver,
            uiShare
        };

        bannersDisplayBehavior.isDisplayEnabled = false;
    }

    protected void Start() {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 200;

        //android only, leave game when native back button is clicked
        Input.backButtonLeavesApp = true;

        updateUI();
        setOnboardingUIVisible();
    }

    void IInGameListener.onGameStart(InGameBehavior inGameBehavior) {

        setGameUIVisible();

        GameSaver.instance.data.totalPlayCount++;
        GameSaver.instance.save();

        FirebaseManager.trackEvent(
            "game_start",
            "play_count",
            GameSaver.instance.data.totalPlayCount.ToString()
        );
    }

    void IInGameListener.onGameOver(InGameBehavior inGameBehavior) {

        StartCoroutine(triggerGameOverAfterDelay());
    }

    void IScoreListener.onScoreChanged(ScoreBehavior knife, int score) {

        updateUI();
    }

    void IScoreListener.onMaxScoreChanged(ScoreBehavior knife, int maxScore) {

        updateUI();
    }

    private IEnumerator triggerGameOverAfterDelay() {

        yield return new WaitForSeconds(2);

        yield return showGameOverAnimated(true);
    }

    public void retryAnimated() {

        FirebaseManager.trackEvent("click_retry");

        StartCoroutine(showOnboardingAnimated());
    }

    public void shareAnimated() {

        GameSaver.instance.data.totalShareCount++;
        GameSaver.instance.save();

        FirebaseManager.setUserProperty("total_share_count", GameSaver.instance.data.totalShareCount.ToString());

        FirebaseManager.trackEvent("click_share");

        bannersDisplayBehavior.isDisplayEnabled = false;

        StartCoroutine(triggerShare());
    }

    public IEnumerator triggerShare() {

        yield return showShareAnimated();

        NativePopupManager.instance.shareScreenshot(
            Tr.get("Share.GameTitle"),
            Tr.get("Share.Subject"),
            Tr.get("Share.Body"),
            "https://youvsknife.page.link/app",
            () => StartCoroutine(showGameOverAnimated(false))
        );
    }

    private IEnumerator showOnboardingAnimated() {

        cameraBehavior.moveToGameScreen(true);

        yield return new WaitForSeconds(CameraBehavior.ANIM_MOVE_DURATION);

        setOnboardingUIVisible();
    }

    private IEnumerator showGameOverAnimated(bool animateContent) {

        cameraBehavior.moveToGameOverScreen(true);

        yield return new WaitForSeconds(CameraBehavior.ANIM_MOVE_DURATION);

        setGameOverUIVisible(animateContent);
    }

    public IEnumerator showShareAnimated() {

        cameraBehavior.moveToShareScreen(true);

        yield return new WaitForSeconds(CameraBehavior.ANIM_MOVE_DURATION);

        setShareUIVisible();
    }

    public void updateUI() {

        foreach (var ui in uiBehaviors) {
            ui.updateUI();
        }
    }

    private void showUI(IUIBehavior uiToShow) {

        foreach (var ui in uiBehaviors) {

            if (ui == uiToShow) {
                ui.showUI();
            } else {
                ui.hideUI();
            }
        }
    }

    private void setOnboardingUIVisible() {

        inGameBehavior.resetGame();

        showUI(uiOnboarding);

        bannersDisplayBehavior.isDisplayEnabled = false;
    }

    private void setGameUIVisible() {

        showUI(uiGame);
    }

    private void setGameOverUIVisible(bool animateContent) {

        uiGameOver.setContentAnimated(animateContent);

        showUI(uiGameOver);

        int playCount = GameSaver.instance.data.totalPlayCount;
        if (playCount >= 3) {
            bannersDisplayBehavior.isDisplayEnabled = true;
        }

        if (bannersDisplayBehavior.isShowingBannerGame &&
            (playCount == 5 || playCount == 10 || playCount == 20)) {
            //hide banner game to display rating once if not done
            bannersDisplayBehavior.isBannerGameEnabled = false;
        } else {
            bannersDisplayBehavior.isBannerGameEnabled = true;
        }
    }

    private void setShareUIVisible() {

        showUI(uiShare);
    }

}
