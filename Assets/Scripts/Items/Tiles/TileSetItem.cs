using System.Linq;
using Tile;
using UnityEngine;

namespace Items.Tiles
{
    /// <summary>
    /// Scriptable object representing a collection of tile items in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "TileSetItem", menuName = "Tower Defense/TileSetItem", order = 1)]
    public class TileSetItem : ScriptableObject
    {
        /// <summary>
        /// Array of tile items in this tile set.
        /// </summary>
        public TileItem[] tileItems;
        
        /// <summary>
        /// Gets the bonus value associated with a specific tile type.
        /// </summary>
        /// <param name="tileType">The type of tile to retrieve the bonus for.</param>
        /// <returns>The bonus value for the specified tile type.</returns>
        public float GetTileBonus(TileType tileType)
        {
            return (from tileItem in tileItems where tileItem.tileType == tileType select tileItem.bonusValue).FirstOrDefault();
        }
    }
}