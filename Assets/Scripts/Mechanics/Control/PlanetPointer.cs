using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPointer : MonoBehaviour {
    public Transform playerTransform;
    public Transform centreMass;

    private Vector3 closestPosition;
    private float closestDistance;
    private Vector3 playerPosition;

    void Update() {
        playerPosition = playerTransform.position;
        foreach(Transform transform in centreMass.transform) {
            checkClosestBody(transform);
        }   
    }

    private void checkClosestBody(Transform transform) {
        float distance = Vector3.Distance(closestPosition, playerPosition);

        if (closestPosition == null || distance < closestDistance) {
            closestPosition = transform.position;
            closestDistance = distance;
        }

        if(transform.childCount > 0) {
            foreach (Transform childTransform in transform) {
                checkClosestBody(childTransform);
            }
        }
    }
}
