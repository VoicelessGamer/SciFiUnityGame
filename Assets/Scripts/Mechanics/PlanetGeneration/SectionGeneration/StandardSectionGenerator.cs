using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Mechanics.TileMapGen;
using System.IO;

public class StandardSectionGenerator : SectionGenerator {


    //class properties
    private int initialX;
    private int initialY;

    public StandardSectionGenerator(int width, 
            int height, 
            GameObject tileMapContainer, 
            GameObject sectionPrefab, 
            TileMapGenerator tileMapGenerator,
            Tile tile) : base(width, height, tileMapContainer, sectionPrefab, tileMapGenerator, tile) {
        
        this.initialX = (int)(this.width * 0.5f);
        this.initialY = (int)(this.height * 0.5f);
    }

    public override int[,] generateSection() {
        return this.tileMapGenerator.generateMap(this.width, this.height);
    }

    public override GameObject buildSection(int[,] tileMapping, int xPos) {
        //Generate in instantiated clone of the section prefab
        GameObject instantiatedSection = (GameObject)Instantiate(sectionPrefab, new Vector3(xPos, 0, 0), Quaternion.identity, this.tileMapContainer.transform);
        //get the tile map component to add tiles to
        Tilemap section = (Tilemap)instantiatedSection.GetComponent(typeof(Tilemap));

        for(int x = 0; x < this.width; x++) {
            for(int y = 0; y < this.height; y++) {
                if(tileMapping[x,y] == 1) {
                    //set up new vector position for the current tile
                    Vector3Int pos = new Vector3Int(x - this.initialX, this.initialY - y - 1, 0);
                    //add tile in position
                    section.SetTile(pos, this.tile);
                }
            }
        }

        return instantiatedSection;
    }
}
