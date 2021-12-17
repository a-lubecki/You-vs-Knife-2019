/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;


public class GameTimer {


    private float lastTimeStarted;
    private float lastTimeStopped;


    public bool isStarted() {
        return (lastTimeStarted > lastTimeStopped);
    }

    public int getTimeSec() {

        if (isStarted()) {
            return (int) (Time.timeSinceLevelLoad - lastTimeStarted);
        }

        return (int) (lastTimeStopped - lastTimeStarted);
    }

    public void start() {
        lastTimeStarted = Time.timeSinceLevelLoad;
    }

    public void stop() {
        lastTimeStopped = Time.timeSinceLevelLoad;
    }

}

