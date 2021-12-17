/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;


public class CameraBehavior : MonoBehaviour {


    public static readonly float SCREEN_POS_Y_GAME = 0;
    public static readonly float SCREEN_POS_Y_GAME_OVER = -9.5f;
    public static readonly float SCREEN_POS_Y_SHARE = 0;

    public static readonly float ANIM_MOVE_DURATION = 0.25f;

    private Vector3 lastScreenPos;
    private Vector3 nextScreenPos;
    private float movePercentage;


    protected void Awake() {

        moveToGameScreen(false);
    }

    protected void LateUpdate() {

        updatePos(true);
    }

    private void resetPos(float nextPosY) {

        movePercentage = 0;

        lastScreenPos = transform.position;

        nextScreenPos = transform.position;
        nextScreenPos.y = nextPosY;
    }

    private void updatePos(bool animated) {

        if (movePercentage >= 1) {
            //translation finished
            return;
        }

        if (animated) {

            movePercentage += Time.deltaTime / ANIM_MOVE_DURATION;
            transform.position = Vector3.Lerp(lastScreenPos, nextScreenPos, movePercentage);

        } else {

            movePercentage = 1;
            transform.position = nextScreenPos;
        }
    }

    public void moveToGameScreen(bool animated) {

        resetPos(SCREEN_POS_Y_GAME);

        updatePos(animated);
    }

    public void moveToGameOverScreen(bool animated) {

        resetPos(SCREEN_POS_Y_GAME_OVER);

        updatePos(animated);
    }

    public void moveToShareScreen(bool animated) {

        resetPos(SCREEN_POS_Y_SHARE);

        updatePos(animated);
    }

}