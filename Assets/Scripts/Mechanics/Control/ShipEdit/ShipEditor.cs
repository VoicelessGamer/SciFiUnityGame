using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEditor : MonoBehaviour {

    public GameObject testObject;
    public GameObject selectedObject;

    void Start() {
        setSelectedObject(testObject);
    }

    public void setSelectedObject(GameObject go) {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        selectedObject = Instantiate(go, pos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update() {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        selectedObject.transform.position = pos;
    }
}
