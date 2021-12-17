/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System;
using UnityEngine;


public class SplatsPoolBehavior : MonoBehaviour, ISplatListener {


    [SerializeField] private Transform trSplatsBackground = null;
    [SerializeField] private Transform trSplatsKnife = null;
    [SerializeField] private GameObject prefabSplat = null;


    public SplatBehavior generateSplatBackground(float angleDegrees, float distanceFromCenter) {

        var splat = dequeue(trSplatsBackground);
        splat.setAsSplatBackground(angleDegrees, distanceFromCenter);

        return splat;
    }

    public SplatBehavior generateSplatKnife(float distanceFromCenter) {

        var splat = dequeue(trSplatsKnife);
        splat.setAsSplatKnife(distanceFromCenter);

        return splat;
    }

    public void enqueue(SplatBehavior splat) {

        if (splat == null) {
            throw new ArgumentException();
        }

        splat.transform.SetParent(transform);
        splat.gameObject.SetActive(false);
    }

    public SplatBehavior dequeue(Transform trParent) {

        SplatBehavior splat = null;

        if (transform.childCount <= 0) {
            splat = GameObject.Instantiate(prefabSplat).GetComponent<SplatBehavior>();
            splat.listener = this;
        } else {
            splat = transform.GetChild(0).GetComponent<SplatBehavior>();
        }

        splat.transform.SetParent(trParent);
        splat.gameObject.SetActive(true);

        return splat;
    }

    void ISplatListener.onSplatDisappear(SplatBehavior splat) {

        enqueue(splat);
    }

}
