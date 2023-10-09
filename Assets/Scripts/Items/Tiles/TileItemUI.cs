using System;
using Tile;
using UnityEngine;

namespace Items.Tiles
{
    /// <summary>
    /// Serializable class representing the UI information for a tile item.
    /// </summary>
    [Serializable]
    public class TileItemUI
    {
        /// <summary>
        /// The type of tile associated with this UI item.
        /// </summary>
        public TileType tileType;

        /// <summary>
        /// The sprite representing this tile in the UI.
        /// </summary>
        public Sprite tileSprite;

        /// <summary>
        /// A description of the tile for UI display.
        /// </summary>
        public string description;
    }
}