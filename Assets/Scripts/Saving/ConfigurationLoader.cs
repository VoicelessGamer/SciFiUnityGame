using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics.TileMapGen;

public class ConfigurationLoader {
    
    [System.Serializable]
    public struct SimpleMapConfigurations {

        [System.Serializable]
        public struct SimpleMapEntry {
            public string id;
            public SimpleTileMapGenerator config;
        }

        public SimpleMapEntry[] configurations;
    }

    public static SimpleMapConfigurations createSimpleMapConfigFromJSON(string jsonString) {
        return JsonUtility.FromJson<SimpleMapConfigurations>(jsonString);
    }
}
