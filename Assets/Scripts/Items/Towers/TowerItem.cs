using System;
using Towers;
using UnityEngine;

namespace Items.Towers
{
    /// <summary>
    /// Serializable class representing data for a tower type in the game.
    /// </summary>
    [Serializable]
    public class TowerItem
    {
        /// <summary>
        /// The type of the tower.
        /// </summary>
        public TowerType towerType;

        /// <summary>
        /// The prefab associated with this tower.
        /// </summary>
        public GameObject prefab;

        /// <summary>
        /// The name of the tower.
        /// </summary>
        public string towerName;

        /// <summary>
        /// The base damage of the tower.
        /// </summary>
        public float baseDamage;

        /// <summary>
        /// Multiplier for damage to health.
        /// </summary>
        public float healthDamageMultiplier;

        /// <summary>
        /// Multiplier for damage to armor.
        /// </summary>
        public float armorDamageMultiplier;

        /// <summary>
        /// Multiplier for damage to shield.
        /// </summary>
        public float shieldDamageMultiplier;

        /// <summary>
        /// The range of the tower.
        /// </summary>
        public float range;

        /// <summary>
        /// The fire rate of the tower.
        /// </summary>
        public float fireRate;

        /// <summary>
        /// The initial cost of building this tower.
        /// </summary>
        public int cost;

        /// <summary>
        /// Multiplier for the cost of upgrading this tower.
        /// </summary>
        public float costMultiplier;

        /// <summary>
        /// The cost of updating this tower.
        /// </summary>
        public int updateCost;

        /// <summary>
        /// Multiplier for the update cost of this tower.
        /// </summary>
        public float updateCostMultiplier;

        /// <summary>
        /// Experience required for the tower to auto-upgrade.
        /// </summary>
        public int autoUpgradeExperience;

        /// <summary>
        /// Multiplier for the auto-upgrade experience of this tower.
        /// </summary>
        public float autoUpgradeExperienceMultiplier;
    }
}