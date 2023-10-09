using System;
using Tile;
using UnityEngine;

namespace Items.Tiles
{
    /// <summary>
    /// Serializable class representing a tile item.
    /// </summary>
    [Serializable]
    public class TileItem
    {
        /// <summary>
        /// The type of tile associated with this item.
        /// </summary>
        public TileType tileType;

        /// <summary>
        /// The prefab representing this tile.
        /// </summary>
        public GameObject tilePrefab;

        /// <summary>
        /// ID for tile
        /// </summary>
        public int tileNumber;

        /// <summary>
        /// The bonus value associated with this tile.
        /// </summary>
        public float bonusValue;
    }
}