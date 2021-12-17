/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;


public class EffectsBehavior : MonoBehaviour {


    [SerializeField] private GameObject effectCut = null;


    public void playCut() {

        effectCut.SetActive(true);
    }

}
