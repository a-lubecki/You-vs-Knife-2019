/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;


public class CharacterFaceBehavior : MonoBehaviour {


    public static readonly float MIN_SPEED_PERCENTAGE_FOR_HAPPY_FACE = 0.12f;


    [SerializeField] private Sprite spriteHappy = null;
    [SerializeField] private Sprite spriteWorried = null;
    [SerializeField] private Sprite spriteThirsty = null;
    [SerializeField] private bool isFaceSwapEnabled = true;

    private SpriteRenderer spriteRenderer = null;


    protected void Awake() {

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void setFaceSwapEnabled(bool enabled) {
        isFaceSwapEnabled = enabled;
    }

    public void setRotatingPercentage(float speedPercentage) {

        if (speedPercentage < MIN_SPEED_PERCENTAGE_FOR_HAPPY_FACE) {
            setFaceHappy();
        } else {
            setFaceWorried();
        }
    }

    public void setFaceHappy() {
        setSprite(spriteHappy);
    }

    public void setFaceWorried() {
        setSprite(spriteWorried);
    }

    public void setFaceThirsty() {
        setSprite(spriteThirsty);
    }

    private void setSprite(Sprite sprite) {

        if (!isFaceSwapEnabled) {
            return;
        }

        spriteRenderer.sprite = sprite;
    }

}
