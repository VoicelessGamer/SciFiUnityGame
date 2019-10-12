using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.TileMapGen {
    [System.Serializable]
    public class RelativePathTileMapGenerator : TileMapGenerator {

         public int variation;
         public bool fillGround;

        public RelativePathTileMapGenerator() {
        }

        public RelativePathTileMapGenerator(int variation, bool fillGround) {
            this.variation = variation;
            this.fillGround = fillGround;
        }

        public override int[,] generateMap(int width, int height) {
            return generateMap(width, height, null, null);
        }

        public override int[,] generateMap(int width, int height, int[,] leftSideMapping, int[,] rightSideMapping) {
            int[,] tileMapping = new int[width, height];

            int currentY = leftSideMapping != null ? getYPosition(getArrayRow(leftSideMapping, leftSideMapping.GetLength(0) - 1)) : (int)Mathf.Round(Random.Range(0.0f, (float)(height - 1)));
            int targetRight = rightSideMapping != null ? getYPosition(getArrayRow(rightSideMapping, 0)) : (int)Mathf.Round(Random.Range(0.0f, (float)(height - 1)));

            int deviation;

            for(int i = 0; i < width; i++) {
                if(fillGround) {
                    for(int j = currentY; j < height; j++) {
                        tileMapping[i, j] = 1;
                    }
                } else {
                    tileMapping[i, currentY] = 1;
                }

                deviation = targetRight - currentY;

                //check whether path needs to start building towards the target
                if((width - (i + 1)) <= Mathf.Abs(deviation)) {
                    if(deviation < 0) {
                        currentY--;
                    } else {
                        currentY++;
                    }
                } else {
                    //calculate the available bounds to choose from based on relative position of last placed                  
                    int lowerBound = currentY - variation;
                    int upperBound = currentY + variation;
                    lowerBound = lowerBound >= 0 ? lowerBound : 0;
                    upperBound = upperBound < height ? upperBound : height - 1;

                    currentY = (int)Mathf.Round(Random.Range((float)lowerBound, (float)upperBound));
                }                
            }

            return tileMapping;
        }

        private int[] getArrayRow(int[,] multiArray, int rowIndex) {
            int[] arraySubset = new int[multiArray.GetLength(1)];

            for(int i = 0; i < multiArray.GetLength(1); i++) {
                arraySubset[i] = multiArray[rowIndex, i];
            }

            return arraySubset;
        }

        private int getYPosition(int[] column) {
            for(int i = 0; i < column.GetLength(0); i++) {
                if(column[i] != 0) {
                    return i;
                }
            }

            return -1;
        }
    }
}