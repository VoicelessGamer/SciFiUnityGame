using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMapManager : MonoBehaviour {
    public GameObject starmapIcon;

    private readonly float twoPi = Mathf.PI * 2;

    // Start is called before the first frame update
    void Start() {
        buildMap();
    }

    private void buildMap() {
        GameObject icon = createIcon(new Vector3(0,0,0));

        for(int i = 0; i < 5; i++) {
            float radAngle = Random.Range(0, twoPi);
            Vector3 relativeDir = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0);
            Vector3 newPosition = relativeDir * Random.Range(5, 30);
            
            icon = createIcon(newPosition);

            Vector3[] positions = new Vector3[2];
            positions[0] = new Vector3(0, 0, 0);
            positions[1] = newPosition;

            GameObject connection = createConnection(positions);
        }
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
