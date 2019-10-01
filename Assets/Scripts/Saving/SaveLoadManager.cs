using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager {

    public static void saveTileMappings(Dictionary<int, int[,]> tileMapping) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/tileMappings.sav", FileMode.Create);

        TileMapping mapping = new TileMapping(tileMapping);

        bf.Serialize(stream, mapping);
        stream.Close();
    }

    public static Dictionary<int, int[,]> loadTileMappings() {
        if(File.Exists(Application.persistentDataPath + "/tileMappings.sav")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/tileMappings.sav", FileMode.Open);

            Dictionary<int, int[,]> tileMapping = ((TileMapping)bf.Deserialize(stream)).mapping;
            stream.Close();

            return tileMapping;
        }

        return new Dictionary<int, int[,]>();
    }
}

[Serializable]
public class TileMapping {
    public Dictionary<int, int[,]> mapping;

    public TileMapping(Dictionary<int, int[,]> mapping) {
        this.mapping = mapping;
    }
}