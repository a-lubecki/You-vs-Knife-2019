/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;


public class CameraScaler : MonoBehaviour {


    private static readonly float MIN_RATIO = 1f;//square ratio
    private static readonly float MAX_RATIO = 2.2f;//iPhone X portrait

    private static readonly float MIN_ORTHOGRAPHIC_SIZE = 6f;//square ratio
    private static readonly float MAX_ORTHOGRAPHIC_SIZE = 8f;//iPhone X portrait


    protected void Awake() {

        var screenRatio = Screen.height / (float)Screen.width;

        Camera cam = GetComponent<Camera>();

        var size = 0f;

        if (screenRatio < MIN_RATIO) {
            size = MIN_ORTHOGRAPHIC_SIZE;
        } else if (screenRatio >= MAX_RATIO) {
            size = MAX_ORTHOGRAPHIC_SIZE;
        } else {

            var percentage = (screenRatio - MIN_RATIO) / (MAX_RATIO - MIN_RATIO);
            size = MIN_ORTHOGRAPHIC_SIZE + percentage * (MAX_ORTHOGRAPHIC_SIZE - MIN_ORTHOGRAPHIC_SIZE);
        }

        cam.orthographicSize = size;
    }

}