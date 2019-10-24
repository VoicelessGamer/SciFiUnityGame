using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {
    public GameObject wayPointIcon;
    public Transform playerTransform;
    public bool includeSubBodies;
    [Range(0, 10)]
    public int maxPointers;

    [Range(0, 1)]
    public float pointerBoundsWidthPercentage = 0.95f;
    [Range(0, 1)]
    public float pointerBoundsHeightPercentage = 0.95f;

    private Transform centreMass;
    private List<Vector3> closestPositions;
    private List<float> closestDistances;
    private List<GameObject> waypoints;

    //variables for rotating the waypoints and keeping the waypointa within screen bounds
    private Bounds pointerBounds;
    private Vector3 playerPosition;
    private float pBoundsWidth;
    private float pBoundsHeight;
    private float halfPBoundsWidth;
    private float halfPBoundsHeight;
    private readonly float twoPi = Mathf.PI * 2; 
    private float arctangentValue;
    private float[,] quadrants;
    private Vector2 centrePosition;

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

        closestPositions = new List<Vector3>();
        closestDistances = new List<float>();
        waypoints = new List<GameObject>();
}

    void Update() {
        if (centreMass == null) {
            return;
        }

        playerPosition = playerTransform.position;
        //reset closest distances
        closestDistances = new List<float>();
        closestPositions = new List<Vector3>();
        foreach (Transform transform in centreMass.transform) {
            checkClosestBody(transform);
        }

        //limit the positions to max number of pointers
        if(closestPositions.Count > maxPointers) {
            closestPositions = closestPositions.GetRange(0, maxPointers);
        }
        //make sure correct number of pointers exist
        if(waypoints.Count != closestPositions.Count) {
            updateTotalWaypoints(closestPositions.Count);
        }

        for(int i = 0; i < waypoints.Count; i++) {
            GameObject waypoint = waypoints[i];

            //find rotation angle between player and closest body
            Vector3 offsetDir = playerTransform.position - closestPositions[i];
            float rot = Mathf.Atan2(offsetDir.y, offsetDir.x);
            float angle = -(rot - Mathf.PI);

            //make sure angle is within range of quadrant sections
            if (angle > twoPi - arctangentValue) {
                angle -= twoPi;
            }

            //rotate the pointer image to point at the target object
            Vector2.Angle(Camera.main.WorldToScreenPoint(playerTransform.position), Camera.main.WorldToScreenPoint(offsetDir));
            waypoint.transform.rotation = Quaternion.Euler(0, 0, rot * Mathf.Rad2Deg);

            //place the waypoint at the point on the bounds in the direction
            waypoint.transform.position = PointOnBounds(angle);
        }
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

    public void createWaypoints(int count) {
        for(int i = 0; i < count; i++) {
            waypoints.Add(Instantiate(wayPointIcon, gameObject.transform, false));
        }
    }

    public void updateTotalWaypoints(int total) {
        if(waypoints.Count < total) {
            createWaypoints(total - waypoints.Count);
        } else {
            destroyWaypoints(waypoints.Count - total);
        }
    }

    public void destroyWaypoints(int count) {
        if(waypoints.Count == 0) {
            return;
        }
        int start = waypoints.Count - 1, end = waypoints.Count - count > 0 ? waypoints.Count - count : 0;
        for (int i = start; i >= end; i--) {
            GameObject ob = waypoints[i];
            waypoints.Remove(ob);
            Destroy(ob);
        }
    }

    public void setCentreMass(Transform centreMass) {
        this.centreMass = centreMass;
    }

    private void checkClosestBody(Transform transform) {
        float distance = Vector3.Distance(transform.position, playerPosition);

        int distCount = closestDistances.Count;
        for (int i = 0; i < distCount; i++) {
            if (distance < closestDistances[i]) {
                closestDistances.Insert(i, distance);
                closestPositions.Insert(i, transform.position);
            }
        }
        if(closestDistances.Count == distCount) {
            closestDistances.Add(distance);
            closestPositions.Add(transform.position);
        }

        if(includeSubBodies && transform.childCount > 0) {
            foreach (Transform childTransform in transform) {
                checkClosestBody(childTransform);
            }
        }
    }
}
