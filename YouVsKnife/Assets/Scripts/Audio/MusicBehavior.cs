/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;


public class MusicBehavior : MonoBehaviour {


    private static readonly float MIN_PITCH = 1;
    private static readonly float MAX_PITCH = 1.5f;


    private AudioSource audioSource;


    protected void Awake() {

        audioSource = GetComponent<AudioSource>();
    }

    public void reset() {

        updatePitch(MIN_PITCH);
    }

    public void updateSpeedPercentage(float speedPercentage) {

        if (speedPercentage < 0) {
            speedPercentage = 0;
        } else if (speedPercentage > 1) {
            speedPercentage = 1;
        }

        //exponential change
        speedPercentage = speedPercentage * speedPercentage * speedPercentage;

        updatePitch(MIN_PITCH + speedPercentage * (MAX_PITCH - MIN_PITCH));
    }

    private void updatePitch(float pitch) {
        audioSource.pitch = pitch;
    }

}
