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
        selectedObject = Instantiate(go, new Vector3(0,0,0), Quaternion.identity);
        Color tmp = selectedObject.GetComponent<SpriteRenderer>().color;
        tmp.a = 0.5f;
        selectedObject.GetComponent<SpriteRenderer>().color = tmp;
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

        if(Input.GetButtonDown("Fire1")) {
            Instantiate(testObject, pos, Quaternion.identity);
        }
    }
}
