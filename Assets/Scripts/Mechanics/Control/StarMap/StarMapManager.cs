using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class StarMapManager : MonoBehaviour {
    public GameObject starmapIcon;

    [Range(0, 3)]
    public int minNewConnections;
    [Range(0, 3)]
    public int maxNewConnections;
    public float safetyAngle;
    public float minimumConnectionDistance;
    public float maximumConnectionDistance;
    public bool debugOuterPolygon;

    private float radSafteyAngle;
    private readonly float twoPi = Mathf.PI * 2;
    private readonly float halfPi = Mathf.PI / 2;
    private Dictionary<GameObject, ConnectionInformation> systemConnections;
    private List<Vector2> outerSystemPolygon;

    public int TESTERINT = 5;

    // Start is called before the first frame update
    void Start() {
        radSafteyAngle = safetyAngle * Mathf.Deg2Rad;
        outerSystemPolygon = new List<Vector2>();
        systemConnections = new Dictionary<GameObject, ConnectionInformation>();
        buildMap();
    }

    private void Update() {
        if(debugOuterPolygon && outerSystemPolygon.Count > 1) {
            for(int i = 0; i < outerSystemPolygon.Count; i++) {
                Debug.DrawLine(outerSystemPolygon[i], outerSystemPolygon[getWrappedOuterPolygonIndex(i + 1)]);
            }
        }
    }

    private void buildMap() {
        GameObject initialSystem = createIcon(new Vector3(0, 0, 0));
        systemConnections.Add(initialSystem, new ConnectionInformation(null, new List<GameObject>()));
        outerSystemPolygon.Add(new Vector2(0, 0));
        generateConnectedSystems(initialSystem);

        int index = 1;
        while (TESTERINT > 0) {
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
        Vector2 searchPoint = systemPoint;
        int expandPointIndex = outerSystemPolygon.IndexOf(searchPoint);

        //point must be on the outer polygon to create new points
        if (expandPointIndex == -1) {
            return;
        }

        float clockwiseAngle, antiClockwiseAngle;
        float[] generationZone;

        int connectionsToCreate = Random.Range(minNewConnections, maxNewConnections + 1);

        List<GameObject> connectedSystems = new List<GameObject>();

        for (int i = 0; i < connectionsToCreate; i++) {
            //find the minimum and maximum angles to generate a new system between
            generationZone = calculateGenerationZone(system, expandPointIndex);
            clockwiseAngle = generationZone[0] + radSafteyAngle;
            antiClockwiseAngle = generationZone[1] - radSafteyAngle;

            if(clockwiseAngle >= antiClockwiseAngle) {
                i = connectionsToCreate;
                continue;
            }

            //get new angle
            float randAngle = Random.Range(clockwiseAngle, antiClockwiseAngle);
            float radAngle = originDirectionOffset + randAngle;
            Vector3 relativeDir = new Vector3(Mathf.Cos(radAngle), Mathf.Sin(radAngle), 0);
            Vector3 newPosition = systemPoint + (relativeDir * Random.Range(minimumConnectionDistance, maximumConnectionDistance));

            //create new system in caculated position
            GameObject icon = createIcon(newPosition);
            connectedSystems.Add(icon);

            //add point to outer polygon
            outerSystemPolygon.Insert(randAngle > 0 ? expandPointIndex : getWrappedOuterPolygonIndex(expandPointIndex + 1), newPosition);

            Vector3[] positions = new Vector3[2];
            positions[0] = systemPoint;
            positions[1] = newPosition;

            //create connection line
            createConnection(positions);

            systemConnections.Add(icon, new ConnectionInformation(system, new List<GameObject>()));
        }

        systemConnections[system].connectedSystems = connectedSystems;

        if (outerSystemPolygon.Count > 3) {
            expandPointIndex = outerSystemPolygon.IndexOf(searchPoint);

            //point must be on the outer polygon to create new points
            if (expandPointIndex == -1) {
                return;
            }

            foreach (GameObject go in connectedSystems) {
                updateOuterPolygon(expandPointIndex);
                for (int j = 0; j < outerSystemPolygon.Count; j++) {
                    Debug.Log("OP " + j + ": " + outerSystemPolygon[j]);
                }
                Debug.Log("---------------------------");
            }
        } else {
            for (int j = 0; j < outerSystemPolygon.Count; j++) {
                Debug.Log("OP " + j + ": " + outerSystemPolygon[j]);
            }
            Debug.Log("---------------------------");
        }
    }

    private void updateOuterPolygon(int expandPointIndex) {
        List<Vector2> checkPolygon = new List<Vector2>();

        Vector2 checkPoint;

        bool expandOriginRemoved = false;

        for (int i = 0; i < outerSystemPolygon.Count; i++) {
            if(i != expandPointIndex) {
                checkPolygon.Add(outerSystemPolygon[i]);
            }
        }

        checkPoint = outerSystemPolygon[expandPointIndex];

        if (Utility.isPointInPolygon(checkPoint, checkPolygon)) {
            outerSystemPolygon = checkPolygon;
            expandOriginRemoved = true;
        }

        int checkIndexOffset = 3;

        checkPolygon = new List<Vector2>() {
            outerSystemPolygon[getWrappedOuterPolygonIndex(expandPointIndex - 1)],
            outerSystemPolygon[getWrappedOuterPolygonIndex(expandPointIndex)],
            new Vector2()
        };

        List<Vector2> removablePoints = new List<Vector2>();

        bool continueIterations = true;

        //update against previous points
        while (continueIterations) {
            checkPolygon[2] = outerSystemPolygon[getWrappedOuterPolygonIndex(expandPointIndex - checkIndexOffset)];
            checkPoint = outerSystemPolygon[getWrappedOuterPolygonIndex(expandPointIndex - (checkIndexOffset - 1))];

            continueIterations = Utility.isPointInPolygon(checkPoint, checkPolygon);

            if(continueIterations) {
                removablePoints.Add(checkPoint);
                checkIndexOffset++;
            }
        }

        checkIndexOffset = expandOriginRemoved ? 2 : 3;
        checkPolygon = new List<Vector2>() {
            outerSystemPolygon[getWrappedOuterPolygonIndex(expandOriginRemoved ? expandPointIndex - 1 : expandPointIndex)],
            outerSystemPolygon[getWrappedOuterPolygonIndex(expandOriginRemoved ? expandPointIndex : expandPointIndex + 1)],
            new Vector2()
        };

        continueIterations = true;

        //update against next points
        while (continueIterations) {
            checkPolygon[2] = outerSystemPolygon[getWrappedOuterPolygonIndex(expandPointIndex + checkIndexOffset)];
            checkPoint = outerSystemPolygon[getWrappedOuterPolygonIndex(expandPointIndex + (checkIndexOffset - 1))];

            continueIterations = Utility.isPointInPolygon(checkPoint, checkPolygon);

            if (continueIterations) {
                removablePoints.Add(checkPoint);
                checkIndexOffset++;
            }
        }

        for(int i = 0; i < removablePoints.Count; i++) {
            outerSystemPolygon.Remove(removablePoints[i]);
        }
    }

    private int getWrappedOuterPolygonIndex(int currentIndex) {
        int newIndex = currentIndex < 0 ? outerSystemPolygon.Count + currentIndex : currentIndex;
        newIndex = newIndex >= outerSystemPolygon.Count ? newIndex - outerSystemPolygon.Count : newIndex;

        return newIndex;
    }

    private float[] calculateGenerationZone(GameObject system, int searchPointIndex) {
        GameObject originObject = systemConnections[system].originSystem;

        float point1Angle = -Mathf.PI;
        float point2Angle = Mathf.PI;

        if (originObject != null && outerSystemPolygon.Count > 2) {
            //Get direction from this system' origin point
            Vector3 distanceFromOrigin = system.transform.position - originObject.transform.position;
            float oAngle = Mathf.Atan2(distanceFromOrigin.y, distanceFromOrigin.x);

            //Get direction from this system to previous polygon point
            Vector3 previousPolyPoint = outerSystemPolygon[searchPointIndex == 0 ? outerSystemPolygon.Count - 1 : searchPointIndex - 1];
            Vector3 relativeDistance = previousPolyPoint - system.transform.position;
            float pAngle = Mathf.Atan2(relativeDistance.y, relativeDistance.x);

            //Get direction from this system to next polygon point
            Vector3 nextPolyPoint = outerSystemPolygon[searchPointIndex == outerSystemPolygon.Count - 1 ? 0 : searchPointIndex + 1];
            relativeDistance = nextPolyPoint - system.transform.position;
            float nAngle = Mathf.Atan2(relativeDistance.y, relativeDistance.x);

            if (oAngle >= halfPi) {
                point1Angle = pAngle < 0 ? twoPi + pAngle - oAngle : pAngle - oAngle;
                point2Angle = nAngle - oAngle;
            } else if (oAngle >= 0 && oAngle < halfPi) {
                point1Angle = pAngle < 0 ? twoPi + pAngle - oAngle : pAngle - oAngle;
                point2Angle = nAngle - oAngle;
            } else if (oAngle < 0 && oAngle >= -halfPi) {
                point1Angle = pAngle - oAngle;
                point2Angle = nAngle < 0 ? nAngle - oAngle : nAngle - twoPi - oAngle;
            } else {
                point1Angle = pAngle - oAngle;
                point2Angle = nAngle < 0 ? nAngle - oAngle : nAngle - twoPi - oAngle;
            }
        }

        if (point1Angle < 0) {
            return new float[2] { point1Angle, point2Angle };
        } else {
            return new float[2] { point2Angle, point1Angle };
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

    protected class ConnectionInformation {

        public GameObject originSystem;

        public List<GameObject> connectedSystems;

        public ConnectionInformation(GameObject originSystem, List<GameObject> connectedSystems) {
            this.originSystem = originSystem;
            this.connectedSystems = connectedSystems;
        }
    }
}
