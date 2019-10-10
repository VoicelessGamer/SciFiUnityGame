using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.TileMapGen {
    [System.Serializable]
    public class RelativePathTileMapGenerator : TileMapGenerator {

         public int variation;

        public RelativePathTileMapGenerator() {
        }

        public RelativePathTileMapGenerator(int variation) {
            this.variation = variation;
        }

        public override int[,] generateMap(int width, int height) {
            return generateMap(width, height, null, null);
        }

        public override int[,] generateMap(int width, int height, int[,] leftSideMapping, int[,] rightSideMapping) {
            int[,] tileMapping = new int[width, height];

            int targetLeft = leftSideMapping != null ? getYPosition(leftSideMapping[leftSideMapping.GetLength(0) - 1]) : -1;
            int targetRight = rightSideMapping != null ? getYPosition(rightSideMapping[0]) : -1;

            int currentY = targetLeft != -1 ? targetLeft : (int)Random.Range(0, height);

            int deviation = targetRight == -1 ? -1 : targetRight - currentY;

            for(int i = 0; i < width; i++) {
                tileMapping[i, currentY] = 1;

                if (deviation == -1) {
                    currentY = Random.Range(currentY - variation, currentY + variation);
                    continue;
                } else {
                    deviation = targetRight - currentY;

                    if((width - i) <= Mathf.Abs(deviation)) {
                        if(deviation < 0) {
                            currentY--;
                        } else {
                            currentY++;
                        }
                    }
                }
            }

            return tileMapping;
        }

        public int getYPosition(int[] column) {
            for(int i = 0; i < column.GetLength(0); i++) {
                if(column[i] != 0) {
                    return i;
                }
            }

            return -1;
        }
    }
}