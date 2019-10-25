using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMapManager : MonoBehaviour {
    public GameObject starmapIcon;

    // Start is called before the first frame update
    void Start() {
        buildMap();
    }

    private void buildMap() {
        GameObject icon = Instantiate(starmapIcon, new Vector3(0,0,0), Quaternion.identity);
        icon.name = "icon";

        GameObject connection = Instantiate(new GameObject(), new Vector3(0, 0, 0), Quaternion.identity);
        connection.name = "connection";
        LineRenderer lineRenderer = connection.AddComponent<LineRenderer>();

        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(0,0,0);
        positions[1] = new Vector3(10, 10, 0);

        lineRenderer.SetPositions(positions);
    }
}
