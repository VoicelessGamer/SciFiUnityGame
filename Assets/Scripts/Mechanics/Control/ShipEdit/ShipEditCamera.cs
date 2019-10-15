using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEditCamera : MonoBehaviour {
    public float zoomAmount = 0; //With Positive and negative values
    public float minZoomClamp = 2;
    public float maxZoomClamp = 8;
    public float panSpeed = 0.1f;
    //public Bounds maxPanBounds;
    public float maxPanWidth = 10f;
    public float maxPanHeight = 5f;

    [Range(0, 1)]
    public float panWidthPercentage = 0.95f;
    [Range(0, 1)]
    public float panHeightPercentage = 0.95f;

    private Camera cam;
    private Bounds panBounds;
    void Awake() {
        cam = GetComponent<Camera>();
        panBounds = new Bounds(new Vector3(Screen.width / 2, Screen.height / 2, 0), new Vector3(Screen.width * panWidthPercentage, Screen.height * panHeightPercentage, 0));
    }

    void Update() {
        float scrollAxis = Input.GetAxis("Mouse ScrollWheel");

        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scrollAxis, minZoomClamp, maxZoomClamp);

        if (!panBounds.Contains(Input.mousePosition)) {
            Vector3 pos = Input.mousePosition - panBounds.ClosestPoint(Input.mousePosition);
            Vector3 newPosition = transform.position + (new Vector3(pos.x, pos.y, 0) * panSpeed * Time.deltaTime);

            newPosition.x = Mathf.Clamp(newPosition.x, -maxPanWidth, maxPanWidth);
            newPosition.y = Mathf.Clamp(newPosition.y, -maxPanHeight, maxPanHeight);

            transform.position = newPosition;
        }
    }
}
