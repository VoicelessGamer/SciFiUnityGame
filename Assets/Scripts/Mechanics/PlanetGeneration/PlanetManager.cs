using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Mechanics.TileMapGen;
using System.IO;

public class PlanetManager : MonoBehaviour
{
    //building details
    public int sectionWidth;
    public int sectionHeight;
    public GameObject tileMapContainer;
    public GameObject sectionPrefab;
    public TileMapGenerator tileMapGenerator;
    public int sectionsInView;

    //planet details
    [Range(5, 100)]
    public int totalSections;
    public Tile tile;//temporary (replace with class that alters the 1's in the generated tile mapping array)
    [SerializeField]
    private Dictionary<int, int[,]> planetTileMappings;

    //player deatils
    public Transform playerTransform;

    //temp details
    private StandardSectionGenerator sectionGen;
    int[] temporarySectionIndexes;

    // Start is called before the first frame update
    void Start()
    {
        this.planetTileMappings = SaveLoadManager.loadTileMappings();

        //create a new planet generator
        //(figure out how to determine what kind later)
        this.sectionGen = new StandardSectionGenerator(sectionWidth,
            sectionHeight,
            tileMapContainer,
            sectionPrefab,
            tileMapGenerator,
            tile);

        //eventually array will be generated from a starting section and surrounding positions
        temporarySectionIndexes = new int[] { this.totalSections - 1, 0, 1 };
        int startingX = -this.sectionWidth;

        for (int i = 0; i < temporarySectionIndexes.Length; i++) {
            int[,] tileMapping;
            //if section not in save data, create new section
            if (!this.planetTileMappings.ContainsKey(temporarySectionIndexes[i])) {
                tileMapping = this.sectionGen.generateSection();
                planetTileMappings.Add(temporarySectionIndexes[i], tileMapping);
            } else {
                tileMapping = planetTileMappings[temporarySectionIndexes[i]];
            }

            //passing through placed sections (places all to the right of first placed)
            GameObject section = this.sectionGen.buildSection(tileMapping, startingX + (this.sectionWidth * i));
        }

        SaveLoadManager.saveTileMappings(this.planetTileMappings);

        //startTestTimer();
    }

    //EVERYTHING BELOW FOR TESTING PURPOSES

    private int tr = 15;
    private int testInt = -2;
    void startTestTimer()
    {
        Invoke("_tick", 1f);
    }
    void _tick()
    {
        tr--;
        if (tr > 0)
        {
            for (int i = 0; i < temporarySectionIndexes.Length; i++)
            {
                temporarySectionIndexes[i] = temporarySectionIndexes[i] - 1;
                if (temporarySectionIndexes[i] < 0)
                {
                    temporarySectionIndexes[i] = this.totalSections - temporarySectionIndexes[i];
                }
            }
            
            int[,] tileMapping;
            //if section not in save data, create new section
            if (!this.planetTileMappings.ContainsKey(temporarySectionIndexes[0])) {
                tileMapping = this.sectionGen.generateSection();
                planetTileMappings.Add(temporarySectionIndexes[0], tileMapping);
            } else {
                tileMapping = planetTileMappings[temporarySectionIndexes[0]];
            }

            //passing through placed sections (places all to the right of first placed)
            GameObject section = this.sectionGen.buildSection(tileMapping, (this.sectionWidth * testInt));
            testInt--;

            SaveLoadManager.saveTileMappings(this.planetTileMappings);

            Invoke("_tick", 1f);
        }
    }
}
