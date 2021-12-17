/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class LivesBehavior : MonoBehaviour {


    [SerializeField] private Texture2D textureLifeOn = null;
    [SerializeField] private Texture2D textureLifeOff = null;
    [SerializeField] private GameObject[] goLives = null;
    [SerializeField] private int lives = 0;


    protected void OnEnable() {

        updateUI();
    }

    public int getLives() {
        return lives;
    }

    public int getMaxLives() {
        return goLives.Length;
    }

    public void refillLives() {

        lives = getMaxLives();

        updateUI();
    }

    public void loseLife() {

        if (lives <= 0) {
            return;
        }

        lives--;

        updateUI();
    }

    public void resetLives() {

        lives = 0;

        updateUI();
    }

    private void updateUI() {

        for (var i = 0; i < getMaxLives(); i++) {

            if (i < lives - 1) {
                enableLife(goLives[i]);
            } else {
                disableLife(goLives[i]);
            }
        }
    }

    private void enableLife(GameObject goLife) {

        setTexture(goLife, textureLifeOn);

        getRotationAnim(goLife).Play();
    }

    private void disableLife(GameObject goLife) {

        setTexture(goLife, textureLifeOff);

        getRotationAnim(goLife).Stop();

        goLife.transform.localRotation = Quaternion.identity;
    }

    private void setTexture(GameObject goLife, Texture2D texture) {
        goLife.GetComponent<RawImage>().texture = texture;
    }

    private Animation getRotationAnim(GameObject goLife) {
        return goLife.GetComponent<Animation>();
    }

    public Vector3 getLifePosition(int pos) {
        return goLives[pos].transform.position;
    }

}
