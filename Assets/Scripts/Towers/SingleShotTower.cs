using Enemies;
using UnityEngine;

namespace Towers
{
    /// <summary>
    /// Represents a tower that fires single-shot bullets at enemies.
    /// </summary>
    public class SingleShotTower : AbstractTower
    {
        [SerializeField] private GameObject bulletPrefab; // Prefab for the bullets fired by this tower.
        private Enemy _targetEnemy; // The current target enemy for the tower.

        /// <summary>
        /// Overrides the abstract Attack method to fire a bullet at the target enemy.
        /// </summary>
        protected override void Attack()
        {
            _targetEnemy = towerManager.GetEnemyInTowerRange(this);

            if (!_targetEnemy) return;

            // Get a bullet from the object pool and initialize it with the target enemy and damage information.
            Bullet bullet = ObjectPooling.instance.GetObject(GetTowerType()).GetComponent<Bullet>();
            bullet.transform.position = transform.position;
            bullet.Initialize(_targetEnemy, GetApplyingDamageToEnemy());
        }

        /// <summary>
        /// Updates the tower's rotation to face the current target enemy.
        /// </summary>
        private void Update()
        {
            if (!_targetEnemy) return;

            Vector3 direction = _targetEnemy.transform.position - rotationImage.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rotationImage.transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward);

        }

    }
}