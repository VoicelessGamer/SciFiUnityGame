using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ShipEditor : MonoBehaviour {

    public enum EditorMode {
        Placement,
        Deletion
    }

    public GameObject testObject;
    public GameObject grid;
    public Vector2[] gridPolygonBounds;

    private GameObject attatchedObject;
    private Color activeColour;
    private Tilemap activeTilemap;

    private EditorMode editorMode;

    private List<Vector3> occupiedSpaces;

    private void Awake() {
        occupiedSpaces = new List<Vector3>();
    }

    // Update is called once per frame
    void Update() {
        if(attatchedObject != null) {
            //the snap position on the grid
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.x = Mathf.Round(pos.x - 0.5f);
            pos.y = Mathf.Round(pos.y + 0.5f);
            pos.z = 0;

            //update the position of the object following the mouse
            attatchedObject.transform.position = pos;
        }

        //once clicked
        if (Input.GetButtonDown("Fire1")) {
            switch(editorMode) {
                case EditorMode.Placement:
                    //quick return if missing an attatched object
                    if(attatchedObject == null) {
                        break;
                    }

                    Tilemap tilemap = attatchedObject.GetComponent<Tilemap>();

                    List<Vector3> tilemapPositions = new List<Vector3>();

                    //if tilemap can be placed
                    if (tilemapIsPlaceable(tilemap, tilemapPositions)) {
                        //add all positions to occupid spaces for future checks
                        occupiedSpaces.AddRange(tilemapPositions);

                        //make current tilemap fully visible
                        activeColour.a = 1f;
                        activeTilemap.color = activeColour;

                        //update the object following the cursor 
                        attatchObject(testObject);
                    }
                    break;
                case EditorMode.Deletion:
                    //get world position of click
                    Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    //match the z position to the grid
                    cursorPos.z = grid.transform.position.z;
                    //iterate all tilemap components in the grid child objects
                    foreach(Tilemap tm in grid.GetComponentsInChildren<Tilemap>()) {
                        //check for a tile in the cell position clicked on
                        if(tm.GetTile(tm.WorldToCell(cursorPos)) != null) {

                            freeOccupiedSpaces(tm);

                            Destroy(tm.gameObject);
                            break;
                        }
                    }
                    break;
            }
        }
    }

    public bool tilemapIsPlaceable(Tilemap tilemap, List<Vector3> tilemapPositions) {
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
                    if (occupiedSpaces.Contains(worldPos) || !isPointInPolygon(worldPos)) {
                        return false;
                    } else {
                        //add the world position to a list 
                        tilemapPositions.Add(worldPos);
                    }
                }
            }
        }

        return true;
    }

    public void freeOccupiedSpaces(Tilemap tilemap) {
        //check all tiles in the current selected object
        for (int r = tilemap.cellBounds.xMin; r < tilemap.cellBounds.xMax; r++) {
            for (int c = tilemap.cellBounds.yMin; c < tilemap.cellBounds.yMax; c++) {
                //local position in tilemap
                Vector3Int localPos = new Vector3Int(r, c, (int)tilemap.transform.position.z);

                //check there is a tile in the currect position
                if (tilemap.GetTile(localPos) != null) {
                    //remove position from list of occupied spaces
                    occupiedSpaces.Remove(tilemap.CellToWorld(localPos));
                }
            }
        }
    }

    public void attatchObject(GameObject go) {
        go.GetComponent<Tilemap>().CompressBounds();
        attatchedObject = Instantiate(go, Input.mousePosition, Quaternion.identity, grid.transform);
        activeTilemap = attatchedObject.GetComponent<Tilemap>();
        activeColour = activeTilemap.color;
        activeColour.a = 0.5f;
        activeTilemap.color = activeColour;
    }

    public void removeAttatchedObject() {
        Destroy(attatchedObject);
        attatchedObject = null;
    }

    public bool isPointInPolygon(Vector2 point) {
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

    public void changeEditorMode(int newMode) {
        switch(newMode) {
            case 0:
                editorMode = EditorMode.Placement;
                attatchObject(testObject);
                break;
            case 1:
                editorMode = EditorMode.Deletion;
                removeAttatchedObject();
                break;
        }
    }
}
