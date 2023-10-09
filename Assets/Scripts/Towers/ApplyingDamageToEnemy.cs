using System.Collections.Generic;
using Enemies;

namespace Towers
{
    /// <summary>
    /// Represents the total damage to be applied to an enemy, including health, armor, and shield damage, along with negative effects.
    /// </summary>
    public class ApplyingDamageToEnemy
    {
        private readonly float _totalHealthDamage;  // Total health-based damage to be applied.
        private readonly float _totalArmorDamage;   // Total armor-based damage to be applied.
        private readonly float _totalShieldDamage;  // Total shield-based damage to be applied.

        private readonly Dictionary<EnemyNegativeEffectType, float> _negativeEffects;  // Dictionary to store negative effects and their values.
        private readonly TowerExperiences _towerExperiences;  // Tower experiences for damage type progression.

        /// <summary>
        /// Constructs an instance of ApplyingDamageToEnemy.
        /// </summary>
        /// <param name="totalHealthDamage">Total health-based damage to be applied.</param>
        /// <param name="totalArmorDamage">Total armor-based damage to be applied.</param>
        /// <param name="totalShieldDamage">Total shield-based damage to be applied.</param>
        /// <param name="negativeEffects">Dictionary of negative effects and their values.</param>
        /// <param name="towerExperiences">Tower experiences for damage type progression.</param>
        public ApplyingDamageToEnemy(float totalHealthDamage, float totalArmorDamage, float totalShieldDamage, 
            Dictionary<EnemyNegativeEffectType, float> negativeEffects, TowerExperiences towerExperiences)
        {
            _totalHealthDamage = totalHealthDamage;
            _totalArmorDamage = totalArmorDamage;
            _totalShieldDamage = totalShieldDamage;
            _negativeEffects = negativeEffects;
            _towerExperiences = towerExperiences;
        }

        /// <summary>
        /// Adds experience points to the tower based on the damage type.
        /// </summary>
        /// <param name="damageType">The type of damage to apply experience for.</param>
        public void AddExperienceToTower(DamageType damageType)
        {
            _towerExperiences.AddExperienceValue(damageType,1);
        }

        /// <summary>
        /// Gets the total health-based damage to be applied.
        /// </summary>
        public float GetTotalHealthDamage() => _totalHealthDamage;

        /// <summary>
        /// Gets the total armor-based damage to be applied.
        /// </summary>
        public float GetTotalArmorDamage() => _totalArmorDamage;

        /// <summary>
        /// Gets the total shield-based damage to be applied.
        /// </summary>
        public float GetTotalShieldDamage() => _totalShieldDamage;

        /// <summary>
        /// Gets the dictionary of negative effects and their values.
        /// </summary>
        public Dictionary<EnemyNegativeEffectType, float> GetNegativeEffects() => _negativeEffects;

    }
}