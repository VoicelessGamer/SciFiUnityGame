using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using Mechanics.TileMapGen;
using System.IO;
using Core;

public class PlanetManager : MonoBehaviour
{
    //building details
    public int sectionWidth;
    public int sectionHeight;
    public int sectionsInView;
    public GameObject tileMapContainer;
    public GameObject sectionPrefab;
    public TileMapGenerator tileMapGenerator;

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
    private List<Section> sections;
    private float[] xBoundaries = new float[2];

    private List<TileMapGenerator> tileMapGenerators;

    // Start is called before the first frame update
    void Start() {
        //setup map configurations
        this.tileMapGenerators = new List<TileMapGenerator>();
        setupMapConfigurations(new List<string>(){"simpleTest"});

        this.planetTileMappings = SaveLoadManager.loadTileMappings();

        this.sections = new List<Section>();

        //create a new planet generator
        //(figure out how to determine what kind later)
        this.sectionGen = new StandardSectionGenerator(sectionWidth,
            sectionHeight,
            tileMapContainer,
            sectionPrefab,
            tileMapGenerators[(int)Random.Range(0, tileMapGenerators.Count)],
            tile);

        //eventually array will be generated from a starting section and surrounding positions
        generateSectionsInView(0);

        SaveLoadManager.saveTileMappings(this.planetTileMappings);
    }

    void Update() {
        //builds and delete sections when the player crosses a boundary
        if(playerTransform.position.x < xBoundaries[0]) {
            shiftView(0);
        } else if(playerTransform.position.x > xBoundaries[1]) {
            shiftView(1);
        }
    }

    public void generateSectionsInView(int centredSection) {
        float halfWidth = (this.sectionWidth / 2);

        //Currently set to 3 sections in view
        //Centred section and the map section either side of it will be generated
        int[] sectionIndices = new int[] { centredSection != 0 ? centredSection - 1 :this.totalSections - 1, centredSection, centredSection + 1 };
        int initialX = -this.sectionWidth;

        for (int i = 0; i < sectionIndices.Length; i++) {
            int[,] tileMapping;
            int index = sectionIndices[i];
            //if section not in save data, create new section
            if (!this.planetTileMappings.ContainsKey(index)) {
                tileMapping = this.sectionGen.generateSection();
                this.planetTileMappings.Add(index, tileMapping);
            } else {
                tileMapping = this.planetTileMappings[index];
            }

            //building section, sending through the section x position to be placed
            GameObject section = this.sectionGen.buildSection(tileMapping, initialX + (this.sectionWidth * i));
            section.name = "Section-" + index;

            //set the boundaries, for the player to cross to load next and delete furthest sections, to the edges of the centred section
            if(index == centredSection) {
                xBoundaries[0] = section.transform.position.x - halfWidth;
                xBoundaries[1] = section.transform.position.x + halfWidth;
            }

            //store for later use (loading new sections, deleting old)
            this.sections.Add(new Section(index, section));
        }
    }

    /*
    Currently set up to work with 3 sections in view.
    Needs work if that value changes
     */
    public void shiftView(int dir) {
        float halfWidth = (this.sectionWidth / 2);
        int[,] tileMapping;
        
        if(dir == 0) {
            //left
            //update boundaries
            GameObject go = this.sections[0].getGameObject();
            xBoundaries[0] = go.transform.position.x - halfWidth;
            xBoundaries[1] = go.transform.position.x + halfWidth;
            
            //get index of the section to be created (wrapping if needed)
            int index = this.sections[0].getSectionId() - 1;
            index = index == -1 ? this.totalSections - 1 : index;

            //if section not in save data, create new section
            if (!this.planetTileMappings.ContainsKey(index)) {
                tileMapping = this.sectionGen.generateSection();
                this.planetTileMappings.Add(index, tileMapping);
            } else {
                tileMapping = this.planetTileMappings[index];
            }

            //building section, sending through the section x position to be placed
            GameObject section = this.sectionGen.buildSection(tileMapping, (int)(go.transform.position.x - this.sectionWidth));
            section.name = "Section-" + index;
            
            //destroy right section
            Destroy(this.sections[2].getGameObject());
            this.sections.Remove(this.sections[2]);

            //insert new section
            this.sections.Insert(0, new Section(index, section));
        } else {
            //right
            //update boundaries
            GameObject go = this.sections[2].getGameObject();
            xBoundaries[0] = go.transform.position.x - halfWidth;
            xBoundaries[1] = go.transform.position.x + halfWidth;
            
            //get index of the section to be created (wrapping if needed)
            int index = this.sections[2].getSectionId() + 1;
            index = index == this.totalSections ? 0 : index;

            //if section not in save data, create new section
            if (!this.planetTileMappings.ContainsKey(index)) {
                tileMapping = this.sectionGen.generateSection();
                this.planetTileMappings.Add(index, tileMapping);
            } else {
                tileMapping = this.planetTileMappings[index];
            }

            //building section, sending through the section x position to be placed
            GameObject section = this.sectionGen.buildSection(tileMapping, (int)(go.transform.position.x + this.sectionWidth));
            section.name = "Section-" + index;
            
            //destroy left section
            Destroy(this.sections[0].getGameObject());
            this.sections.Remove(this.sections[0]);

            //insert new section
            this.sections.Add(new Section(index, section));
        }
    }

    private void setupMapConfigurations(List<string> groupIds) {
        TextAsset textFile = Resources.Load<TextAsset>("configurations/tileMapConfigurations");

        ConfigurationLoader.MapConfiguration configuration = ConfigurationLoader.createMapConfigFromJSON(textFile.text);

        this.tileMapGenerators = ConfigurationLoader.getTileMapGenerators(configuration, groupIds);
    }
}
