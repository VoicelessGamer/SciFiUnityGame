using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Mechanics.TileMapGen {
    /* 
        Picks a row for each column based on weighted values. All positions beneath the chosen
        row (including selected row) are including in the generated map
     */
    [System.Serializable]
    public class SimpleTileMapGenerator : TileMapGenerator
    {

        //placement chance for each position on the bottom row
        [Range(0,100)]
        public int placementChance;
        
        public WeightedInteger[] weightedValues;

        public override int[,] generateMap(int width, int height) {

            int[,] tileMapping = new int[width, height];

            for(int x = 0; x < width; x++) {
                bool canPlace = Random.Range(0, 100) < this.placementChance ? true : false;
                if(!canPlace) {
                    continue;
                }
                //int y = (int)Mathf.Floor(Random.Range(0, height));
                int y = ((WeightedInteger)WeightedValueSelector.selectValue(weightedValues)).getValue();
                for(; y < height; y++) {
                    tileMapping[x,y] = 1;
                }
            }

            return tileMapping;
        }
    }
}