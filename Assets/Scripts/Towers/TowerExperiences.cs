using System.Collections.Generic;
using UnityEngine;

namespace Towers
{
    /// <summary>
    /// Class representing the experiences and upgrades of a tower.
    /// </summary>
    public class TowerExperiences
    {
        private readonly float _experienceMultiplier = 1f;
        private int _towerLevel = 1;
        private readonly Dictionary<DamageType, TowerExperienceValueHolder> _damageTypeExperiences;
        private readonly AbstractTower _tower;

        /// <summary>
        /// Initializes a new instance of the <see cref="TowerExperiences"/> class.
        /// </summary>
        /// <param name="tower">The tower associated with these experiences.</param>
        public TowerExperiences(AbstractTower tower)
        {
            _tower = tower;
            _damageTypeExperiences = new Dictionary<DamageType, TowerExperienceValueHolder>
            {
                { DamageType.Health, new TowerExperienceValueHolder(tower, DamageType.Health) },
                { DamageType.Armor, new TowerExperienceValueHolder(tower, DamageType.Armor) },
                { DamageType.Shield, new TowerExperienceValueHolder(tower, DamageType.Shield) }
            };
        }

        /// <summary>
        /// Adds experience value for a specific damage type.
        /// </summary>
        /// <param name="type">The damage type to add experience for.</param>
        /// <param name="value">The experience value to add.</param>
        public void AddExperienceValue(DamageType type, float value)
        {
            _damageTypeExperiences[type].AddExperienceValue(value);
            _tower.InvokeOnTowerDataChanged();
        }

        /// <summary>
        /// Increases the tower level.
        /// </summary>
        public void IncreaseTowerLevel()
        {
            _towerLevel++;
        }

        /// <summary>
        /// Upgrades damage to the next level for a specific damage type.
        /// </summary>
        /// <param name="type">The damage type to upgrade.</param>
        public void UpgradeDamageToNextLevel(DamageType type)
        {
            _damageTypeExperiences[type].UpgradeExperienceToNextLevel();
            _tower.InvokeOnTowerDataChanged();
        }

        /// <summary>
        /// Gets the tower level.
        /// </summary>
        public int GetTowerLevel() => _towerLevel;

        /// <summary>
        /// Gets the experience multiplier.
        /// </summary>
        public float GetExperienceMultiplier() => _experienceMultiplier;


        /// <summary>
        /// Gets the upgrade cost for upgrading damage to the next level for a specific damage type.
        /// </summary>
        /// <param name="type">The damage type to calculate the upgrade cost for.</param>
        /// <returns>The upgrade cost.</returns>
        public int GetUpgradeCost(DamageType type)
        {
            float cost = GetUpgradeCost();
            float currentExp = _damageTypeExperiences[type].GetCurrentExperienceValue();
            float expToUpgrade = _damageTypeExperiences[type].GetCurrentExperienceToUpgrade();

            if (currentExp == 0)
            {
                return (int)cost;
            }

            cost *= (1 - (currentExp / expToUpgrade));

            return (int)Mathf.Max(cost, 1f);
            ;
        }

        /// <summary>
        /// Calculates the base upgrade cost for a specific damage type.
        /// </summary>
        /// <returns>The base upgrade cost.</returns>
        private int GetUpgradeCost()
        {
            float cost = _tower.GetTowerItem().updateCost;
            cost += cost * Mathf.Pow(_tower.GetTowerItem().costMultiplier, _towerLevel);
            return (int)cost;
        }

        /// <summary>
        /// Gets the experience percentage for UI display for a specific damage type.
        /// </summary>
        /// <param name="type">The damage type to calculate the experience percentage for.</param>
        /// <returns>The experience percentage for UI display.</returns>
        public float GetExperiencePercentForUI(DamageType type)
        {
            float currentExp = _damageTypeExperiences[type].GetCurrentExperienceValue();
            float expToUpgrade = _damageTypeExperiences[type].GetCurrentExperienceToUpgrade();

            if (currentExp == 0) return 0f;

            return currentExp / expToUpgrade;
        }

        /// <summary>
        /// Checks if there's enough money to upgrade damage for a specific damage type.
        /// </summary>
        /// <param name="type">The damage type to check the upgrade cost for.</param>
        public bool IsEnoughMoneyToUpgradeDamage(DamageType type)
        {
            return _tower.GetGame().IsEnoughCoinsToBuy(GetUpgradeCost(type));
        }
    }
}