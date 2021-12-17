/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;
using System.Collections;


public class TargetBehavior : MonoBehaviour {


    private static readonly float MIN_LOCATION = 1.3f;
    private static readonly float MAX_LOCATION = 2.5f;
    private static readonly float MIN_FAR_ANGLE = 60;
    private static readonly float MIN_SPEED = 1;
    private static readonly float MAX_SPEED = 2;


    [SerializeField] private GameObject goTargetListener = null;
    private ITargetListener listener {
        get {
            if (goTargetListener == null) {
                return null;
            }
            return goTargetListener.GetComponent<ITargetListener>();
        }
    }

    [SerializeField] private Transform trColliderTarget = null;
    private Animation anim {
        get {
            return trColliderTarget.GetComponent<Animation>();
        }
    }

    [SerializeField] private bool canTouchTarget = false;

    private bool isHidden = false;

    private Coroutine coroutineShow;


    public Vector3 getTargetPosition() {
        return trColliderTarget.position;
    }

    public void resetLocation() {

        setNewLocation(MAX_LOCATION, 180);
    }

    public void setRandomLocation() {

        setNewLocation(newRandomLocation(), newRandomAngle(transform.localRotation.eulerAngles.z));
    }

    private void setNewLocation(float posY, float rotZ) {

        trColliderTarget.localPosition = new Vector3(0, posY, 0);

        transform.localRotation = Quaternion.Euler(0, 0, rotZ);
        trColliderTarget.rotation = Quaternion.identity;
    }

    private float newRandomLocation() {
        return Random.Range(MIN_LOCATION, MAX_LOCATION);
    }

    private float newRandomAngle(float lastAngle) {

        //set the current location far from the previous one
        var minRange = lastAngle + MIN_FAR_ANGLE;
        var maxRange = minRange + 360 - (2 * MIN_FAR_ANGLE);

        var angle = Random.Range(minRange, maxRange);
        if (angle > 360) {
            return angle - 360;
        }

        return angle;
    }

    private AnimationState getCurrentAnimState() {
        return anim[anim.clip.name];
    }

    public void playAnimationShow() {

        stopAnimationShow();

        isHidden = false;

        anim.Play();

        var state = getCurrentAnimState();
        state.time = 0;
        state.speed = 1;

        StartCoroutine(cutAnimationShow());
    }

    private IEnumerator cutAnimationShow() {

        yield return new WaitForSeconds(0.65f);

        anim.Stop();

        trColliderTarget.localScale = Vector3.one;

        canTouchTarget = true;
    }

    public void playAnimationShowThenHide(float speedPercentage) {

        canTouchTarget = true;
        isHidden = false;

        stopAnimationShow();

        anim.Play();

        var state = getCurrentAnimState();
        state.time = 0;
        state.speed = MIN_SPEED + speedPercentage * (MAX_SPEED - MIN_SPEED);

        coroutineShow = StartCoroutine(collapseWhenAnimationFinished());
    }

    public void playAnimationHide() {

        var wasHidden = isHidden;

        canTouchTarget = false;
        isHidden = true;

        stopAnimationShow();

        if (wasHidden) {
            //already stopped
            return;
        }

        anim.Play();

        var state = getCurrentAnimState();
        if (state.time < 1) {
            state.time = 1;
        }

        if (state.time < 2) {
            state.speed = 10;
        } else {
            state.speed = 5;
        }
    }

    public void stopAnimationShow() {

        if (coroutineShow != null) {
            StopCoroutine(coroutineShow);
        }

        coroutineShow = null;

        anim.Stop();
        anim.Rewind();
    }

    private IEnumerator collapseWhenAnimationFinished() {

        yield return new WaitWhile(() => anim.isPlaying);

        canTouchTarget = false;
        isHidden = true;

        listener?.onTargetCollape(this);
    }

    public void onTouch() {

        if (!canTouchTarget) {
            return;
        }

        canTouchTarget = false;

        listener?.onTargetTouch(this);
    }

}

public interface ITargetListener {

    void onTargetTouch(TargetBehavior target);

    void onTargetCollape(TargetBehavior target);
}
