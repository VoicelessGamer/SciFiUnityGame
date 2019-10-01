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

    //player deatils
    public Transform playerTransform;

    //temp details
    private StandardSectionGenerator sectionGen;
    int[] temporarySectionIndexes;

    // Start is called before the first frame update
    void Start()
    {
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

        for (int i = 0; i < temporarySectionIndexes.Length; i++)
        {
            if (!this.sectionGen.loadSection(temporarySectionIndexes[i]))
            {
                int[,] tileMapping = this.sectionGen.generateSection();
                //passing through placed sections (places all to the right of first placed)
                GameObject section = this.sectionGen.buildSection(tileMapping, startingX + (this.sectionWidth * i));
                this.sectionGen.saveSection(section, temporarySectionIndexes[i]);
            }
        }

        startTestTimer();
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
            if (!this.sectionGen.loadSection(temporarySectionIndexes[0]))
            {
                int[,] tileMapping = this.sectionGen.generateSection();
                //passing through placed sections (places all to the right of first placed)
                GameObject section = this.sectionGen.buildSection(tileMapping, (this.sectionWidth * testInt));
                testInt--;
                this.sectionGen.saveSection(section, temporarySectionIndexes[0]);
            }
            Invoke("_tick", 1f);
        }
    }

    void Update()
    {
        //Debug.Log(playerTransform.position.x);
    }
}
