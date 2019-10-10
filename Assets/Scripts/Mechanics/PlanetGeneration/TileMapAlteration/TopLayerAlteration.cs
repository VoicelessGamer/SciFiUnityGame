using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLayerAlteration : TileMapAlteration {
    public override void alterMap(int[,] tileMapping) {
        for(int x = 0; x < tileMapping.GetLength(0); x++) {
            for(int y = 0; y < tileMapping.GetLength(1); y++) {
                if(tileMapping[x,y] == 1) {
                    tileMapping[x,y] = 2;
                    break;
                }
            }
        }
    }
}
