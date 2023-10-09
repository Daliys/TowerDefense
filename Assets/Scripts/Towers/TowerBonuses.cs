using System.Collections.Generic;
using Enemies;
using Items.BonusCards;

namespace Towers
{
    /// <summary>
    /// Represents a class for managing bonuses and negative effects associated with a tower.
    /// </summary>
    public class TowerBonuses
    {
        private Dictionary<TowerBonusType, float> _bonuses = new();
        private Dictionary<EnemyNegativeEffectType, float> _negativeEffects = new();

        /// <summary>
        /// Adds a bonus card to the tower, affecting its attributes.
        /// </summary>
        /// <param name="bonusCard">The bonus card to be applied.</param>
        public void AddBonusCard(AbstractBonusCard bonusCard)
        {
            if (bonusCard is BonusTowerCard bonusTowerCard)
            {
                AddTowerBonus(bonusTowerCard.bonusType, bonusTowerCard.bonusValue);
            }
            else if (bonusCard is BonusTowerNegativeEffectCard bonusTowerNegativeEffectCard)
            {
                AddBonusNegativeEffectForEnemy(bonusTowerNegativeEffectCard.bonusType,
                    bonusTowerNegativeEffectCard.bonusValue);
            }
        }

        /// <summary>
        /// Adds a bonus to the tower based on the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of bonus to be added.</param>
        /// <param name="value">The value of the bonus to be added.</param>
        public void AddTowerBonus(TowerBonusType type, float value)
        {
            if (_bonuses.ContainsKey(type))
            {
                _bonuses[type] += value;
            }
            else
            {
                _bonuses.Add(type, value);
            }
        }

        /// <summary>
        /// Adds a negative effect for the tower to apply to enemies based on the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of negative effect to be added.</param>
        /// <param name="value">The value of the negative effect to be added.</param>
        public void AddBonusNegativeEffectForEnemy(EnemyNegativeEffectType type, float value)
        {
            if (_negativeEffects.ContainsKey(type))
            {
                _negativeEffects[type] += value;
            }
            else
            {
                _negativeEffects.Add(type, value);
            }
        }

        #region Getters

        /// <summary>
        /// Gets the base damage bonus from the tower's bonuses.
        /// </summary>
        public float GetBaseDamage() => _bonuses.TryGetValue(TowerBonusType.BaseDamage, out var value) ? value : 0;

        /// <summary>
        /// Gets the health damage bonus from the tower's bonuses.
        /// </summary>
        public float GetHealthDamage() => _bonuses.TryGetValue(TowerBonusType.DamageHealth, out var value) ? value : 0;

        /// <summary>
        /// Gets the armor damage bonus from the tower's bonuses.
        /// </summary>
        public float GetArmorDamage() => _bonuses.TryGetValue(TowerBonusType.DamageArmor, out var value) ? value : 0;

        /// <summary>
        /// Gets the shield damage bonus from the tower's bonuses.
        /// </summary>
        public float GetShieldDamage() => _bonuses.TryGetValue(TowerBonusType.DamageShield, out var value) ? value : 0;

        /// <summary>
        /// Gets the fire rate bonus from the tower's bonuses.
        /// </summary>
        public float GetFireRate() => _bonuses.TryGetValue(TowerBonusType.FireRate, out var value) ? value : 0;

        /// <summary>
        /// Gets the range bonus from the tower's bonuses.
        /// </summary>
        public float GetRange() => _bonuses.TryGetValue(TowerBonusType.Range, out var value) ? value : 0;

        /// <summary>
        /// Gets the experience bonus from the tower's bonuses.
        /// </summary>
        public float GetExperience() => _bonuses.TryGetValue(TowerBonusType.Experience, out var value) ? value : 0;

        /// <summary>
        /// Gets the bleeding negative effect bonus from the tower's negative effects.
        /// </summary>
        public float GetNegativeBleeding() => _negativeEffects.TryGetValue(EnemyNegativeEffectType.Bleeding, out var value) ? value : 0;

        /// <summary>
        /// Gets the fire negative effect bonus from the tower's negative effects.
        /// </summary>
        public float GetNegativeFire() => _negativeEffects.TryGetValue(EnemyNegativeEffectType.Fire, out var value) ? value : 0;

        /// <summary>
        /// Gets the poison negative effect bonus from the tower's negative effects.
        /// </summary>
        public float GetNegativePoison() => _negativeEffects.TryGetValue(EnemyNegativeEffectType.Poison, out var value) ? value : 0;

        /// <summary>
        /// Gets the slowdown negative effect bonus from the tower's negative effects.
        /// </summary>
        public float GetNegativeSlowdown() => _negativeEffects.TryGetValue(EnemyNegativeEffectType.Slowdown, out var value) ? value : 0;

        /// <summary>
        /// Gets the dictionary of negative effects and their values.
        /// </summary>
        public Dictionary<EnemyNegativeEffectType, float> GetNegativeEffects() => _negativeEffects;

        #endregion

    }
}