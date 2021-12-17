/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;
using UnityEngine;


public class FingersPoolBehavior : MonoBehaviour, IFingerListener {


    [SerializeField] private Transform trFingers = null;
    [SerializeField] private GameObject prefabFinger = null;


    public FingerBehavior generateFinger(float angleDegrees, float distanceFromCenter) {

        var finger = dequeue(trFingers);
        finger.move(angleDegrees, distanceFromCenter);

        return finger;
    }

    public void enqueue(FingerBehavior finger) {

        if (finger == null) {
            throw new ArgumentException();
        }

        finger.transform.SetParent(transform);
        finger.gameObject.SetActive(false);
    }

    public FingerBehavior dequeue(Transform trParent) {

        FingerBehavior finger = null;

        if (transform.childCount <= 0) {
            finger = GameObject.Instantiate(prefabFinger).GetComponent<FingerBehavior>();
            finger.listener = this;
        } else {
            finger = transform.GetChild(0).GetComponent<FingerBehavior>();
        }

        finger.transform.SetParent(trParent);
        finger.gameObject.SetActive(true);

        return finger;
    }

    void IFingerListener.onFingerDisappear(FingerBehavior finger) {

        enqueue(finger);
    }

}
