using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Mechanics.TileMapGen;
using System.IO;

public abstract class SectionBuilder {
    public abstract int[,] generateSection(int width, int height, TileMapGenerator tileMapGenerator);

    public abstract void buildSection(int[,] tileMapping, GameObject instantiatedSection, Dictionary<int, Tile> tiles);
}
