/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System.Collections;
using UnityEngine;


public class SplatBehavior : MonoBehaviour {


    public static readonly float DISTANCE_SPLAT_BACKGROUND_MIN = 1;
    public static readonly float DISTANCE_SPLAT_BACKGROUND_MAX = 3.3f;

    private static int currentZOrder = 0;


    [SerializeField] private Sprite[] spritesSplatBackground = null;
    [SerializeField] private Sprite[] spritesSplatKnife = null;

    private Transform trSpriteSplat;
    private SpriteRenderer spriteRendererSplat;
    private Animation animDisappear;

    private Coroutine coroutineDisappear;

    public ISplatListener listener;


    protected void Awake() {

        trSpriteSplat = transform.GetChild(0).transform;
        spriteRendererSplat = trSpriteSplat.GetComponent<SpriteRenderer>();
        animDisappear = trSpriteSplat.GetComponent<Animation>();
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

        listener?.onSplatDisappear(this);
    }

    private float calculateDistancePercentage(float distanceFromCenter) {

        var res = (distanceFromCenter - DISTANCE_SPLAT_BACKGROUND_MIN) / (DISTANCE_SPLAT_BACKGROUND_MAX - DISTANCE_SPLAT_BACKGROUND_MIN);

        if (res < 0) {
            return 0;
        }

        if (res > 1) {
            return 1;
        }

        return res;
    }

    public void setAsSplatBackground(float angleDegrees, float distanceFromCenter) {

        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = new Vector3(0, 0, angleDegrees);

        var distancePercentage = calculateDistancePercentage(distanceFromCenter);
        spriteRendererSplat.sprite = selectSprite(spritesSplatBackground, distancePercentage);

        var distance = DISTANCE_SPLAT_BACKGROUND_MIN + distancePercentage * (DISTANCE_SPLAT_BACKGROUND_MAX - DISTANCE_SPLAT_BACKGROUND_MIN);
        trSpriteSplat.localPosition = new Vector3(0, distance, getZOrder());
    }

    public void setAsSplatKnife(float distanceFromCenter) {

        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;

        spriteRendererSplat.sprite = selectSprite(spritesSplatKnife, calculateDistancePercentage(distanceFromCenter));

        trSpriteSplat.localPosition = new Vector3(0, 0, getZOrder());
    }

    private Sprite selectSprite(Sprite[] sprites, float distancePercentage) {

        int nbSprites = sprites.Length;
        int pos = Mathf.FloorToInt(distancePercentage * nbSprites);

        if (pos < 0) {
            pos = 0;
        } else if (pos >= nbSprites) {
            pos = nbSprites - 1;
        }

        return sprites[pos];
    }

    private float getZOrder() {

        //increment the order and return the z value of the splat sprite
        currentZOrder++;

        return -0.0001f * currentZOrder;
    }
}

public interface ISplatListener {

    void onSplatDisappear(SplatBehavior splat);
}
