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

    //planet details
    [Range(5, 100)]
    public int totalSections;
    public Tile dirtTile;//temporary (replace with class that alters the 1's in the generated tile mapping array)
    public Tile grassTile;//temporary (replace with class that alters the 1's in the generated tile mapping array)
    public Dictionary<int, Tile> tiles;

    [SerializeField]
    private Dictionary<int, int[,]> planetTileMappings;

    //player deatils
    public Transform playerTransform;
    private float[] xBoundaries = new float[2];

    //other details
    private List<Section> sections;
    private List<TileMapGenerator> tileMapGenerators;
    private SectionBuilder sectionBuilder;
    private TileMapAlteration tileMapAlteration;

    // Start is called before the first frame update
    void Start() {
        //setup map configurations
        this.tileMapGenerators = new List<TileMapGenerator>();
        setupMapConfigurations(new List<string>(){"simpleTest"});

        tiles = new Dictionary<int, Tile>(){{1, dirtTile},{2, grassTile}};

        this.sections = new List<Section>();

        sectionBuilder = SectionBuilderFactory.getSectionBuilder("STANDARD");

        tileMapAlteration = TileMapAlterationFactory.getSectionBuilder("TOP_LAYER");

        this.planetTileMappings = SaveLoadManager.loadTileMappings();

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
        int[] sectionIndices = new int[] { 
            centredSection != 0 ? centredSection - 1 : this.totalSections - 1, 
            centredSection, 
            centredSection != this.totalSections - 1 ? centredSection + 1 : 0};

        int initialX = -this.sectionWidth;

        for (int i = 0; i < sectionIndices.Length; i++) {
            int[,] tileMapping;
            int index = sectionIndices[i];
            //if section not in save data, create new section
            if (!this.planetTileMappings.ContainsKey(index)) {
                generateSection(index);
            }
            tileMapping = this.planetTileMappings[index];

            //building section
            //Generate in instantiated clone of the section prefab
            GameObject instantiatedSection = (GameObject)Instantiate(sectionPrefab, new Vector3(initialX + (this.sectionWidth * i), 0, 0), Quaternion.identity, this.tileMapContainer.transform);
            sectionBuilder.buildSection(tileMapping, instantiatedSection, tiles);
            instantiatedSection.name = "Section-" + index;

            //set the boundaries, for the player to cross to load next and delete furthest sections, to the edges of the centred section
            if(index == centredSection) {
                xBoundaries[0] = instantiatedSection.transform.position.x - halfWidth;
                xBoundaries[1] = instantiatedSection.transform.position.x + halfWidth;
            }

            //store for later use (loading new sections, deleting old)
            this.sections.Add(new Section(index, instantiatedSection));
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
                generateSection(index);
            }
            tileMapping = this.planetTileMappings[index];

            //building section
            GameObject instantiatedSection = (GameObject)Instantiate(sectionPrefab, new Vector3((int)(go.transform.position.x - this.sectionWidth), 0, 0), Quaternion.identity, this.tileMapContainer.transform);
            sectionBuilder.buildSection(tileMapping, instantiatedSection, tiles);
            instantiatedSection.name = "Section-" + index;
            
            //destroy right section
            Destroy(this.sections[2].getGameObject());
            this.sections.Remove(this.sections[2]);

            //insert new section
            this.sections.Insert(0, new Section(index, instantiatedSection));
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
                generateSection(index);
            }
            tileMapping = this.planetTileMappings[index];

            //building section
            GameObject instantiatedSection = (GameObject)Instantiate(sectionPrefab, new Vector3((int)(go.transform.position.x + this.sectionWidth), 0, 0), Quaternion.identity, this.tileMapContainer.transform);
            sectionBuilder.buildSection(tileMapping, instantiatedSection, tiles);
            instantiatedSection.name = "Section-" + index;
            
            //destroy left section
            Destroy(this.sections[0].getGameObject());
            this.sections.Remove(this.sections[0]);

            //insert new section
            this.sections.Add(new Section(index, instantiatedSection));
        }
    }

    private void generateSection(int index) {
        int leftIndex = index != 0 ? index - 1 : this.totalSections - 1;
        int rightIndex = index != this.totalSections - 1 ? index + 1 : 0;

        int[,] tileMapping = sectionBuilder.generateSection(this.sectionWidth, 
            this.sectionHeight, 
            tileMapGenerators[(int)Random.Range(0, tileMapGenerators.Count)],
            this.planetTileMappings.ContainsKey(leftIndex) ? this.planetTileMappings[leftIndex] : null,
            this.planetTileMappings.ContainsKey(rightIndex) ? this.planetTileMappings[rightIndex] : null);

        tileMapAlteration.alterMap(tileMapping);

        this.planetTileMappings.Add(index, tileMapping);
    }

    private void setupMapConfigurations(List<string> groupIds) {
        TextAsset textFile = Resources.Load<TextAsset>("configurations/tileMapConfigurations");

        ConfigurationLoader.MapConfiguration configuration = ConfigurationLoader.createMapConfigFromJSON(textFile.text);

        this.tileMapGenerators = ConfigurationLoader.getTileMapGenerators(configuration, groupIds);
    }
}
