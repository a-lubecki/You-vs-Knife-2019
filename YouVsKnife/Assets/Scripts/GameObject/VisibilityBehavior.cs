/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;


public class VisibilityBehavior : MonoBehaviour {


    public void show() {
        
        gameObject.SetActive(true);
    }

    public void hide() {
        
        gameObject.SetActive(false);
    }

}
