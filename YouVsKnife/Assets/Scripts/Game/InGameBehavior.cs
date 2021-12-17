/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System.Collections;
using UnityEngine;
using Firebase.Analytics;


public class InGameBehavior : MonoBehaviour, ITargetListener, IKnifeListener, IAxisListener {


    [SerializeField] private ScoreBehavior scoreBehavior = null;
    [SerializeField] private LivesBehavior livesBehavior = null;
    [SerializeField] private MovingLifeBehavior movingLifeBehavior = null;
    [SerializeField] private TargetBehavior targetBehavior = null;
    [SerializeField] private AxisBehavior axisBehavior = null;
    [SerializeField] private CharacterFaceBehavior characterFaceBehavior = null;
    [SerializeField] private MusicBehavior musicBehavior = null;
    [SerializeField] private SplatsPoolBehavior splatsPool = null;
    [SerializeField] private FingersPoolBehavior fingersPool = null;
    [SerializeField] private EffectsBehavior effectsBehavior = null;
    [SerializeField] private SoundsBehavior soundsBehavior = null;
    [SerializeField] private bool isPlaying = false;
    [SerializeField] private bool isGameOver = false;

    private GameTimer gameTimer = new GameTimer();

    [SerializeField] private GameObject goInGameListener = null;
    private IInGameListener listener {
        get {
            if (goInGameListener == null) {
                return null;
            }
            return goInGameListener.GetComponent<IInGameListener>();
        }
    }


    public void resetGame() {

        isPlaying = false;
        isGameOver = false;

        scoreBehavior.resetScore();

        livesBehavior.refillLives();

        targetBehavior.resetLocation();
        targetBehavior.playAnimationShow();

        characterFaceBehavior.setFaceSwapEnabled(true);
        characterFaceBehavior.setFaceHappy();
    }

    void ITargetListener.onTargetTouch(TargetBehavior target) {

        if (isGameOver) {
            //game is stopped
            return;
        }

        if (!isPlaying) {

            //start
            isPlaying = true;

            gameTimer.start();

            listener?.onGameStart(this);
        }

        //increase score
        scoreBehavior.incrementScore();

        targetBehavior.playAnimationHide();

        soundsBehavior.playDing();

        axisBehavior.increaseSpeed();

        StartCoroutine(showTargetAfterDelay(0.5f, true));

        FirebaseManager.trackEvent("click_target");
    }

    void IAxisListener.onAxisSpeedChange(AxisBehavior axis) {

        var percentage = axis.speedPercentage;
        characterFaceBehavior.setRotatingPercentage(percentage);

        musicBehavior.updateSpeedPercentage(percentage);
    }

    void IAxisListener.onAxisRotate360(AxisBehavior axis) {

        //play a swoosh sound when the knife has done a complete rotation
        soundsBehavior.playSwoosh(axis.speedPercentage);
    }

    void ITargetListener.onTargetCollape(TargetBehavior target) {

        if (!isPlaying || isGameOver) {
            //game stopped
            return;
        }

        livesBehavior.loseLife();

        soundsBehavior.playTargetHide();

        FirebaseManager.trackEvent(
            "life_losed",
            "life_count",
            livesBehavior.getLives().ToString()
        );

        int lives = livesBehavior.getLives();
        if (lives <= 0) {
            triggerGameOver();
            return;
        }

        //move to random location before moving life to updated target position
        targetBehavior.setRandomLocation();

        movingLifeBehavior.moveLifeAnimated(
            livesBehavior.getLifePosition(lives - 1),
            targetBehavior.getTargetPosition()
        );

        StartCoroutine(showTargetAfterDelay(0.25f, false));
    }

    void IKnifeListener.onKnifeTouch(KnifeBehavior knife) {

        if (!isPlaying || isGameOver) {
            //game stopped
            return;
        }

        var angleDegrees = axisBehavior.transform.localEulerAngles.z;

        //get distance between touch or mouse pointer and the axis center
        var pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distanceFromCenter = Vector2.Distance(pointerPos, axisBehavior.transform.position);

        splatsPool.generateSplatBackground(angleDegrees, distanceFromCenter);
        splatsPool.generateSplatKnife(distanceFromCenter);

        fingersPool.generateFinger(angleDegrees, distanceFromCenter);

        effectsBehavior.playCut();

        soundsBehavior.playCut();
        soundsBehavior.playSplat();

        FirebaseManager.trackEvent("knife_cut");

        triggerGameOver();
    }

    private void triggerGameOver() {

        if (!isPlaying || isGameOver) {
            //game stopped
            return;
        }

        //game over
        isPlaying = false;
        isGameOver = true;

        gameTimer.stop();

        saveGameStats();

        livesBehavior.resetLives();

        targetBehavior.playAnimationHide();

        axisBehavior.stopRotating();

        //set thirsty and disable to avoid changing with the knife speed when returning to the start angle
        characterFaceBehavior.setFaceThirsty();
        characterFaceBehavior.setFaceSwapEnabled(false);

        musicBehavior.reset();

        listener?.onGameOver(this);

        StartCoroutine(playSoundGameOverDelayed());

        FirebaseManager.trackEvent(
            "game_over",
            new Parameter("life_count", livesBehavior.getLives()),
            new Parameter("score", scoreBehavior.getScore()),
            new Parameter("time_sec", gameTimer.getTimeSec())
        );
    }

    private void saveGameStats() {

        GameSaver.instance.data.totalPlayCount++;

        scoreBehavior.saveCurrentScoreIfMax();

        var timeSec = gameTimer.getTimeSec();

        if (timeSec > GameSaver.instance.data.maxTimeSec) {
            GameSaver.instance.data.maxTimeSec = timeSec;
        }

        GameSaver.instance.data.totalTimeSec += timeSec;

        GameSaver.instance.save();

        //update properties
        FirebaseManager.setUserProperty("total_play_count", GameSaver.instance.data.totalPlayCount.ToString());
        FirebaseManager.setUserProperty("max_score", scoreBehavior.getMaxScore().ToString());
        FirebaseManager.setUserProperty("max_time_sec", GameSaver.instance.data.maxTimeSec.ToString());
        FirebaseManager.setUserProperty("total_time_sec", GameSaver.instance.data.totalTimeSec.ToString());
    }

    private IEnumerator playSoundGameOverDelayed() {

        yield return new WaitForSeconds(0.75f);

        soundsBehavior.playGameOver();
    }

    private IEnumerator showTargetAfterDelay(float delay, bool moveToRandomLocation) {

        yield return new WaitForSeconds(delay);

        if (!isPlaying || isGameOver) {
            //game stopped
            yield break;
        }

        if (moveToRandomLocation) {
            targetBehavior.setRandomLocation();
        }

        targetBehavior.playAnimationShowThenHide(axisBehavior.speedPercentage);

        soundsBehavior.playTargetShow();
    }

}

public interface IInGameListener {

    void onGameStart(InGameBehavior inGameBehavior);

    void onGameOver(InGameBehavior inGameBehavior);
}
