using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour {
    public GameObject wayPointIcon;
    public Transform playerTransform;
    public bool includeSubBodies;
    public float waypointRotationOffset = 45f;
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
    private float arctangentValue;
    private float[,] quadrants;

    private GameObject waypoint;

    void Awake() {
        pBoundsWidth = Screen.width * pointerBoundsWidthPercentage;
        pBoundsHeight = Screen.height * pointerBoundsHeightPercentage;
        halfPBoundsWidth = pBoundsWidth / 2;
        halfPBoundsHeight = pBoundsHeight / 2;

        pointerBounds = new Bounds(new Vector3(Screen.width / 2, Screen.height / 2, 0), new Vector3(pBoundsWidth, pBoundsHeight, 0));

        arctangentValue = Mathf.Atan(pBoundsHeight / pBoundsWidth);
        quadrants = new float[4, 2];
        //right
        quadrants[0, 0] = -arctangentValue;
        quadrants[0, 1] = arctangentValue;
        //up
        quadrants[1, 0] = arctangentValue;
        quadrants[1, 1] = Mathf.PI - arctangentValue;
        //left
        quadrants[2, 0] = Mathf.PI - arctangentValue;
        quadrants[2, 1] = Mathf.PI + arctangentValue;
        //down
        quadrants[3, 0] = Mathf.PI + arctangentValue;
        quadrants[3, 1] = -arctangentValue;
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

        Debug.DrawLine(playerTransform.position, closestPosition, Color.green, 0.1f);

        /*Ray ray = new Ray(playerTransform.position, closestPosition - playerTransform.position);
        pointerBounds.IntersectRay(ray);

        Vector3 pos = pointerBounds.ClosestPoint(Camera.main.WorldToScreenPoint(closestPosition));*/


        Vector3 offsetDir = closestPosition - playerTransform.position;
        float angle = Mathf.Atan2(offsetDir.y, offsetDir.x) + Mathf.PI;
        //Vector2.Angle(Camera.main.WorldToScreenPoint(playerTransform.position), Camera.main.WorldToScreenPoint(offsetDir));//
        //waypoint.transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log("a" + angle);

        /*Vector3 pos = Camera.main.WorldToScreenPoint(closestPosition);
        pos.x = Mathf.Clamp(pos.x, pointerBounds.min.x, pointerBounds.max.x);
        pos.y = Mathf.Clamp(pos.y, pointerBounds.min.y, pointerBounds.max.y);
        pos.z = transform.position.z;*/

        //Debug.Log("cp:" + closestPosition + ", p:" + pos + ", pp:" + playerPosition + ", d:" + closestDistance + ", a:" + angle);

        waypoint.transform.position = PointOnBounds(angle);
    }

    public Vector2 PointOnBounds(int quadrantGroup, float angle) {
        if(quadrantGroup == 0) {
            return new Vector2(0, halfPBoundsWidth * Mathf.Tan(angle) - halfPBoundsHeight);
        }
        return new Vector2((pBoundsHeight / (2 * Mathf.Tan(angle))) - halfPBoundsWidth, 0);
    }
    public Vector2 PointOnBounds(float angle) {
        if((angle > quadrants[0,0] && angle > quadrants[0, 1]) || (angle > quadrants[2, 0] && angle > quadrants[2, 1])) {
            return PointOnBounds(0, angle);
        }
        return PointOnBounds(1, angle);
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
