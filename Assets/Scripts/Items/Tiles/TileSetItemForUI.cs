using System.Linq;
using Tile;
using UnityEngine;

namespace Items.Tiles
{
    /// <summary>
    /// Scriptable object representing a collection of tile item UI data in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "TileSetItemForUI", menuName = "Tower Defense/TileSetItemForUI", order = 1)]
    public class TileSetItemForUI : ScriptableObject
    {
        /// <summary>
        /// Array of tile item UI data in this tile set.
        /// </summary>
        public TileItemUI[] tileItemsUI;

        /// <summary>
        /// Gets the tile item UI data associated with a specific tile type.
        /// </summary>
        /// <param name="tileType">The type of tile to retrieve the UI data for.</param>
        /// <returns>The tile item UI data for the specified tile type.</returns>
        public TileItemUI GetTileItem(TileType tileType)
        {
            return tileItemsUI.FirstOrDefault(tile => tile.tileType == tileType);
        }
    }
}