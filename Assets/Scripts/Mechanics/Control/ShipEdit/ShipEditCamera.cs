using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEditCamera : MonoBehaviour {
    public float zoomAmount = 0; //With Positive and negative values
    public float minZoomClamp = 2;
    public float maxZoomClamp = 8;

    private Camera cam;

    void Awake() {
        cam = this.GetComponent<Camera>();
    }

    void Update() {
        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scrollAxis, minZoomClamp, maxZoomClamp);
    }
}
