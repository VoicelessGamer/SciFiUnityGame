using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {
    public GameObject wayPointIcon;
    public Transform playerTransform;
    public bool includeSubBodies;
    //public int maxPointers;

    [Range(0, 1)]
    public float pointerBoundsWidthPercentage = 0.95f;
    [Range(0, 1)]
    public float pointerBoundsHeightPercentage = 0.95f;

    private Transform centreMass;
    private Vector3 closestPosition;
    private float closestDistance;
    private Vector3 playerPosition;
    private Bounds pointerBounds;

    private float pBoundsWidth;
    private float pBoundsHeight;
    private float halfPBoundsWidth;
    private float halfPBoundsHeight;
    private readonly float twoPi = Mathf.PI * 2; 
    private float arctangentValue;
    private float[,] quadrants;
    private Vector2 centrePosition;

    private GameObject waypoint;

    void Awake() {
        pBoundsWidth = Screen.width * pointerBoundsWidthPercentage;
        pBoundsHeight = Screen.height * pointerBoundsHeightPercentage;
        halfPBoundsWidth = pBoundsWidth / 2;
        halfPBoundsHeight = pBoundsHeight / 2;

        pointerBounds = new Bounds(new Vector3(Screen.width / 2, Screen.height / 2, 0), new Vector3(pBoundsWidth, pBoundsHeight, 0));

        centrePosition = new Vector2(halfPBoundsWidth + pointerBounds.min.x, halfPBoundsHeight + pointerBounds.min.y);

        arctangentValue = Mathf.Atan2(pBoundsHeight, pBoundsWidth);
        quadrants = new float[3, 2];
        //right
        quadrants[0, 0] = -arctangentValue;
        quadrants[0, 1] = arctangentValue;
        //up
        quadrants[1, 0] = arctangentValue;
        quadrants[1, 1] = Mathf.PI - arctangentValue;
        //left
        quadrants[2, 0] = Mathf.PI - arctangentValue;
        quadrants[2, 1] = Mathf.PI + arctangentValue;
    }
    
    void Update() {
        if (centreMass == null || waypoint == null) {
            return;
        }

        playerPosition = playerTransform.position;
        closestDistance = 0;

        foreach (Transform transform in centreMass.transform) {
            checkClosestBody(transform);
        }

        //find rotation angle between player and closest body
        Vector3 offsetDir = playerTransform.position - closestPosition;
        float rot = Mathf.Atan2(offsetDir.y, offsetDir.x);
        float angle = -(rot - Mathf.PI);

        //make sure angle is within range of quadrant sections
        if(angle > twoPi - arctangentValue) {
            angle -= twoPi;
        }

        //rotate the pointer image to point at the target object
        Vector2.Angle(Camera.main.WorldToScreenPoint(playerTransform.position), Camera.main.WorldToScreenPoint(offsetDir));
        waypoint.transform.rotation = Quaternion.Euler(0, 0, rot * Mathf.Rad2Deg);

        //place the waypoint at the point on the bounds in the direction
        waypoint.transform.position = PointOnBounds(angle);
    }

    public Vector2 PointOnBounds(float angle) {
        Vector2 returnVect = new Vector2(centrePosition.x, centrePosition.y);

        //return vector based on the quadrant the angle lies between
        if ((angle > quadrants[0,0] && angle <= quadrants[0, 1])) {
            returnVect.x += halfPBoundsWidth;
            returnVect.y += -1 * halfPBoundsWidth * Mathf.Tan(angle);
        } else if ((angle > quadrants[1, 0] && angle <= quadrants[1, 1])) {
            returnVect.x += pBoundsHeight / (2 * Mathf.Tan(angle));
            returnVect.y += -1 * halfPBoundsHeight;
        } else if((angle > quadrants[2, 0] && angle <= quadrants[2, 1])) {
            returnVect.x += -1 * halfPBoundsWidth;
            returnVect.y += halfPBoundsWidth * Mathf.Tan(angle);
        } else {
            returnVect.x += -1 * (pBoundsHeight / (2 * Mathf.Tan(angle)));
            returnVect.y += halfPBoundsHeight;
        }
        
        return returnVect;
    }

    public void createWaypoint() {
        waypoint = Instantiate(wayPointIcon, gameObject.transform, false);
    }

    public void setCentreMass(Transform centreMass) {
        this.centreMass = centreMass;
    }

    private void checkClosestBody(Transform transform) {
        float distance = Vector3.Distance(transform.position, playerPosition);

        if (closestDistance == 0 || distance < closestDistance) {
            closestPosition = transform.position;
            closestDistance = distance;
        }

        if(includeSubBodies && transform.childCount > 0) {
            foreach (Transform childTransform in transform) {
                checkClosestBody(childTransform);
            }
        }
    }
}
