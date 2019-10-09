using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Mechanics.TileMapGen;
using System.IO;

public class StandardSectionBuilder : SectionBuilder {
    public override int[,] generateSection(int width, int height, TileMapGenerator tileMapGenerator) {
        return tileMapGenerator.generateMap(width, height);
    }

    public override void buildSection(int[,] tileMapping, GameObject instantiatedSection, Dictionary<int, Tile> tiles) {

        int width = tileMapping.GetLength(0);
        int height = tileMapping.GetLength(1);
        int initialX = (int)(width * 0.5f);
        int initialY = (int)(height * 0.5f);
        
        //get the tile map component to add tiles to
        Tilemap section = (Tilemap)instantiatedSection.GetComponent(typeof(Tilemap));

        for(int x = 0; x < width; x++) {
            for(int y = 0; y < height; y++) {
                if(tiles.ContainsKey(tileMapping[x,y])) {
                    //set up new vector position for the current tile
                    Vector3Int pos = new Vector3Int(x - initialX, initialY - y - 1, 0);
                    //add tile in position
                    section.SetTile(pos, tiles[tileMapping[x,y]]);
                }
            }
        }
    }
}
