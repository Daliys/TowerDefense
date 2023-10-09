using UnityEngine;

namespace Items
{
    /// <summary>
    ///  MapItem is a ScriptableObject that contains information about the map.
    /// </summary>
    [CreateAssetMenu(fileName = "MapItem", menuName = "Tower Defense/MapItem", order = 1)]
    public class MapItem : ScriptableObject
    {
        /// <summary>
        ///  The size of the map in tiles.
        /// </summary>
        public Point mapSize;

        /// <summary>
        ///  The size of each tile in world units.
        /// </summary>
        public string mapString;

        /// <summary>
        ///  Path points are the points that the enemies will follow.
        /// </summary>
        public Point[] pathPoints;
    }
}