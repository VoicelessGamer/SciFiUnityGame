using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mechanics.TileMapGen;

public class ConfigurationLoader {
    
    [System.Serializable]
    public struct SimpleMapConfiguration {

        [System.Serializable]
        public struct SimpleMapEntry {
            public string groupId;
            public SimpleTileMapGenerator[] configurations;
        }

        public SimpleMapEntry[] configurations;
    }
    
    [System.Serializable]
    public struct StandardMapConfigurations {

        [System.Serializable]
        public struct StandardMapEntry {
            public string groupId;
            public StandardTileMapGenerator[] configurations;
        }
        
        public StandardMapEntry[] configurations;
    }

    public static SimpleMapConfiguration createSimpleMapConfigFromJSON(string jsonString) {
        return JsonUtility.FromJson<SimpleMapConfiguration>(jsonString);
    }

    public static StandardMapConfigurations createStandardMapConfigFromJSON(string jsonString) {
        return JsonUtility.FromJson<StandardMapConfigurations>(jsonString);
    }
}
