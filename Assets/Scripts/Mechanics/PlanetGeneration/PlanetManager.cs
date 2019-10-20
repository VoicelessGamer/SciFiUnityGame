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
    public TileMapGenerator[] tileMapGenerators;
    public GameObject liquid;
    public int sectionsInView;

    //planet details
    [Range(5, 100)]
    public int totalSections;
    public List<Tile> tile;//temporary (replace with class that alters the 1's in the generated tile mapping array)
    [SerializeField]
    private Dictionary<int, int[,]> planetTileMappings;
    private Dictionary<int, List<Liquid>> liquids;
    
    //player deatils
    public Transform playerTransform;

    //temp details
    private LiquidGenerator liquidGen;
    private List<StandardSectionGenerator> sectionGen;
    private List<Section> sections;
    private List<Liquids> sectionedLiquids;
    private float[] xBoundaries = new float[2];

    // Start is called before the first frame update
    void Start()
    {
        this.liquids = SaveLoadManager.loadLiquids();
        this.planetTileMappings = SaveLoadManager.loadTileMappings();
        this.sections = new List<Section>();
        this.sectionedLiquids = new List<Liquids>();
        this.sectionGen = new List<StandardSectionGenerator>();
        this.liquidGen = new LiquidGenerator();
        //create a new planet generator
        //(figure out how to determine what kind later)


        foreach (TileMapGenerator tileMapGenerator in tileMapGenerators){            
            this.sectionGen.Add(new StandardSectionGenerator(sectionWidth,
            sectionHeight,
            tileMapContainer,
            sectionPrefab,
            tileMapGenerator,
            tileMapGenerator.tiles)
            
            );
        }

        //eventually array will be generated from a starting section and surrounding positions
        generateSectionsInView(0);

        SaveLoadManager.saveTileMappings(this.planetTileMappings);
        SaveLoadManager.saveLiquids(this.liquids);
    }

    public void generateSectionsInView(int centredSection) {
        float halfWidth = (this.sectionWidth / 2);

        //Currently set to 3 sections in view
        //Centred section and the map section either side of it will be generated
        int[] sectionIndices = new int[] { centredSection != 0 ? centredSection - 1 :this.totalSections - 1, centredSection, centredSection + 1 };
        int initialX = -this.sectionWidth;

        for (int i = 0; i < sectionIndices.Length; i++) {
            int[,] tileMapping;
            List<Liquid> sectionLiquids;
            List<GameObject> createdLiquids = new List<GameObject>();
            int index = sectionIndices[i];
            int r = Random.Range(0, sectionGen.Count);
            //if section not in save data, create new section
            if (!this.planetTileMappings.ContainsKey(index)) {
                
                tileMapping = this.sectionGen[r].generateSection();                
                
                this.planetTileMappings.Add(index, tileMapping);
                //generate liquids for section
                //liquids = liquidGen.GeneratorLiquid(tileMapping, this.sectionWidth * i, this.sectionWidth, this.sectionHeight, liquid);
                
            } else {
                tileMapping = this.planetTileMappings[index];
            }

            //building section, sending through the section x position to be placed
            GameObject section = this.sectionGen[r].buildSection(tileMapping, initialX + (this.sectionWidth * i));
            section.name = "Section-" + index;

            //Add liquids
            if (!this.liquids.ContainsKey(index)) {
                sectionLiquids = liquidGen.GeneratorLiquid(tileMapping, this.sectionWidth * i, this.sectionWidth, this.sectionHeight, liquid, section);
                this.liquids.Add(index, sectionLiquids);
            } else {
                sectionLiquids = this.liquids[index];
            }

            for (int liq = 0; liq < sectionLiquids.Count; liq++)
            {
                createdLiquids.Add((GameObject)Instantiate(liquid, sectionLiquids[liq].getPosition(), Quaternion.identity));
                createdLiquids[liq].GetComponent<DynamicWater>().bound.top = sectionLiquids[liq].bound.top;
                createdLiquids[liq].GetComponent<DynamicWater>().bound.right = sectionLiquids[liq].bound.right;
                createdLiquids[liq].GetComponent<DynamicWater>().bound.bottom = sectionLiquids[liq].bound.bottom;
                createdLiquids[liq].GetComponent<DynamicWater>().bound.left = sectionLiquids[liq].bound.left;
                createdLiquids[liq].GetComponent<DynamicWater>().quality = Mathf.Abs(sectionLiquids[liq].getSizeX()) * 20;
            }
            //set the boundaries, for the player to cross to load next and delete furthest sections, to the edges of the centred section
            if(index == centredSection) {
                xBoundaries[0] = section.transform.position.x - halfWidth;
                xBoundaries[1] = section.transform.position.x + halfWidth;
            }

            //store for later use (loading new sections, deleting old)
            this.sections.Add(new Section(index, section));
            this.sectionedLiquids.Add(new Liquids(index, createdLiquids));
        }
    }

    /*
    Currently set up to work with 3 sections in view.
    Needs work if that value changes
     */
    public void shiftView(int dir) {
        float halfWidth = (this.sectionWidth / 2);
        int[,] tileMapping;
        List<Liquid> sectionLiquids;
        List<GameObject> createdLiquids = new List<GameObject>();
        int r = Random.Range(0, sectionGen.Count);
        if (dir == 0) {
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
                tileMapping = this.sectionGen[r].generateSection();
                this.planetTileMappings.Add(index, tileMapping);
            } else {
                tileMapping = this.planetTileMappings[index];
            }

            //building section, sending through the section x position to be placed
            GameObject section = this.sectionGen[r].buildSection(tileMapping, (int)(go.transform.position.x - this.sectionWidth));
            section.name = "Section-" + index;

            //Add liquids
            if (!this.liquids.ContainsKey(index))
            {
                sectionLiquids = liquidGen.GeneratorLiquid(tileMapping, this.sectionWidth * 0, this.sectionWidth, this.sectionHeight, liquid, section);
                this.liquids.Add(index, sectionLiquids);
            }
            else
            {
                sectionLiquids = this.liquids[index];
            }

            for (int liq = 0; liq < sectionLiquids.Count; liq++)
            {
                createdLiquids.Add((GameObject)Instantiate(liquid, sectionLiquids[liq].getPosition(), Quaternion.identity));
                createdLiquids[liq].GetComponent<DynamicWater>().bound.top = sectionLiquids[liq].bound.top;
                createdLiquids[liq].GetComponent<DynamicWater>().bound.right = sectionLiquids[liq].bound.right;
                createdLiquids[liq].GetComponent<DynamicWater>().bound.bottom = sectionLiquids[liq].bound.bottom;
                createdLiquids[liq].GetComponent<DynamicWater>().bound.left = sectionLiquids[liq].bound.left;
                createdLiquids[liq].GetComponent<DynamicWater>().quality = Mathf.Abs(sectionLiquids[liq].getSizeX()) * 20;
            }

            //destroy right section
            Destroy(this.sections[2].getGameObject());
            this.sections.Remove(this.sections[2]);
            
            if (this.sectionedLiquids[2].getGameObject().Count > 0)
            {
                foreach (GameObject lGO in this.sectionedLiquids[2].getGameObject())
                {
                    Destroy(lGO);
                }
            }

            this.sectionedLiquids.Remove(this.sectionedLiquids[2]);

            //insert new section
            this.sections.Insert(0, new Section(index, section));
            this.sectionedLiquids.Insert(0, new Liquids(index, createdLiquids));
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
                tileMapping = this.sectionGen[r].generateSection();
                this.planetTileMappings.Add(index, tileMapping);
            } else {
                tileMapping = this.planetTileMappings[index];
            }

            //building section, sending through the section x position to be placed
            GameObject section = this.sectionGen[r].buildSection(tileMapping, (int)(go.transform.position.x + this.sectionWidth));
            section.name = "Section-" + index;

            //Add liquids
            if (!this.liquids.ContainsKey(index))
            {
                sectionLiquids = liquidGen.GeneratorLiquid(tileMapping, this.sectionWidth * 2, this.sectionWidth, this.sectionHeight, liquid, section);
                this.liquids.Add(index, sectionLiquids);
            }
            else
            {
                sectionLiquids = this.liquids[index];
            }

            for (int liq = 0; liq < sectionLiquids.Count; liq++)
            {
                createdLiquids.Add((GameObject)Instantiate(liquid, sectionLiquids[liq].getPosition(), Quaternion.identity));
                createdLiquids[liq].GetComponent<DynamicWater>().bound.top = sectionLiquids[liq].bound.top;
                createdLiquids[liq].GetComponent<DynamicWater>().bound.right = sectionLiquids[liq].bound.right;
                createdLiquids[liq].GetComponent<DynamicWater>().bound.bottom = sectionLiquids[liq].bound.bottom;
                createdLiquids[liq].GetComponent<DynamicWater>().bound.left = sectionLiquids[liq].bound.left;
                createdLiquids[liq].GetComponent<DynamicWater>().quality = Mathf.Abs(sectionLiquids[liq].getSizeX()) * 20;
            }

            //destroy left section
            Destroy(this.sections[0].getGameObject());
            this.sections.Remove(this.sections[0]);

            if (this.sectionedLiquids[0].getGameObject().Count > 0)
            {
                foreach (GameObject lGO in this.sectionedLiquids[0].getGameObject())
                {
                    Destroy(lGO);
                }
            }

            this.sectionedLiquids.Remove(this.sectionedLiquids[0]);

            //insert new section
            this.sections.Add(new Section(index, section));
            this.sectionedLiquids.Add(new Liquids(index, createdLiquids));
        }
    }

    void Update() {
        //builds and delete sections when the player crosses a boundary
        if(playerTransform.position.x < xBoundaries[0]) {
            shiftView(0);
        } else if(playerTransform.position.x > xBoundaries[1]) {
            shiftView(1);
        }
    }
}
