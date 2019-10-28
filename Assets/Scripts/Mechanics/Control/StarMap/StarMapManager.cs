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
        GameObject icon = createIcon(new Vector3(0,0,0));

        for(int i = 0; i < 5; i++) {

        }

        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(0, 0, 0);
        positions[1] = new Vector3(10, 10, 0);

        GameObject connection = createConnection(positions);
    }

    private GameObject createIcon(Vector3 position) {
        GameObject icon = Instantiate(starmapIcon, position, Quaternion.identity);
        icon.name = "icon";

        return icon;
    }

    private GameObject createConnection(Vector3[] positions) {
        GameObject connection = new GameObject();
        connection.name = "connection";
        LineRenderer lineRenderer = connection.AddComponent<LineRenderer>();

        lineRenderer.SetPositions(positions);

        return connection;
    }
}
