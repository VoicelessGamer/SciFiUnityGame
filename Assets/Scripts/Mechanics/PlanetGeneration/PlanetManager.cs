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

    //background
    public List<Sprite> sbackgrounds;
    public GameObject camera;
    public Material mat;
    public GameObject backgroundPrefab;
    private List<GameObject> backgrounds;

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
        tileMapGenerators = new List<TileMapGenerator>();
        setupMapConfigurations(new List<string>(){"simpleTest2"});

        tiles = new Dictionary<int, Tile>(){{1, dirtTile},{2, grassTile}};

        sections = new List<Section>();

        sectionBuilder = SectionBuilderFactory.getSectionBuilder("STANDARD");

        tileMapAlteration = TileMapAlterationFactory.getSectionBuilder("TOP_LAYER");

        planetTileMappings = SaveLoadManager.loadTileMappings();

        //background
        backgrounds = new List<GameObject>();
        BackgroundGenerator backgroundGenerator = new BackgroundGenerator();
        backgrounds = backgroundGenerator.backgroundGen(sbackgrounds, camera, false, true, mat, backgroundPrefab);

        //eventually array will be generated from a starting section and surrounding positions
        generateSectionsInView(0);

        SaveLoadManager.saveTileMappings(planetTileMappings);
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
        float halfWidth = (sectionWidth / 2);

        //Currently set to 3 sections in view
        //Centred section and the map section either side of it will be generated
        int[] sectionIndices = new int[] { 
            centredSection != 0 ? centredSection - 1 : totalSections - 1, 
            centredSection, 
            centredSection != totalSections - 1 ? centredSection + 1 : 0};

        int initialX = -sectionWidth;

        for (int i = 0; i < sectionIndices.Length; i++) {
            int[,] tileMapping;
            int index = sectionIndices[i];
            //if section not in save data, create new section
            if (!planetTileMappings.ContainsKey(index)) {
                generateSection(index);
            }
            tileMapping = planetTileMappings[index];

            //building section
            //Generate in instantiated clone of the section prefab
            GameObject instantiatedSection = (GameObject)Instantiate(sectionPrefab, new Vector3(initialX + (sectionWidth * i), 0, 0), Quaternion.identity, tileMapContainer.transform);
            sectionBuilder.buildSection(tileMapping, instantiatedSection, tiles);
            instantiatedSection.name = "Section-" + index;

            //set the boundaries, for the player to cross to load next and delete furthest sections, to the edges of the centred section
            if(index == centredSection) {
                xBoundaries[0] = instantiatedSection.transform.position.x - halfWidth;
                xBoundaries[1] = instantiatedSection.transform.position.x + halfWidth;
            }

            //store for later use (loading new sections, deleting old)
            sections.Add(new Section(index, instantiatedSection));
        }
    }

    /*
    Currently set up to work with 3 sections in view.
    Needs work if that value changes
     */
    public void shiftView(int dir) {
        float halfWidth = (sectionWidth / 2);
        int[,] tileMapping;
        
        if(dir == 0) {
            //left
            //update boundaries
            GameObject go = sections[0].getGameObject();
            xBoundaries[0] = go.transform.position.x - halfWidth;
            xBoundaries[1] = go.transform.position.x + halfWidth;
            
            //get index of the section to be created (wrapping if needed)
            int index = sections[0].getSectionId() - 1;
            index = index == -1 ? totalSections - 1 : index;

            //if section not in save data, create new section
            if (!planetTileMappings.ContainsKey(index)) {
                generateSection(index);
            }
            tileMapping = planetTileMappings[index];

            //building section
            GameObject instantiatedSection = (GameObject)Instantiate(sectionPrefab, new Vector3((int)(go.transform.position.x - sectionWidth), 0, 0), Quaternion.identity, tileMapContainer.transform);
            sectionBuilder.buildSection(tileMapping, instantiatedSection, tiles);
            instantiatedSection.name = "Section-" + index;
            
            //destroy right section
            Destroy(sections[2].getGameObject());
            sections.Remove(sections[2]);

            //insert new section
            sections.Insert(0, new Section(index, instantiatedSection));
        } else {
            //right
            //update boundaries
            GameObject go = sections[2].getGameObject();
            xBoundaries[0] = go.transform.position.x - halfWidth;
            xBoundaries[1] = go.transform.position.x + halfWidth;
            
            //get index of the section to be created (wrapping if needed)
            int index = sections[2].getSectionId() + 1;
            index = index == totalSections ? 0 : index;

            //if section not in save data, create new section
            if (!planetTileMappings.ContainsKey(index)) {
                generateSection(index);
            }
            tileMapping = planetTileMappings[index];

            //building section
            GameObject instantiatedSection = (GameObject)Instantiate(sectionPrefab, new Vector3((int)(go.transform.position.x + sectionWidth), 0, 0), Quaternion.identity, tileMapContainer.transform);
            sectionBuilder.buildSection(tileMapping, instantiatedSection, tiles);
            instantiatedSection.name = "Section-" + index;
            
            //destroy left section
            Destroy(sections[0].getGameObject());
            sections.Remove(sections[0]);

            //insert new section
            sections.Add(new Section(index, instantiatedSection));
        }
    }

    private void generateSection(int index) {
        int leftIndex = index != 0 ? index - 1 : totalSections - 1;
        int rightIndex = index != totalSections - 1 ? index + 1 : 0;

        int[,] tileMapping = sectionBuilder.generateSection(sectionWidth, 
            sectionHeight, 
            tileMapGenerators[(int)Random.Range(0, tileMapGenerators.Count)],
            planetTileMappings.ContainsKey(leftIndex) ? planetTileMappings[leftIndex] : null,
            planetTileMappings.ContainsKey(rightIndex) ? planetTileMappings[rightIndex] : null);

        tileMapAlteration.alterMap(tileMapping);

        planetTileMappings.Add(index, tileMapping);
    }

    private void setupMapConfigurations(List<string> groupIds) {
        TextAsset textFile = Resources.Load<TextAsset>("configurations/tileMapConfigurations");

        ConfigurationLoader.MapConfiguration configuration = ConfigurationLoader.createMapConfigFromJSON(textFile.text);

        tileMapGenerators = ConfigurationLoader.getTileMapGenerators(configuration, groupIds);
    }
}
