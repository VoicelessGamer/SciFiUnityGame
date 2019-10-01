using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Mechanics.TileMapGen;
using System.IO;

public abstract class SectionGenerator: MonoBehaviour {
    //building details
    protected int width;
    protected int height;
    protected GameObject tileMapContainer;
    protected GameObject sectionPrefab;
    protected TileMapGenerator tileMapGenerator;
    protected int totalSections;
    protected Tile tile;
    protected string sectionIdentifier = "PlanetSection-";// update to not be hard-coded

    public SectionGenerator(int width, 
            int height, 
            GameObject tileMapContainer, 
            GameObject sectionPrefab, 
            TileMapGenerator tileMapGenerator,
            Tile tile) {

        this.width = width;
        this.height = height;
        this.tileMapContainer = tileMapContainer;
        this.sectionPrefab = sectionPrefab;
        this.tileMapGenerator = tileMapGenerator;
        this.tile = tile;
    }

    public abstract int[,] generateSection();

    public abstract GameObject buildSection(int[,] tileMapping, int startingX);

    public void saveSection(GameObject go, int index) {
        string saveName = this.sectionIdentifier + index;

        Directory.CreateDirectory("Assets/Resources/Prefabs/Sections/");

        if(go) {
            string savePath = "Assets/Resources/Prefabs/Sections/" + saveName + ".prefab";
            if(PrefabUtility.SaveAsPrefabAsset(go, savePath)) {
                Debug.Log("Section saved under " + savePath);
            } else {
                Debug.Log("Section NOT saved, path: " + savePath);
            }
        }
    }

    public bool loadSection(int index) {
        GameObject go;
        bool generated = false;

        //generate the gameobject of the prefab is found in resources
        go = Resources.Load<GameObject>("Prefabs/Sections/" + this.sectionIdentifier + index);

        if(go) {
            generated = true;
            //update the gameobject position
            go = (GameObject)Instantiate(go, new Vector3(0, 0, 0), Quaternion.identity, this.tileMapContainer.transform);
        }

        return generated;
    }
}
