using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics.TileMapGen;

public class TileMapGeneratorFactory {
    public static TileMapGenerator getTileMapGenerator(string type, string jsonString) {
        switch(type) {
            case "STANDARD":
                return StandardTileMapGenerator.CreateFromJSON(jsonString);
            case "SIMPLE":
                return SimpleTileMapGenerator.CreateFromJSON(jsonString);
        }

        return null;
    }
}
