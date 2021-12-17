/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;
using System.Collections;


public class AxisBehavior : MonoBehaviour {


    public static readonly float MIN_SPEED = 7;
    public static readonly float MAX_SPEED = 20;


    [SerializeField] private float speed = 0;
    [SerializeField] private GameObject goAxisListener = null;
    private IAxisListener listener {
        get {
            if (goAxisListener == null) {
                return null;
            }
            return goAxisListener.GetComponent<IAxisListener>();
        }
    }

    private float lastAngleDegrees;

    public float speedPercentage {
        get {

            var res = (speed - MIN_SPEED) / (MAX_SPEED - MIN_SPEED);

            if (res < 0) {
                return 0;
            }

            if (res > MAX_SPEED) {
                return MAX_SPEED;
            }

            return res;
        }
    }


    protected void FixedUpdate() {

        if (speed <= 0) {
            //do nothing when no speed
            return;
        }

        transform.Rotate(Vector3.back, speed);

        var currentAngleDegrees = transform.localEulerAngles.z;

        if (currentAngleDegrees > 270 && lastAngleDegrees < 90) {
            //the knife has done a complete rotation
            listener?.onAxisRotate360(this);
        }

        lastAngleDegrees = currentAngleDegrees;
    }

    public void stopRotating() {

        if (speed <= 0) {
            //already stopped
            return;
        }

        StartCoroutine(animateStop());
    }

    private IEnumerator animateStop() {

        //animate speed reduce
        while (speed > 3) {

            yield return new WaitForSeconds(0.1f);
            setSpeed(speed / 1.2f);
        }

        //speed is 1, wait until angle between 0 and 10°
        yield return new WaitUntil(() => transform.rotation.eulerAngles.z % 360 <= 30);

        //set speed to 0
        while (speed > 0) {

            yield return new WaitForSeconds(0.1f);
            setSpeed(speed - 1);
        }
    }

    public void increaseSpeed() {

        if (speed <= 0) {
            setSpeed(MIN_SPEED);
            return;
        }

        if (speed > MAX_SPEED) {
            //already at max
            return;
        }

        //increase by 20% of current speed diff (slow progress)
        setSpeed(speed + 0.2f * (MAX_SPEED - speed));
    }

    private void setSpeed(float newSpeed) {

        if (newSpeed < 0) {
            newSpeed = 0;
        }

        if (newSpeed == speed) {
            return;
        }

        speed = newSpeed;

        listener?.onAxisSpeedChange(this);
    }

}

public interface IAxisListener {

    void onAxisSpeedChange(AxisBehavior axis);

    void onAxisRotate360(AxisBehavior axis);

}
