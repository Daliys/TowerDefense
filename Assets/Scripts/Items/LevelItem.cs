using Items.Waves;
using UnityEngine;

namespace Items
{
    /// <summary>
    /// ScriptableObject representing a level in the game, containing information such as its name, sprite, map, and wave configurations.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelItem", menuName = "Tower Defense/LevelItem", order = 0)]
    public class LevelItem : ScriptableObject
    {
        /// <summary>
        /// The name of the level for display in the game's menu.
        /// </summary>
        public string menuLevelName;

        /// <summary>
        /// The sprite representing the level in the game's menu.
        /// </summary>
        public Sprite menuLevelSprite;

        /// <summary>
        /// The map configuration for this level.
        /// </summary>
        public MapItem mapItem;

        /// <summary>
        /// The wave configurations for this level.
        /// </summary>
        public WavesItem wavesItem;
    }
}
