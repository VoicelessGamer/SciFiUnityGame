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
        List<Liquid> liquids = new List<Liquid>();
        List<Liquid> tempLiquids = new List<Liquid>();
        List<Vector2> tempSizes = new List<Vector2>();
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
                Vector3Int localPlaceMinus1Y = (new Vector3Int(n, p + 1, (int)tileMap.transform.position.y));
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

                        float restDepth = globalPlace.y;

                        //check for depth space
                        for (int d = 0; d < depth; d++)
                        {
                            Vector3Int localPlacePlusLengthDepth = (new Vector3Int(n + l, p + d, (int)tileMap.transform.position.y));
                            if (d + globalPlace.y < sectionHeight)
                            {
                                if (tileMap.HasTile(localPlacePlusLengthDepth) && restDepth == globalPlace.y)
                                {
                                    if (deepestLand < globalPlace.y + d)
                                    {
                                        deepestLand = globalPlace.y + d;
                                        restDepth = globalPlace.y + d;
                                    }
                                }
                            }
                        }
                    }

                    float liqWidth = globalPlace.x - firstLand;
                    float liqHeight = globalPlace.y - deepestLand;

                    //length and depth are at least > 2 gap
                    if (Mathf.Abs(liqWidth) > 1 && Mathf.Abs(liqHeight) > 1)
                    {
                        //bool canPlace = Random.Range(0, 100) < getScaledPlacementChance(y, x) ? true : false;
                        //if (canPlace)
                        //{
                        bool cantPlace = false;
                        for (int q = 0; q < tempLiquids.Count; q++)
                        {
                            if (tempLiquids[q].getPosition().x < globalPlace.x + 1 || firstLand < tempLiquids[q].getSizeX())
                                if (tempLiquids[q].getPosition().y < globalPlace.y || deepestLand < tempLiquids[q].getSizeY())
                                    cantPlace = true;
                        }
                        if (!cantPlace)
                        {

                            Liquid newLiquid = new Liquid(new Vector3((int)globalPlace.x - ((int)liqWidth / 2), (int)globalPlace.y - ((int)liqHeight / 2)), (int)liqWidth, (int)liqHeight, liquid);
                            liquids.Add(newLiquid);
                            tempLiquids.Add(newLiquid);
                        }
                        //}
                    }

                }
            }
        }

        /*for (int x = 1; x < mapLength - length; x++)
        {
            for (int y = 0; y < sectionHeight; y++)
            {
                //check that there was land before it
                if (tileMapping[x, y] == 0 && tileMapping[x - 1, y] == 1)
                {
                    int firstLand = x ;
                    int deepestLand = y;
                    for (int l = 0; l < length; l++)
                    {
                        if (l + x < mapLength)
                        {
                            if (tileMapping[x + l, y] == 1 && firstLand == x)
                            {
                                firstLand = x + l;
                            }
                        }
                        int restDepth = y;
                        //check for depth space
                        for (int d = 0; d < depth; d++)
                        {
                            if (d + y < sectionHeight)
                            {
                                if (tileMapping[x + l, y + d] == 1 && restDepth == y)
                                {
                                    if (deepestLand < y + d)
                                    {
                                        deepestLand = y + d;
                                        restDepth = y + d;
                                    }
                                }
                            }
                        }

                    }
                    int liqWidth = x - firstLand;
                    int liqHeight = (y + this.initialY) - deepestLand;

                    //length and depth are at least > 2 gap
                    if (Mathf.Abs(liqWidth) > 1 && Mathf.Abs(liqHeight) > 1 )
                    {
                        //bool canPlace = Random.Range(0, 100) < getScaledPlacementChance(y, x) ? true : false;
                        //if (canPlace)
                        //{
                        bool cantPlace = false;
                        for (int q = 0; q < tempLiquids.Count; q++)
                        {   if (tempLiquids[q].getPosition().x < x + 1 || firstLand < tempLiquids[q].getSizeX())
                                if (tempLiquids[q].getPosition().y < y || deepestLand < tempLiquids[q].getSizeY())
                                    cantPlace = true;
                        }
                        if (!cantPlace)
                        {
                            
                            Liquid newLiquid = new Liquid(new Vector3(((x + (sectionWidth / 2)) - this.initialX) + liqWidth/2, (this.initialY - y) - liqHeight/2), liqWidth, liqHeight, liquid);
                            liquids.Add(newLiquid);
                            tempLiquids.Add(newLiquid);
                        }
                        //}
                    }
                    
                }

            }

        }*/

        return liquids;
    }

    public float getScaledPlacementChance(int y, int height)
    {
        placementScaling = 1;
        placementChance = 69;
        //placement chance - (percentage of distance of row from top) * placement scaling value
        return this.placementChance - ((((float)height - (float)y) / height) * this.placementChance) * this.placementScaling;
    }
}
