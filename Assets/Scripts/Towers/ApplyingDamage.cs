using System.Collections.Generic;
using Enemies;
using Items.Towers;

namespace Towers
{
    /// <summary>
    /// Represents the application of damage by a tower, factoring in base damage and bonuses.
    /// </summary>
    public class ApplyingDamage
    {
        private float _baseDamage;              // The base damage applied by the tower before any bonuses.
        private float _healthDamage;            // The damage multiplier for health-based damage
        private float _armorDamage;             // The damage multiplier for armor-based damage.
        private float _shieldDamage;            // The damage multiplier for shield-based damage.

        private readonly TowerBonuses _bonuses; // The bonuses applied to the tower's damage calculations.

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplyingDamage"/> class.
        /// </summary>
        /// <param name="bonuses">Bonuses applied to the damage calculations.</param>
        /// <param name="towerItem">Tower item containing base damage and multipliers.</param>
        public ApplyingDamage(TowerBonuses bonuses, TowerItem towerItem)
        {
            _bonuses = bonuses;

            _baseDamage = towerItem.baseDamage;
            _healthDamage = towerItem.healthDamageMultiplier;
            _armorDamage = towerItem.armorDamageMultiplier;
            _shieldDamage = towerItem.shieldDamageMultiplier;
        }

        public void AddBaseDamage(float value) => _baseDamage += value;
        public void AddHealthDamage(float value) => _healthDamage += value;
        public void AddArmorDamage(float value) => _armorDamage += value;
        public void AddShieldDamage(float value) => _shieldDamage += value;


        #region Getters
        
        /// <summary>
        ///  Retrieves the total damage values, factoring in base damage and bonuses.
        /// </summary>
        private float GetTotalBaseDamage() => _baseDamage + _bonuses.GetBaseDamage();

        /// <summary>
        ///  Return the total health damage values, factoring in base damage and health damage bonuses 
        /// </summary>
        public float GetTotalHealthDamage() => GetTotalBaseDamage() * GetHealthDamageWithBonuses();

        /// <summary>
        ///  Return the total armor damage values, factoring in base damage and armor damage bonuses
        /// </summary>
        public float GetTotalArmorDamage() => GetTotalBaseDamage() * GetArmorDamageWithBonuses();

        /// <summary>
        ///  Return the total shield damage values, factoring in base damage and shield damage bonuses
        /// </summary>
        public float GetTotalShieldDamage() => GetTotalBaseDamage() * GetShieldDamageWithBonuses();

        /// <summary>
        /// Return the health damage with bonuses
        /// </summary>
        private float GetHealthDamageWithBonuses() => _healthDamage + _bonuses.GetHealthDamage();

        /// <summary>
        ///  Return the armor damage with bonuses
        /// </summary>
        private float GetArmorDamageWithBonuses() => _armorDamage + _bonuses.GetArmorDamage();

        /// <summary>
        ///  Return the shield damage with bonuses
        /// </summary>
        /// <returns></returns>
        private float GetShieldDamageWithBonuses() => _shieldDamage + _bonuses.GetShieldDamage();

        /// <summary>
        /// Return the base damage as a string for UI purposes 
        /// </summary>
        public string GetBaseDamageForUI() => $"{GetTotalBaseDamage():F0}";

        /// <summary>
        ///  Return the health damage as a string for UI purposes
        /// </summary>
        public string GetHealthDamageForUI() =>
            $"{(_healthDamage + _bonuses.GetHealthDamage()):F0} ({GetTotalHealthDamage():F0})";

        /// <summary>
        ///  Return the armor damage as a string for UI purposes
        /// </summary>
        public string GetArmorDamageForUI() =>
            $"{(_armorDamage + _bonuses.GetArmorDamage()):F0} ({GetTotalArmorDamage():F0})";

        /// <summary>
        ///  Return the shield damage as a string for UI purposes
        /// </summary>
        public string GetShieldDamageForUI() =>
            $"{(_shieldDamage + _bonuses.GetShieldDamage()):F0} ({GetTotalShieldDamage():F0})";

        /// <summary>
        /// Retrieves the negative effects and their corresponding values from bonuses.
        /// </summary>
        public Dictionary<EnemyNegativeEffectType, float> GetNegativeEffects() => _bonuses.GetNegativeEffects();

        #endregion
    }
}