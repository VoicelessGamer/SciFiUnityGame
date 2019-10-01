using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace Mechanics.TileMapGen {
    public abstract class TileMapGenerator : MonoBehaviour {
        public abstract int[,] generateMap(int width, int height);
    }
}