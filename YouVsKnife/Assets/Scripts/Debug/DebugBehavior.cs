/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;


public class DebugBehavior : MonoBehaviour {


    [SerializeField] private TargetBehavior targetBehavior = null;


#if DEBUG && UNITY_EDITOR

    protected void Update() {

        if (Input.GetKeyUp(KeyCode.Space)) {
            targetBehavior.onTouch();
        }
    }

#endif

}
