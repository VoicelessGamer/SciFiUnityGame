using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEditCamera : MonoBehaviour {
    public float zoomAmount = 0; //With Positive and negative values
    public float minZoomClamp = 2;
    public float maxZoomClamp = 8;

    private Camera cam;
    private Vector3 dragOrigin;

    void Awake() {
        cam = this.GetComponent<Camera>();
    }

    void Update() {
        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scrollAxis, minZoomClamp, maxZoomClamp);

        if(Input.GetButtonDown("Fire2")) {
            dragOrigin = Input.mousePosition;
        } else if (Input.GetButton("Fire2")) {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(pos.x, pos.y, 0);

            transform.Translate(move, Space.World);

            /*Vector3 distance = dragOrigin - Input.mousePosition;
            transform.position +=  distance;
            dragOrigin = Input.mousePosition;*/
        }
    }
}
