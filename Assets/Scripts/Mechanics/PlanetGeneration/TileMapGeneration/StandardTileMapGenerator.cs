﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.TileMapGen {
    /*
        Generates a map from bottom left to top right. First row is generated using just the placement chance.
        Each placed tile must be connected to a previously placed tile.
     */
    [System.Serializable]
    public class StandardTileMapGenerator : TileMapGenerator {

        //placement chance for each position on the bottom row
        [SerializeField]
        private int placementChance;
        //true if the tile space below must be occupied
        [SerializeField]
        private bool requireConnection;
        //scaling vlaue for the placement chance per row
        [SerializeField]
        private float placementScaling;

        public StandardTileMapGenerator() {
            placementChance = 100;
            requireConnection = true;
            placementScaling = 0.1f;
        }

        public StandardTileMapGenerator(int placementChance, bool requireConnection, float placementScaling) {
            this.placementChance = placementChance;
            this.requireConnection = requireConnection;
            this.placementScaling = placementScaling;
        }

        public override int[,] generateMap(int width, int height) {

            int[,] tileMapping = new int[width, height];

            for(int y = height - 1; y >= 0; y--) {
                for(int x = 0; x < width; x++) {
                    //providing not bottom row, check tile can be placed
                    bool canPlace = y < (height - 1) ? canPlaceTile(x, y, width, height, tileMapping) : true;
                    //if tile can be placed, check if the tile will be placed based on a scaling value
                    if(canPlace) {
                        canPlace = Random.Range(0, 100) < getScaledPlacementChance(y, height) ? true : false;
                    }
                    tileMapping[x,y] = canPlace == true ? 1 : 0;
                }
            }

            return tileMapping;
        }
        public override int[,] generateMap(int width, int height, int[,] leftSideMapping, int[,] rightSideMapping) {
            return generateMap(width, height);
        }

        public float getScaledPlacementChance(int y, int height) {
            //placement chance - (percentage of distance of row from top) * placement scaling value
            return placementChance - ((((float)height - (float)y) / height) * placementChance) * placementScaling;
        }

        public bool canPlaceTile(int x, int y, int width, int height, int[,] tileMapping) {
            if(!requireConnection) {
                return true;
            }

            //check left
            if(x > 0 && tileMapping[x - 1, y] == 1) {
                return true;
            }
            //check down
            if(y < (height - 1) && tileMapping[x, y + 1] == 1) {
                return true;
            }
            //other directions not needed

            return false;
        }
    }
}