using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics.TileMapGen;

public class ConfigurationLoader {
    
    [System.Serializable]
    public struct MapConfiguration {

        [System.Serializable]
        public struct MapEntry {
            public string groupId;
            public SimpleTileMapGenerator[] simpleConfigurations;
            public StandardTileMapGenerator[] standardConfigurations;
            public RelativePathTileMapGenerator[] relativePathConfigurations;
        }

        public MapEntry[] configurations;
    }

    private MapConfiguration mapConfiguration;

    public static MapConfiguration createMapConfigFromJSON(string jsonString) {
        return JsonUtility.FromJson<MapConfiguration>(jsonString);
    }

    public static List<TileMapGenerator> getTileMapGenerators(MapConfiguration mapConfiguration, List<string> groupIds) {
        List<TileMapGenerator> tileMapGenerators = new List<TileMapGenerator>();

        foreach(MapConfiguration.MapEntry mapGroup in mapConfiguration.configurations) {
            if(groupIds.Contains(mapGroup.groupId)) {
                if(mapGroup.simpleConfigurations != null) {
                    tileMapGenerators.AddRange(mapGroup.simpleConfigurations);
                }
                if(mapGroup.standardConfigurations != null) {
                    tileMapGenerators.AddRange(mapGroup.standardConfigurations);
                }
                if(mapGroup.relativePathConfigurations != null) {
                    tileMapGenerators.AddRange(mapGroup.relativePathConfigurations);
                }
            }
        }

        return tileMapGenerators;
    }
}
