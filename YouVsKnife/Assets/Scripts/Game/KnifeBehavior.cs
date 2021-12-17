/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;


public class KnifeBehavior : MonoBehaviour {


    [SerializeField] private GameObject goKnifeListener = null;
    private IKnifeListener listener {
        get {
            if (goKnifeListener == null) {
                return null;
            }
			return goKnifeListener.GetComponent<IKnifeListener>();
        }
    }


    public void onTouch() {
        listener?.onKnifeTouch(this);
    }

}

public interface IKnifeListener {

    void onKnifeTouch(KnifeBehavior knife);
}
