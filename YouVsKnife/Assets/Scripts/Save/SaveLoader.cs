/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;


public class SaveLoader : MonoBehaviour {


    protected void Awake() {

        GameSaver.instance.load();
    }

}
