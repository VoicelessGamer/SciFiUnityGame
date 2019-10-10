using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapAlterationFactory {    
    public static TileMapAlteration getSectionBuilder(string type) {
        switch(type) {
            case "TOP_LAYER":
                return new TopLayerAlteration();
        }

        return null;
    }
}
