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
}
