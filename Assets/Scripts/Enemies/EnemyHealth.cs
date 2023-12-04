using System;
using System.Collections;
using System.Collections.Generic;
using Items.Enemies;
using Towers;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Represents the health and damage handling of an enemy in the game.
    /// </summary>
    public class EnemyHealth
    {
        #region Variables
        
        // Event triggered when an enemy dies
        public static event Action<Enemy> OnEnemyDied;

        private readonly Enemy _enemy;

        // Health-related variables
        private readonly float _maxHealthPoint;       // The maximum health points the enemy can have
        private float _currentHealthPoint;            // The current health points of the enemy
        private readonly float _maxArmorPoint;        // The maximum armor points the enemy can have
        private float _currentArmorPoint;             // The current armor points of the enemy
        private readonly float _maxShieldPoint;       // The maximum shield points the enemy can have
        private float _currentShieldPoint;            // The current shield points of the enemy

        // Negative effects and damage handling variables
        private Dictionary<EnemyNegativeEffectType, float> _negativeEffects;
        private Coroutine _timer;

        // Constants for damage calculation
        private const float MaxApplyingDamage = 20;                 // Maximum damage that can be applied
        private const float MaxSlowDownValue = 0.5f;                // Maximum value for the slowdown effect
        private const float AbsorbingSlowDownPerSecond = 0.05f;     // Rate of reduction for slowdown effect over time
        
        #endregion
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyHealth"/> class.
        /// </summary>
        /// <param name="enemy">The associated enemy.</param>
        /// <param name="item">The item defining the enemy.</param>
        public EnemyHealth(Enemy enemy, EnemyItem item)
        {
            _enemy = enemy;
            _maxHealthPoint = item.health;
            _currentHealthPoint = _maxHealthPoint;
            _maxArmorPoint = item.armor;
            _currentArmorPoint = _maxArmorPoint;
            _maxShieldPoint = item.shield;
            _currentShieldPoint = _maxShieldPoint;

            _negativeEffects = new Dictionary<EnemyNegativeEffectType, float>();
            _timer = _enemy.StartCoroutine(ApplyNegativeEffectsCoroutine());
        }

        /// <summary>
        /// Resets the enemy's health and negative effects to their initial values.
        /// </summary>
        public void Reset()
        {
            // Reset values of health, armor, shield, and negative effects
            _currentHealthPoint = _maxHealthPoint;
            _currentArmorPoint = _maxArmorPoint;
            _currentShieldPoint = _maxShieldPoint;
            _negativeEffects.Clear();
            _enemy.UpdateEnemyUI();

            if (_timer != null)
            {
                _enemy.StopCoroutine(_timer);
            }

            _timer = _enemy.StartCoroutine(ApplyNegativeEffectsCoroutine());
        }
        
        /// <summary>
        /// Handles the damage taken by the enemy based on the applying damage information.
        /// </summary>
        /// <param name="applyingDamage">The damage information to apply to the enemy.</param>
        public void TakeDamage(ApplyingDamageToEnemy applyingDamage)
        {
            float appliedDamage = 0;
            
            // Check and deduct damage from shield, armor, or health accordingly
            if (_currentShieldPoint > 0)
            {
                _currentShieldPoint -= applyingDamage.GetTotalShieldDamage();
                appliedDamage += applyingDamage.GetTotalShieldDamage();
                applyingDamage.AddExperienceToTower(DamageType.Shield);
                _currentShieldPoint = Mathf.Max(_currentShieldPoint, 0);
            }
            else if (_currentArmorPoint > 0)
            {
                _currentArmorPoint -= applyingDamage.GetTotalArmorDamage();
                appliedDamage += applyingDamage.GetTotalArmorDamage();
                applyingDamage.AddExperienceToTower(DamageType.Armor);
                _currentArmorPoint = Mathf.Max(_currentArmorPoint, 0);
            }
            else
            {
                _currentHealthPoint -= applyingDamage.GetTotalHealthDamage();
                appliedDamage += applyingDamage.GetTotalHealthDamage();
                applyingDamage.AddExperienceToTower(DamageType.Health);
                _currentHealthPoint = Mathf.Max(_currentHealthPoint, 0);
            }

            // Apply all negative effects associated with the damage
            AddAllNegativeEffects(applyingDamage.GetNegativeEffects(), appliedDamage);
    
            // Update the enemy's UI to reflect the damage and effects
            _enemy.UpdateEnemyUI();

            // Check if the enemy is still alive after taking the damage
            CheckIsAlive();
        }

        /// <summary>
        /// Handles direct damage to the enemy based on a specified value.
        /// </summary>
        /// <param name="value">The value of the damage to apply.</param>
        private void TakeDamage(float value)
        {
            // Check and deduct damage from shield, armor, or health accordingly
            if (_currentShieldPoint > 0)
            {
                _currentShieldPoint -= value;
                _currentShieldPoint = Mathf.Max(_currentShieldPoint, 0);
            }
            else if (_currentArmorPoint > 0)
            {
                _currentArmorPoint -= value;
                _currentArmorPoint = Mathf.Max(_currentArmorPoint, 0);
            }
            else
            {
                _currentHealthPoint -= value;
                _currentHealthPoint = Mathf.Max(_currentHealthPoint, 0);
            }
            
            // Update the enemy's UI to reflect the damage and effects
            _enemy.UpdateEnemyUI();

            // Check if the enemy is still alive after taking the damage
            CheckIsAlive();
        }

        /// <summary>
        /// Checks if the enemy is still alive after taking the damage.
        /// </summary>
        private void CheckIsAlive()
        {
            if (_currentHealthPoint <= 0)
            {
                // If the enemy is dead, trigger the event and set the health to 0
                _currentHealthPoint = 0;
                OnEnemyDiedHandler();
                OnEnemyDied?.Invoke(_enemy);
                AudioManager.instance?.PlayEnemyDiedSound();
            }
        }

        /// <summary>
        /// Adds all the negative effects to the enemy, modifying their properties based on the effects.
        /// </summary>
        /// <param name="negativeEffects">The negative effects to add.</param>
        /// <param name="baseDamage">The base damage to associate with the effects.</param>
        private void AddAllNegativeEffects(Dictionary<EnemyNegativeEffectType, float> negativeEffects, float baseDamage)
        {
            foreach (var negativeEffect in negativeEffects)
            {
                // Handle slowdown effect separately, limiting its value to MaxSlowDownValue
                if (negativeEffect.Key == EnemyNegativeEffectType.Slowdown)
                {
                    if (_negativeEffects.ContainsKey(negativeEffect.Key))
                    {
                        _negativeEffects[negativeEffect.Key] = Mathf.Min(negativeEffect.Value + _negativeEffects[negativeEffect.Key] , MaxSlowDownValue );
                    }
                    else
                    {
                        _negativeEffects.Add(negativeEffect.Key, negativeEffect.Value);
                    }
                    return;
                }
                
                // Add or update the negative effect based on the effect type and base damage
                if (_negativeEffects.ContainsKey(negativeEffect.Key))
                {
                    _negativeEffects[negativeEffect.Key] += negativeEffect.Value * baseDamage;
                }
                else
                {
                    _negativeEffects.Add(negativeEffect.Key, negativeEffect.Value * baseDamage);
                }
            }
        }

        /// <summary>
        ///  Coroutine that applies the negative effects to the enemy, affecting its behavior and state.
        /// </summary>
        private IEnumerator ApplyNegativeEffectsCoroutine()
        {
            while (IsAlive)
            {
                yield return new WaitForSeconds(1);
                ApplyNegativeEffects();
            }
        }

        /// <summary>
        /// Applies the negative effects to the enemy, affecting its behavior and state.
        /// </summary>
        private void ApplyNegativeEffects()
        {
            // Check if there are negative effects to apply
            if (_negativeEffects == null || _negativeEffects.Count == 0) return;
            
            // Create a copy of the negative effects dictionary to avoid modification during iteration
            Dictionary<EnemyNegativeEffectType, float> copy = new Dictionary<EnemyNegativeEffectType, float>(_negativeEffects);
            
            // Iterate through each negative effect and apply the appropriate action
            foreach (var negativeEffect in copy)
            {
                switch (negativeEffect.Key)
                {
                    case EnemyNegativeEffectType.Slowdown:
                        _negativeEffects[negativeEffect.Key] -= AbsorbingSlowDownPerSecond;
                        if(_negativeEffects[negativeEffect.Key] <= 0)
                        {
                            _negativeEffects.Remove(negativeEffect.Key);
                        }
                        _enemy.SetSpeedMultiplier(1 - negativeEffect.Value);
                        break;
                    case EnemyNegativeEffectType.Poison:
                       ApplyDamageEffect( negativeEffect, GameBonus.IncreasePoisonDamage);
                       break;
                    case EnemyNegativeEffectType.Bleeding:
                        ApplyDamageEffect( negativeEffect, GameBonus.IncreaseBleedDamage);
                        break;
                    case EnemyNegativeEffectType.Fire:
                        ApplyDamageEffect( negativeEffect, GameBonus.IncreaseFireDamage);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            _enemy.UpdateEnemyUI();
        }

        /// <summary>
        /// Applies the damage effect associated with a specific negative effect type.
        /// </summary>
        /// <param name="negativeEffect">The negative effect and its associated damage.</param>
        /// <param name="type">The type of damage effect to apply.</param>
        private void ApplyDamageEffect(KeyValuePair<EnemyNegativeEffectType, float> negativeEffect, GameBonus type)
        {
            // Calculate the maximum damage to apply, considering bonuses
            float maxDamage = MaxApplyingDamage + _enemy.GetGenerator().GetBonusValue(type);
            
            // Determine the damage to apply (minimum of the negative effect value and max damage)
            float damageToApply = Mathf.Min(negativeEffect.Value, maxDamage);
            TakeDamage(damageToApply);
            
            // Reduce the damage from the negative effects
            _negativeEffects[negativeEffect.Key] -= damageToApply;

            // Remove the negative effect if the damage has been fully applied
            if (_negativeEffects[negativeEffect.Key] <= 0)
            {
                _negativeEffects.Remove(negativeEffect.Key);
            }
        }

        /// <summary>
        /// Handles the event when the enemy dies.
        /// </summary>
        private void OnEnemyDiedHandler()
        {
            // Stop the negative effects application coroutine
            _enemy.StopCoroutine(_timer);
        }
        
        // Boolean property indicating whether the enemy is alive
        private bool IsAlive => _currentHealthPoint > 0;


        #region Getters and Setters for health-related variables
        
        /// <summary>
        /// Gets the maximum health point of the enemy.
        /// </summary>
        /// <returns>The maximum health point of the enemy.</returns>
        public float GetMaxHealthPoint() => _maxHealthPoint;

        /// <summary>
        /// Gets the current health point of the enemy.
        /// </summary>
        /// <returns>The current health point of the enemy.</returns>
        public float GetCurrentHealthPoint() => _currentHealthPoint;

        /// <summary>
        /// Gets the maximum armor point of the enemy.
        /// </summary>
        /// <returns>The maximum armor point of the enemy.</returns>
        public float GetMaxArmorPoint() => _maxArmorPoint;

        /// <summary>
        /// Gets the current armor point of the enemy.
        /// </summary>
        /// <returns>The current armor point of the enemy.</returns>
        public float GetCurrentArmorPoint() => _currentArmorPoint;

        /// <summary>
        /// Gets the maximum shield point of the enemy.
        /// </summary>
        /// <returns>The maximum shield point of the enemy.</returns>
        public float GetMaxShieldPoint() => _maxShieldPoint;

        /// <summary>
        /// Gets the current shield point of the enemy.
        /// </summary>
        /// <returns>The current shield point of the enemy.</returns>
        public float GetCurrentShieldPoint() => _currentShieldPoint;

        /// <summary>
        /// Gets the negative effects affecting the enemy.
        /// </summary>
        /// <returns>The dictionary of negative effects affecting the enemy.</returns>
        public Dictionary<EnemyNegativeEffectType, float> GetNegativeEffects() => _negativeEffects;

        #endregion
    }
}