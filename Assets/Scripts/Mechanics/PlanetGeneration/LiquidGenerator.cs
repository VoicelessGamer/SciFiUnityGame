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
        List<Liquid> tempLiquids = new List<Liquid>();
        List<Vector2> tempSizes = new List<Vector2>();
        int depth = 15;
        int length = 15;
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
