using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LiquidGenerator : MonoBehaviour
{
    protected int placementScaling;
    protected int placementChance;
    protected int initialX;
    protected int initialY;
    public List<GameObject> GeneratorLiquid(int[,] tileMapping, int sectionWidth, int mapLength, int sectionHeight, GameObject liquid, GameObject section)
    {
        //List<Liquid> liquids = new List<Liquid>();
        List<Liquid> tempLiquids = new List<Liquid>();
        List<Vector2> tempSizes = new List<Vector2>();
        List<GameObject> liquids = new List<GameObject>();
        int depth = 15;
        int length = 15;
        this.initialX = (int)(mapLength * 0.5f);
        this.initialY = (int)(sectionHeight * 0.5f);
        //temporary choose weighting of liquid...this needs to be in config for liquids or biomes

        //look for first 1 (land tile)
        Tilemap tileMap = section.GetComponentInParent<Tilemap>();

        for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++)
        {
            for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
                Vector3Int localPlacePlus1X = (new Vector3Int(n+1, p, (int)tileMap.transform.position.y));
                Vector3Int localPlaceMinus1X = (new Vector3Int(n - 1, p, (int)tileMap.transform.position.y));
                Vector3Int localPlacePlus1Y = (new Vector3Int(n, p +1, (int)tileMap.transform.position.y));
                Vector3Int localPlaceMinus1Y = (new Vector3Int(n, p - 1, (int)tileMap.transform.position.y));
                Vector3 globalPlace = tileMap.CellToWorld(localPlace);
                if (!tileMap.HasTile(localPlace) && tileMap.HasTile(localPlaceMinus1X))
                {
                    float firstLand = globalPlace.x;
                    float deepestLand = globalPlace.y;
                    for (int l = 0; l < length; l++)
                    {
                        Vector3Int localPlacePlusLength = (new Vector3Int(n + l, p, (int)tileMap.transform.position.y));
                        if (l + globalPlace.x < mapLength)
                        {
                            if (tileMap.HasTile(localPlacePlusLength) && firstLand == globalPlace.x)
                            {
                                firstLand = globalPlace.x + l;
                            }
                        }

                        //check for depth space
                        for (int d = 0; d < depth; d++)
                        {
                            Vector3Int localPlacePlusLengthDepth = (new Vector3Int(n + l, p - d, (int)tileMap.transform.position.y));
                            Vector3Int localPlaceDepth = (new Vector3Int(n + l, p - d + 1, (int)tileMap.transform.position.y));
                            if (d + globalPlace.y < sectionHeight)
                            {
                                if (tileMap.HasTile(localPlacePlusLengthDepth) && !tileMap.HasTile(localPlaceDepth))
                                {
                                    if (deepestLand < globalPlace.y + d)
                                    {
                                        deepestLand = globalPlace.y + d;
                                    }
                                }
                            }
                        }
                    }

                    float liqWidth = -globalPlace.x + firstLand;
                    float liqHeight = -globalPlace.y + deepestLand;

                    //length and depth are at least > 2 gap
                    if (Mathf.Abs(liqWidth) > 2 && Mathf.Abs(liqHeight) > 1)
                    {
                        //bool canPlace = Random.Range(0, 100) < getScaledPlacementChance(y, x) ? true : false;
                        //if (canPlace)
                        //{
                        bool cantPlace = false;
                        if (liquids.Count > 0)
                            cantPlace = preventSpawn(globalPlace.x + ((int)liqWidth / 2), globalPlace.y, liquids);

                        /*for (int q = 0; q < tempLiquids.Count; q++)
                        {
                            if (tempLiquids[q].getPosition().x == globalPlace.x + ((int)liqWidth / 2) && tempLiquids[q].getPosition().x + tempLiquids[q].getSizeX() >= (globalPlace.x + ((int)liqWidth / 2)) + liqWidth)
                                if (tempLiquids[q].getPosition().y == globalPlace.y || deepestLand <= tempLiquids[q].getPosition().y + tempLiquids[q].getSizeY())
                                    cantPlace = true;
                        }*/
                        if (!cantPlace)
                        {

                            Liquid newLiquid = new Liquid(new Vector3((int)globalPlace.x + ((int)liqWidth/2), (int)globalPlace.y), (int)liqWidth, (int)liqHeight, liquid);
                                                        

                            liquids.Add((GameObject)Instantiate(liquid, newLiquid.getPosition(), Quaternion.identity));
                            liquids[liquids.Count-1].GetComponent<DynamicWater>().bound.top = newLiquid.getSizeY() / 2;
                            liquids[liquids.Count-1].GetComponent<DynamicWater>().bound.right = newLiquid.getSizeX() / 2;
                            liquids[liquids.Count-1].GetComponent<DynamicWater>().bound.bottom = -newLiquid.getSizeY() / 2;
                            liquids[liquids.Count-1].GetComponent<DynamicWater>().bound.left = -newLiquid.getSizeX() / 2;
                            liquids[liquids.Count-1].GetComponent<DynamicWater>().quality = Mathf.Abs(newLiquid.getSizeX()) * 20;
                        }
                        //}
                    }

                }
            }
        }

        return liquids;
    }

    bool preventSpawn(float x, float y, List<GameObject> liquids)
    {
        Vector3 spawnPos = new Vector3(x, y);
        
        for (int q = 0; q < liquids.Count; q++)
        {
            float leftExtent = liquids[q].transform.position.x - liquids[q].GetComponent<DynamicWater>().bound.left;
            float rightExtent = liquids[q].transform.position.x + liquids[q].GetComponent<DynamicWater>().bound.right;
            float lowerExtent = liquids[q].transform.position.y - liquids[q].GetComponent<DynamicWater>().bound.bottom;
            float upperExtent = liquids[q].transform.position.y + liquids[q].GetComponent<DynamicWater>().bound.top;

            if(spawnPos.x > leftExtent && spawnPos.x <= rightExtent)
            {
                if(spawnPos.y >= lowerExtent && spawnPos.y <= upperExtent)
                {
                    return true;
                }
            }

        }
        return false;
    }

    public float getScaledPlacementChance(int y, int height)
    {
        placementScaling = 1;
        placementChance = 69;
        //placement chance - (percentage of distance of row from top) * placement scaling value
        return this.placementChance - ((((float)height - (float)y) / height) * this.placementChance) * this.placementScaling;
    }
}
