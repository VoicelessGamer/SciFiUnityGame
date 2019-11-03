using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMapManager : MonoBehaviour {
    public GameObject starmapIcon;

    [Range(0, 3)]
    public int minNewConnections;
    [Range(0, 3)]
    public int maxNewConnections;
    public float safetyAngle;
    public float minimumConnectionDistance;
    public float maximumConnectionDistance;

    private float radSafteyAngle;
    private readonly float twoPi = Mathf.PI * 2;
    private Dictionary<GameObject, ConnectionInformation> systemConnections;

    public int TESTERINT = 5;

    // Start is called before the first frame update
    void Start() {
        radSafteyAngle = safetyAngle * Mathf.Deg2Rad;
        systemConnections = new Dictionary<GameObject, ConnectionInformation>();
        buildMap();
    }

    private void buildMap() {
        GameObject initialSystem = createIcon(new Vector3(0, 0, 0));
        systemConnections.Add(initialSystem, new ConnectionInformation(null, new List<GameObject>()));
        generateConnectedSystems(initialSystem);

        int index = 1;
        while(TESTERINT > 0) {
            TESTERINT -= 1;

            GameObject[] keys = new GameObject[systemConnections.Count];
            systemConnections.Keys.CopyTo(keys, 0);

            for (int i = index; i < keys.Length; i++) {
                generateConnectedSystems(keys[i]);
            }

            index = keys.Length;
        }
    }

    private void generateConnectedSystems(GameObject system) {
        //calculate direction offset this system is from its origin point
        GameObject originObject = systemConnections[system].originSystem;
        Vector3 distanceFromOrigin = originObject != null ? system.transform.position - originObject.transform.position : new Vector3();
        float originDirectionOffset = originObject != null ? Mathf.Atan2(distanceFromOrigin.y, distanceFromOrigin.x) : 0;

        Vector3 systemPoint = system.transform.position;

        float closestNegativeAngle = 0, closestPositiveAngle = 0;
        float[] generationZone = new float[2];

        int connectionsToCreate = Random.Range(minNewConnections, maxNewConnections + 1);

        List<GameObject> connectedSystems = new List<GameObject>();

        for (int i = 0; i < connectionsToCreate; i++) {
            //find the minimum and maximum angles to generate a new system between
            generationZone = calculateGenerationZone(system);
            closestNegativeAngle = generationZone[0] + radSafteyAngle;
            closestPositiveAngle = generationZone[1] - radSafteyAngle;
            
            //get new angle
            float radAngle = originDirectionOffset + Random.Range(closestNegativeAngle, closestPositiveAngle);
            Vector3 relativeDir = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0);
            Vector3 newPosition = systemPoint + (relativeDir * Random.Range(minimumConnectionDistance, maximumConnectionDistance));

            //create new system in caculated position
            GameObject icon = createIcon(newPosition);
            connectedSystems.Add(icon);

            Vector3[] positions = new Vector3[2];
            positions[0] = systemPoint;
            positions[1] = newPosition;

            //create connection line
            createConnection(positions);

            systemConnections.Add(icon, new ConnectionInformation(system, new List<GameObject>()));
        }

        systemConnections[system].connectedSystems = connectedSystems;
    }

    private float[] calculateGenerationZone(GameObject system) {
        GameObject originObject = systemConnections[system].originSystem;

        float closestNegativeAngle = -Mathf.PI;
        float closestPositiveAngle = Mathf.PI;

        if (originObject != null) {
            Vector3 originPoint = originObject.transform.position;

            //Get direction from this system' origin point
            Vector3 distanceFromOrigin = system.transform.position - originPoint;
            float originDirectionOffset = Mathf.Atan2(distanceFromOrigin.y, distanceFromOrigin.x);

            //check against all existsing systems
            foreach (GameObject key in systemConnections.Keys) {
                if (system != key) {
                    //relative distance the current iterated system is from this system
                    Vector3 relativeDistance = key.transform.position - system.transform.position;
                    float relativeAngleToSystem = Mathf.Atan2(relativeDistance.y, relativeDistance.x) - originDirectionOffset;

                    //make sure the relative angle is between -pi and +pi
                    if (relativeAngleToSystem < -Mathf.PI) {
                        relativeAngleToSystem += twoPi;
                    } else if (relativeAngleToSystem > Mathf.PI) {
                        relativeAngleToSystem -= twoPi;
                    }

                    //update the closest positive and negative angles to 0
                    if (relativeAngleToSystem < 0 && relativeAngleToSystem > closestNegativeAngle) {
                        closestNegativeAngle = relativeAngleToSystem;
                    } else if (relativeAngleToSystem > 0 && relativeAngleToSystem < closestPositiveAngle) {
                        closestPositiveAngle = relativeAngleToSystem;
                    }
                }
            }
        }

        return new float[2] { closestNegativeAngle, closestPositiveAngle };
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

    protected class ConnectionInformation {

        public GameObject originSystem;

        public List<GameObject> connectedSystems;

        public ConnectionInformation(GameObject originSystem, List<GameObject> connectedSystems) {
            this.originSystem = originSystem;
            this.connectedSystems = connectedSystems;
        }
    }
}
