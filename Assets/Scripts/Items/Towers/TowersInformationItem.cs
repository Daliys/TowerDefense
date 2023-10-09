using System.Linq;
using Towers;
using UnityEngine;

namespace Items.Towers
{
    /// <summary>
    /// ScriptableObject containing tower information for the game.
    /// </summary>
    [CreateAssetMenu(fileName = "TowersInformationItem", menuName = "Tower Defense/TowersInformationItem", order = 1)]
    public class TowersInformationItem : ScriptableObject
    {
        /// <summary>
        /// Array of TowerItem instances representing different tower types.
        /// </summary>
        public TowerItem[] towerItems;

        /// <summary>
        /// Get the TowerItem associated with the specified tower type.
        /// </summary>
        /// <param name="towerType">The tower type.</param>
        /// <returns>The TowerItem associated with the specified tower type, or null if not found.</returns>
        public TowerItem GetTowerItem(TowerType towerType)
        {
            return towerItems.FirstOrDefault(towerItem => towerItem.towerType == towerType);
        }
    }
}