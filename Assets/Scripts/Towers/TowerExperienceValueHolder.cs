using Items.Towers;
using UnityEngine;

namespace Towers
{
    /// <summary>
    /// Holds and manages experience values for a specific damage type for an abstract tower.
    /// </summary>
    public class TowerExperienceValueHolder
    {
        private readonly DamageType _experienceType; // The damage type for this experience holder.
        private readonly AbstractTower _tower; // The associated tower.
        private readonly TowerItem _item; // The tower item associated with the tower.
        
        private float _currentExperienceValue; 
        private float _currentExperienceToUpgrade;

        /// <summary>
        /// Constructor for the TowerExperienceValueHolder.
        /// </summary>
        /// <param name="tower">The associated tower.</param>
        /// <param name="experienceType">The damage type for this experience holder.</param>
        public TowerExperienceValueHolder(AbstractTower tower, DamageType experienceType)
        {
            _tower = tower;
            _item = tower.GetTowerItem();
            _experienceType = experienceType;
            
            _currentExperienceToUpgrade = _item.autoUpgradeExperience;
        }

        /// <summary>
        /// Upgrades the experience to the next level.
        /// </summary>
        public void UpgradeExperienceToNextLevel()
        {
            float experienceValue = _currentExperienceToUpgrade - _currentExperienceValue + 1;
            AddExperienceValue(experienceValue);
        }
        
        /// <summary>
        /// Adds experience value and updates associated tower attributes accordingly.
        /// </summary>
        /// <param name="value">The experience value to add.</param>
        public void AddExperienceValue(float value)
        {
            value += value * _tower.GetTowerBonuses().GetExperience();
            _currentExperienceValue += value;
            
            if (_currentExperienceValue >= _currentExperienceToUpgrade)
            {
                _tower.GetTowerApplyingDamage().AddBaseDamage(1);

                switch (_experienceType)
                {
                    case DamageType.Health:
                        _tower.GetTowerApplyingDamage().AddHealthDamage(1);
                        break;
                    case DamageType.Armor:
                        _tower.GetTowerApplyingDamage().AddArmorDamage(1);
                        break;
                    case DamageType.Shield:
                        _tower.GetTowerApplyingDamage().AddShieldDamage(1);
                        break;
                }

                _currentExperienceValue = 0;
                _tower.GetTowerExperiences().IncreaseTowerLevel();
                UpdateCurrentRequireExperience();
            }
        }
        
        /// <summary>
        /// Updates the current required experience for the next level.
        /// </summary>
        private void UpdateCurrentRequireExperience()
        {
            _currentExperienceToUpgrade = _item.autoUpgradeExperience * Mathf.Pow(_item.autoUpgradeExperienceMultiplier,
                _tower.GetTowerExperiences().GetTowerLevel());
        }
        
        /// <summary>
        /// Gets the current experience value.
        /// </summary>
        public float GetCurrentExperienceValue() => _currentExperienceValue;
        
        /// <summary>
        /// Gets the current required experience to upgrade.
        /// </summary>
        public float GetCurrentExperienceToUpgrade() => _currentExperienceToUpgrade;
        
    }
}