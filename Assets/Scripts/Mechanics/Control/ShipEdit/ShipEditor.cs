using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShipEditor : MonoBehaviour {

    public GameObject testObject;
    public GameObject selectedObject;
    public GameObject gridObject;
    public Vector2[] gridPolygonBounds;

    private Color activeColour;
    private Tilemap activeTilemap;

    List<Vector3> occupiedSpaces;

    private void Awake() {
        occupiedSpaces = new List<Vector3>();
    }

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

        //once clicked
        if (Input.GetButtonDown("Fire1")) {

            bool canPlace = true;

            Tilemap tilemap = selectedObject.GetComponent<Tilemap>();

            List<Vector3> tilemapPositions = new List<Vector3>();

            //check all tiles in the current selected object
            for (int r = tilemap.cellBounds.xMin; r < tilemap.cellBounds.xMax; r++) {
                for (int c = tilemap.cellBounds.yMin; c < tilemap.cellBounds.yMax; c++) {
                    //local position in tilemap
                    Vector3Int localPos = new Vector3Int(r, c, (int)tilemap.transform.position.z);

                    //check there is a tile in the currect position
                    if (tilemap.GetTile(localPos) != null) {
                        //local position translated to world coordinates
                        Vector3 worldPos = tilemap.CellToWorld(localPos);
                        //if position already occupied then can't be placed
                        if (occupiedSpaces.Contains(worldPos) || !IsPointInPolygon(worldPos)) {
                            canPlace = false;
                            break;
                        } else {
                            //add the world position to a list 
                            tilemapPositions.Add(worldPos);
                        }
                    }
                }
                //quick return
                if(!canPlace) {
                    break;
                }
            }

            //if tilemap can be placed
            if (canPlace) {
                //add all positions to occupid spaces for future checks
                occupiedSpaces.AddRange(tilemapPositions);

                //make current tilemap fully visible
                activeColour.a = 1f;
                activeTilemap.color = activeColour;

                //update the object following the cursor 
                setSelectedObject(testObject);
            }
        }
    }

    public bool IsPointInPolygon(Vector2 point) {
        int polygonLength = gridPolygonBounds.Length, i = 0;
        bool inside = false;
        // x, y for tested point.
        float pointX = point.x, pointY = point.y;
        // start / end point for the current polygon segment.
        float startX, startY, endX, endY;
        Vector2 endPoint = gridPolygonBounds[polygonLength - 1];
        endX = endPoint.x;
        endY = endPoint.y;
        while (i < polygonLength) {
            startX = endX; startY = endY;
            endPoint = gridPolygonBounds[i++];
            endX = endPoint.x; endY = endPoint.y;
            inside ^= (endY > pointY ^ startY > pointY) && ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
        }
        return inside;
    }
}
