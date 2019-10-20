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
    public List<Liquid> GeneratorLiquid(int[,] tileMapping, int sectionWidth, int mapLength, int sectionHeight, GameObject liquid, GameObject section)
    {
        List<Vector2> tempSizes = new List<Vector2>();
        List<Liquid> liquids = new List<Liquid>();
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
                                firstLand = globalPlace.x + l + 1;
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
                                    if (deepestLand > globalPlace.y - d)
                                    {
                                        deepestLand = globalPlace.y - d;
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
                            cantPlace = preventSpawn(globalPlace.x + ((int)liqWidth / 2), globalPlace.y, liquids, liqWidth, liqHeight);

                        if (!cantPlace)
                        {
                            Liquid.Bound bound;
                            bound.top = -(int)liqHeight / 2;
                            bound.right = (int)liqWidth / 2;
                            bound.bottom = +(int)liqHeight / 2;
                            bound.left = -(int)liqWidth / 2;
                            Liquid newLiquid = new Liquid(new Vector3((int)globalPlace.x + ((int)liqWidth/2), (int)globalPlace.y), (int)liqWidth, (int)liqHeight, liquid, bound);
                            liquids.Add(newLiquid);
                            
                        }                        
                    }

                }
            }
        }

        return liquids;
    }

    bool preventSpawn(float x, float y, List<Liquid> liquids, float newWidth, float newHeight)
    {
        Vector3 spawnPos = new Vector3(x, y);
        
        for (int q = 0; q < liquids.Count; q++)
        {

            float leftExtent = liquids[q].getPosition().x + liquids[q].bound.left;
            float rightExtent = liquids[q].getPosition().x + liquids[q].bound.right;
            float lowerExtent = liquids[q].getPosition().y + liquids[q].bound.bottom;
            float upperExtent = liquids[q].getPosition().y + liquids[q].bound.top;

            if(spawnPos.x >= leftExtent && spawnPos.x <= rightExtent)
            {
                if(spawnPos.y >= lowerExtent && spawnPos.y <= upperExtent)
                {
                    return true;
                }
            }

            bool inWidthSpace = false;
            bool inHeightSpace = false;
            for (int w = 0; w < newWidth; w++)
            {
                if (spawnPos.x + x >= leftExtent && spawnPos.x + x <= rightExtent)
                {
                    inWidthSpace = true;
                    break;
                }
            }
            for (int h = 0; h < newHeight; h++)
            {
                if (spawnPos.y + y >= lowerExtent && spawnPos.y + y <= upperExtent)
                {
                    inHeightSpace = true;
                    break;
                }
            }

            if (inWidthSpace && inHeightSpace)
            {
                return true;
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
