using System;
using Items.Enemies;
using Towers;
using UI;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// Represents an enemy in the game that can move along a predefined path.
    /// </summary>
    public class Enemy : MonoBehaviour , IDamageable
    {
        #region Variables
        
        // Serialized fields accessible in the Unity Editor
        [SerializeField] private EnemyType enemyType;   // The type of the enemy
        [SerializeField] private EnemyUI enemyUI;       // The UI for the enemy
        
        // Private fields for enemy properties and behavior
        private float _moveSpeed;               // The movement speed of the enemy
        private float _speedModifier;           // A modifier applied to the movement speed (it might be slowdown effect)
        private int _pointIndex;                // The index of the current path point
        private Vector2 _currentTarget;         // The current target position
        private float _offset;                  // The offset applied to the path points
        private Vector2 _lastOffset;            // The offset from the previous step
        private float _pathSequenceRank;        // The rank of the enemy's path sequence
        private EnemyHealth _enemyHealth;       // The health component of the enemy
        private EnemyWaveGenerator _generator;  // The associated wave generator
        
        private EnemyItem _enemyItem;           // The item defining the enemy
        private TiledMap _tiledMap;             // The map with the enemy's path
        
        // Event for when an enemy reaches the end of its path
        public static event Action<Enemy> OnEnemyReachedEnd;

        #endregion
        
        /// <summary>
        /// Initializes the enemy with specific parameters.
        /// </summary>
        /// <param name="generator">The generator associated with the enemy.</param>
        /// <param name="enemyItem">The item defining the enemy.</param>
        /// <param name="tiledMap">The map with the enemy's path.</param>
        /// <param name="offset">The offset for the enemy's position.</param>
        public void Initialize(EnemyWaveGenerator generator, EnemyItem enemyItem, TiledMap tiledMap, float offset)
        {
            // Initialization of private fields
            _generator = generator;
            _enemyItem = enemyItem;
            _tiledMap = tiledMap;
            _moveSpeed = enemyItem.moveSpeed;
            _offset = offset;
            _lastOffset = new Vector2(_offset, _offset);
            _pathSequenceRank = 0;
            
            _pointIndex = -1;
            _speedModifier = 1;

            // Initialize or reset enemy health if it already exists (it might exist if the enemy is being reused via object pooling)
            if (_enemyHealth == null)
            {
                _enemyHealth = new EnemyHealth(this, enemyItem);
            }
            else
            {
                _enemyHealth.Reset();
            }

            transform.position = GetNextPathPointWithOffset();
            _currentTarget = GetNextPathPointWithOffset();
            
            UpdateEnemyUI();
        }

        private void FixedUpdate()
        {
            // Calculate the movement step based on speed, time, and speed modifier
            float movementStep = _moveSpeed / 40 * Time.fixedDeltaTime * _speedModifier;
            
            // Move the enemy towards the current target position using the movement step
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget, movementStep);
            
            CheckIfReachedTarget();
            UpdateCompletedPathInPercent();
        }
        
        /// <summary>
        /// Updates the completed path percentage for the enemy.
        /// </summary>
        private void UpdateCompletedPathInPercent()
        {
            // Calculate the completed path percentage
            float remainingDistance = Vector3.Distance(transform.position, _currentTarget);
            if (remainingDistance <= 1f) return;
            
            _pathSequenceRank = (int)(_pathSequenceRank) + (1 / remainingDistance);
        }
        
        /// <summary>
        /// Checks if the enemy has reached its target point and updates its position.
        /// </summary>
        private void CheckIfReachedTarget()
        {
            if (!(Vector3.Distance(transform.position, _currentTarget) < 0.001f)) return;
            
            transform.position = _currentTarget;

            if (_tiledMap.IsHavePathForEnemy(_pointIndex + 1))
            {
                _currentTarget = GetNextPathPointWithOffset();
            }
            else
            {
                OnEnemyReachedEnd?.Invoke(this);
                AudioManager.instance?.PlayEnemyReachedPortalSound();
            }
        }

        /// <summary>
        /// Calculates the next path point with an offset for the enemy to follow.
        /// </summary>
        /// <returns>The position of the next path point with the calculated offset.</returns>
        private Vector3 GetNextPathPointWithOffset()
        {
            // Increment the point index and path sequence rank
            _pointIndex++;
            _pathSequenceRank++;

            // Get the current path point and neighboring points
            Vector2 currentPoint = _tiledMap.GetPathForEnemy(_pointIndex);
            Vector2 lastPoint = _pointIndex == 0 ? currentPoint : _tiledMap.GetPathForEnemy(_pointIndex - 1);
            Vector2 nextPoint = _pointIndex == _tiledMap.GetPathLength() - 1
                ? currentPoint
                : _tiledMap.GetPathForEnemy(_pointIndex + 1);

            // Adjust the next and last point directions
            nextPoint.x = GetSign(nextPoint.x - currentPoint.x);
            nextPoint.y = GetSign(nextPoint.y - currentPoint.y);

            lastPoint.x = GetSign(lastPoint.x - currentPoint.x);
            lastPoint.y = GetSign(lastPoint.y - currentPoint.y);

            // Initialize offset based on the last offset
            Vector2 offset = new Vector2(_lastOffset.x, _lastOffset.y);

            if (_pointIndex == 0)
            {
                // Determine the offset based on the first path point
                if (nextPoint.y != 0)
                {
                    offset = new Vector2(_offset, 0);
                }
                else if (nextPoint.x != 0)
                {
                    offset = new Vector2(0, _offset);
                }

                _lastOffset = new Vector2(offset.x, offset.y);
                return new Vector3(currentPoint.x + offset.x, currentPoint.y + offset.y, 0);
            }

            // Calculate the offset based on neighboring points and the specified offset
            if (lastPoint.y != 0 && nextPoint.x != 0)
            {
                offset.y = lastPoint.y * nextPoint.x * _offset * GetSign(_offset) *
                           GetSign(_lastOffset.x);
            }
            else if (lastPoint.x != 0 && nextPoint.y != 0)
            {
                offset.x = lastPoint.x * nextPoint.y * _offset * GetSign(_offset) *
                           GetSign(_lastOffset.y);
            }

            _lastOffset = new Vector2(offset.x, offset.y);
            return new Vector3(currentPoint.x + offset.x, currentPoint.y + offset.y, 0);
        }

        /// <summary>
        /// Gets the sign of a value (1 if positive, -1 if negative, 0 if zero).
        /// </summary>
        /// <param name="value">The value to determine the sign for.</param>
        /// <returns>1 for positive, -1 for negative, or 0 for zero.</returns>
        private int GetSign(float value)
        {
            if (value == 0) return 0;
            return value > 0 ? 1 : -1;
        }

        /// <summary>
        /// Inflicts damage on the enemy based on the provided damage information.
        /// </summary>
        /// <param name="applyingDamage">The damage information to apply to the enemy.</param>
        public void TakeDamage(ApplyingDamageToEnemy applyingDamage)
        {
            _enemyHealth.TakeDamage(applyingDamage);
        }

        /// <summary>
        ///  Updates the enemy's UI.
        /// </summary>
        public void UpdateEnemyUI()
        {
            float healthPercent = _enemyHealth.GetMaxHealthPoint() == 0? 0 : _enemyHealth.GetCurrentHealthPoint() / _enemyHealth.GetMaxHealthPoint();
            float armorPercent = _enemyHealth.GetMaxArmorPoint() == 0? 0 : _enemyHealth.GetCurrentArmorPoint() / _enemyHealth.GetMaxArmorPoint();
            float shieldPercent = _enemyHealth.GetMaxShieldPoint() == 0? 0 : _enemyHealth.GetCurrentShieldPoint() / _enemyHealth.GetMaxShieldPoint();
            
            enemyUI.SetHealthBar(healthPercent, armorPercent, shieldPercent);
            enemyUI.SetNegativeEffects(_enemyHealth.GetNegativeEffects());
        }
        
        #region Getters And Setters For Enemy Properties
        
        /// <summary>
        /// Gets the current path sequence rank of the enemy.
        /// </summary>
        /// <returns>The path sequence rank of the enemy.</returns>
        public float GetPathSequenceRank() => _pathSequenceRank;
        
        /// <summary>
        /// Gets the amount of coins the enemy is worth upon defeat.
        /// </summary>
        /// <returns>The number of coins the enemy is worth.</returns>
        public int GetCoinsCount() => _enemyItem.gold + (int)_generator.GetBonusValue(GameBonus.MoneyForKill);

        /// <summary>
        /// Gets the game score value associated with defeating this enemy.
        /// </summary>
        /// <returns>The game score value for defeating this enemy.</returns>
        public int GetGameScoreValue() => _enemyItem.spawnValue;

        /// <summary>
        /// Gets the health of the enemy.
        /// </summary>
        /// <returns>The enemy's health information.</returns>
        public EnemyHealth GetEnemyHealth() => _enemyHealth;

        /// <summary>
        /// Gets the damage dealt by the enemy.
        /// </summary>
        /// <returns>The damage dealt by the enemy.</returns>
        public int GetEnemyDamage() => _enemyItem.damage;

        /// <summary>
        /// Gets the generator associated with the enemy.
        /// </summary>
        /// <returns>The enemy's generator.</returns>
        public EnemyWaveGenerator GetGenerator() => _generator;

        /// <summary>
        /// Gets the type of the enemy.
        /// </summary>
        /// <returns>The type of the enemy.</returns>
        public EnemyType GetEnemyType() => enemyType;
        
        /// <summary>
        /// Sets the speed multiplier of the enemy.
        /// </summary>
        /// <param name="negativeEffectValue">Value that need to be set</param>
        public void SetSpeedMultiplier(float negativeEffectValue) => _speedModifier = negativeEffectValue;

        #endregion
    }
}