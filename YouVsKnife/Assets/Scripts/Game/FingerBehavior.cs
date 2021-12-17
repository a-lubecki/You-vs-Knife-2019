/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System.Collections;
using UnityEngine;


public class FingerBehavior : MonoBehaviour {


    private Transform trFingerDistance;
    private Animation animDisappear;

    private Coroutine coroutineDisappear;

    public IFingerListener listener;


    protected void Awake() {

        trFingerDistance = transform.GetChild(0);
        animDisappear = trFingerDistance.GetChild(0).GetComponent<Animation>();
    }

    protected void OnEnable() {

        //notify listener after delay
        coroutineDisappear = StartCoroutine(triggerDisappear());
    }

    protected void OnDisable() {

        if (coroutineDisappear != null) {
            StopCoroutine(coroutineDisappear);
            coroutineDisappear = null;
        }
    }

    private IEnumerator triggerDisappear() {

        yield return new WaitForSeconds(animDisappear.clip.length);

        listener?.onFingerDisappear(this);
    }

    public void move(float angleDegrees, float distanceFromCenter) {

        transform.localEulerAngles = new Vector3(0, 0, angleDegrees);

        trFingerDistance.localPosition = new Vector3(0, distanceFromCenter, 0);
    }

}

public interface IFingerListener {

    void onFingerDisappear(FingerBehavior finger);
}
