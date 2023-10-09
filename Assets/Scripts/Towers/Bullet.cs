using Enemies;
using UnityEngine;

namespace Towers
{
    /// <summary>
    /// Represents a bullet fired by a tower to damage an enemy.
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private TowerType towerType; // The type of tower associated with this bullet.

        private Enemy _target; // The target enemy for this bullet.
        private ApplyingDamageToEnemy _applyingDamage; // The damage to be applied to the enemy.
        private bool _isActive; // Indicates if the bullet is active and in motion.

        /// <summary>
        /// Initializes the bullet with a target enemy and damage information.
        /// </summary>
        public void Initialize(Enemy target, ApplyingDamageToEnemy applyingDamage)
        {
            _target = target;
            _applyingDamage = applyingDamage;
            _isActive = true;

            EnemyHealth.OnEnemyDied += OnEnemyDied;
            Enemy.OnEnemyReachedEnd += OnEnemyDied;
        }

        /// <summary>
        /// Handler for the enemy death event. Deactivates the bullet if it was targeting the dead enemy.
        /// </summary>
        private void OnEnemyDied(Enemy enemy)
        {
            if (_target == enemy)
            {
                _isActive = false;
                SendToPool();
            }
        }

        /// <summary>
        /// Updates the bullet's position, moves towards the target enemy, and applies damage when close enough.
        /// </summary>
        public void FixedUpdate()
        {
            if (!_isActive) return;

            transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Time.deltaTime * 15);

            if (Vector3.Distance(transform.position, _target.transform.position) < 0.001f)
            {
                _target.TakeDamage(_applyingDamage);
                _isActive = false;

                SendToPool();
            }
        }

        /// <summary>
        /// Sends the bullet back to the object pool and unsubscribes from relevant events.
        /// </summary>
        private void SendToPool()
        {
            EnemyHealth.OnEnemyDied -= OnEnemyDied;
            Enemy.OnEnemyReachedEnd -= OnEnemyDied;

            ObjectPooling.instance.ReturnToPool(this);
        }

        /// <summary>
        /// Gets the tower type associated with this bullet.
        /// </summary>
        public TowerType GetBelongTowerType() => towerType;
    }
}