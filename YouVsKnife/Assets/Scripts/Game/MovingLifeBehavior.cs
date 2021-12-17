/**
 * You vs Knife
 * © Aurélien Lubecki 2019
 * All Rights Reserved
 */

using UnityEngine;


public class MovingLifeBehavior : MonoBehaviour {


    [SerializeField] private Vector2 origin = Vector2.zero;
    [SerializeField] private Vector2 destination = Vector2.zero;


    protected void Update() {

        if (isOnSamePosition(destination)) {
            //already moved
            gameObject.SetActive(false);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 20);
    }

    private bool isOnSamePosition(Vector2 pos) {

        var currentPos = transform.position;
        return currentPos.x == pos.x && currentPos.y == pos.y;
    }

    public void moveLifeAnimated(Vector2 origin, Vector2 destination) {

        gameObject.SetActive(true);

        var z = transform.position.z;

        this.destination = new Vector3(destination.x, destination.y, z);

        transform.position = new Vector3(origin.x, origin.y, z);
    }

}
