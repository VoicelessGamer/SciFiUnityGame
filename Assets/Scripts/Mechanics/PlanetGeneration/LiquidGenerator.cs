using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidGenerator : MonoBehaviour
{
    protected int placementScaling;
    protected int placementChance;
    protected int initialX;
    protected int initialY;
    public List<Liquid> GeneratorLiquid(int[,] tileMapping, int sectionWidth, int mapLength, int sectionHeight, GameObject liquid)
    {
        List<Liquid> liquids = new List<Liquid>();
        List<Vector2> tempLiquids = new List<Vector2>();
        List<Vector2> tempSizes = new List<Vector2>();
        int depth = 5;
        int length = 5;
        this.initialX = (int)(mapLength * 0.5f);
        this.initialY = (int)(sectionHeight * 0.5f);
        //temporary choose weighting of liquid...this needs to be in config for liquids or biomes

        //look for first 1 (land tile)

        for (int x = 1; x < mapLength - length; x++)
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
                        if (tileMapping[x + l, y] == 1 && firstLand == x)
                        {
                            firstLand = x + l;
                        }
                        int restDepth = y;
                        //check for depth space
                        for (int d = 0; d < depth; d++)
                        {
                            if (tileMapping[x + l, y + d] == 1 && restDepth == y)
                            {
                                if (deepestLand < y + d) {
                                    deepestLand = y + d;
                                    restDepth = y + d;
                                }
                            }
                        }

                    }
                    //length and depth are at least > 2 gap
                    if (firstLand > x + 1 && deepestLand > y + 1 )
                    {
                        //bool canPlace = Random.Range(0, 100) < getScaledPlacementChance(y, x) ? true : false;
                        //if (canPlace)
                        //{
                        bool cantPlace = false;
                        for (int q = 0; q < tempLiquids.Count; q++)
                        {   if (tempLiquids[q].x < x + 1 || firstLand < tempSizes[q].x)
                                if (tempLiquids[q].y < y || deepestLand < tempSizes[q].y)
                                    cantPlace = true;
                        }
                        if (!cantPlace)
                        {
                            Liquid newLiquid = new Liquid(new Vector3(x - sectionWidth  + this.initialX + (this.initialX/2), this.initialY - y), firstLand - x, deepestLand - y, liquid);
                            liquids.Add(newLiquid);
                            tempLiquids.Add(new Vector2(x, y + 1));
                            tempSizes.Add(new Vector2(firstLand - x, deepestLand - y));
                        }
                        //}
                    }
                    
                }

            }

        }
        
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
