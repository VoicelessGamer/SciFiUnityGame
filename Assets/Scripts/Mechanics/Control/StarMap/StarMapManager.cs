using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMapManager : MonoBehaviour {
    public GameObject starmapIcon;

    [Range(0, 3)]
    public int minNewConnections;

    [Range(0, 3)]
    public int maxNewConnections;

    private readonly float twoPi = Mathf.PI * 2;

    private Dictionary<GameObject, List<GameObject>> systemConnections;

    // Start is called before the first frame update
    void Start() {
        systemConnections = new Dictionary<GameObject, List<GameObject>>();
        buildMap();
    }

    private void buildMap() {
        generateConnectedSystems(createIcon(new Vector3(0,0,0)));
    }

    private void generateConnectedSystems(GameObject originObject) {
        Vector3 originPoint = originObject.transform.position;
        int connectionsToCreate = Random.Range(minNewConnections, maxNewConnections);

        List<GameObject> connectedSystems = new List<GameObject>();

        GameObject TESTOBJ = null;

        for (int i = 0; i < connectionsToCreate; i++) {
            float radAngle = Random.Range(0, twoPi);
            Vector3 relativeDir = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0);
            Vector3 newPosition = relativeDir * Random.Range(5, 30);

            GameObject icon = createIcon(newPosition);
            connectedSystems.Add(icon);

            Vector3[] positions = new Vector3[2];
            positions[0] = originPoint;
            positions[1] = newPosition;

            createConnection(positions);

            //***Testing***
            if(i == connectionsToCreate - 1) {
                TESTOBJ = icon;
            }
            //******

            systemConnections.Add(icon, new List<GameObject>() { originObject });
        }

        systemConnections.Add(originObject, connectedSystems);

        //***Testing***
        Vector3 relDir = TESTOBJ.transform.position - originPoint;
        //Debug.Log("rel.dir: " + relDir); 
        float rot1 = Mathf.Atan2(relDir.y, relDir.x);
        Debug.Log("rel rot to origin: " + (rot1 * Mathf.Rad2Deg));

        //Debug.Log("this: " + TESTOBJ.transform.position);

        foreach (GameObject key in systemConnections.Keys) {
            if(TESTOBJ != key) {
                Debug.Log("other: " + key.transform.position);

                //float angle = Vector3.SignedAngle(key.transform.position, relDir, Vector3.back);
               
                Vector3 currentDirection = key.transform.position - TESTOBJ.transform.position;
                float rot2 = Mathf.Atan2(currentDirection.y, currentDirection.x);
                Debug.Log("rot2: " + (rot2 * Mathf.Rad2Deg));
                float rot3 = rot2 - rot1;
                Debug.Log("rot3 before: " + (rot3 * Mathf.Rad2Deg));
                if(rot3 < -Mathf.PI) {
                    rot3 += twoPi;
                } else if(rot3 > Mathf.PI) {
                    rot3 -= twoPi;
                }
                Debug.Log("rot3 after: " + (rot3 * Mathf.Rad2Deg));
                /*float angle = -(rot - Mathf.PI);
                Debug.Log("angle: " + angle);*/
            }
        }
        //******
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
