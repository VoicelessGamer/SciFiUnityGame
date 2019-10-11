using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidGenerator : MonoBehaviour
{
    protected int placementScaling;
    protected int placementChance;
    public List<Liquid> GeneratorLiquid(int[,] tileMapping, int sectionWidth, int sectionHeight, GameObject liquid)
    {
        List<Liquid> liquids = new List<Liquid>();
        int depth = 3;
        int length = 3;
        
        //temporary choose weighting of liquid...this needs to be in config for liquids or biomes

        //look for first 1 (land tile)

        for (int x = 1; x < sectionWidth; x++)
        {
            for (int y = 0; y < sectionHeight; y++)
            {
                //check that there was land before it
                if (tileMapping[x, y] == 0 && tileMapping[x - 1, y] == 1)
                {
                    int firstLand = 0 ;
                    int deepestLand = 0;
                    for (int l = 0; l < length; l++)
                    {
                        if (tileMapping[x + l, y] == 1)
                        {
                            firstLand = x + l;
                        }

                        //check for depth space
                        for (int d = 0; d < depth; d++)
                        {
                            if (tileMapping[x + l, y + d] == 1)
                            {
                                if (deepestLand < y + d) {
                                    deepestLand = y + d;
                                }
                            }
                        }

                    }
                    //length and depth are at least > 2 gap
                    if (firstLand > x + 2 && deepestLand > y + 2 )
                    {
                        bool canPlace = Random.Range(0, 100) < getScaledPlacementChance(y, x) ? true : false;
                        if (canPlace)
                        {
                            Liquid newLiquid = new Liquid(new Vector3(x + 1, y), firstLand, deepestLand, liquid);
                            liquids.Add(newLiquid);
                        }
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
