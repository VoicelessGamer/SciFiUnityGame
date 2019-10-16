using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShipEditor : MonoBehaviour {

    public GameObject testObject;
    public GameObject selectedObject;
    public GameObject gridObject;

    private Color activeColour;
    private Tilemap activeTilemap;

    void Start() {
        testObject.GetComponent<Tilemap>().CompressBounds();
        setSelectedObject(testObject);
    }

    public void setSelectedObject(GameObject go) {
        selectedObject = Instantiate(go, Input.mousePosition, Quaternion.identity, gridObject.transform);
        activeTilemap = selectedObject.GetComponent<Tilemap>();
        activeColour = activeTilemap.color;
        activeColour.a = 0.5f;
        activeTilemap.color = activeColour;
    }

    // Update is called once per frame
    void Update() {
        //the snap position on the grid
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.x = Mathf.Round(pos.x - 0.5f);
        pos.y = Mathf.Round(pos.y + 0.5f);
        pos.z = 0;

        //update the position of the object following the mouse
        selectedObject.transform.position = pos;

        if (Input.GetButtonDown("Fire1")) {
            activeColour.a = 1f;
            activeTilemap.color = activeColour;

            Tilemap tm = selectedObject.GetComponent<Tilemap>();
            Debug.Log(tm.cellBounds);
            setSelectedObject(testObject);
        }
    }
}
