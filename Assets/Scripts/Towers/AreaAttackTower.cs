using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Towers
{
    /// <summary>
    /// Represents an area attack tower, a type of tower that deals damage to multiple enemies within its range.
    /// </summary>
    public class AreaAttackTower : AbstractTower
    {
        [SerializeField] private GameObject attackAnimation; // The attack animation object.

        private bool _isShootAnimationPlaying; // Indicates if the attack animation is currently playing.
        private float _animationScale = 1; // The scale factor for the attack animation.

        /// <summary>
        /// Executes custom initialization logic after the tower is initialized.
        /// </summary>
        protected override void AfterInitialization()
        {
            base.AfterInitialization();

            if (GetTowerType() == TowerType.Tesla)
            {
                _animationScale = 1.3f;
            }
        }

        /// <summary>
        /// Implements the attack behavior for the area attack tower, damaging multiple enemies within its range.
        /// </summary>
        protected override void Attack()
        {
            List<Enemy> enemies = towerManager.GetAllEnemiesInTowerRange(this);

            if (enemies == null || enemies.Count == 0) return;

            foreach (var enemy in enemies)
            {
                enemy.TakeDamage(GetApplyingDamageToEnemy());
            }
            
            audioSource.Play();

            StartCoroutine(AttackAnimation());
        }

        /// <summary>
        /// Updates the rotation of the tower's visual representation and the attack animation.
        /// </summary>
        private void Update()
        {
            rotationImage.transform.Rotate(0, 0, -Time.deltaTime * 100);

            if (_isShootAnimationPlaying)
            {
                attackAnimation.transform.Rotate(0, 0, -Time.deltaTime * 100);
            }
        }

        /// <summary>
        /// Plays the attack animation for the area attack tower.
        /// </summary>
        private IEnumerator AttackAnimation()
        {
            attackAnimation.transform.localScale = new Vector3(GetTotalTowerRange() * _animationScale,
                GetTotalTowerRange() * _animationScale, 1f);

            _isShootAnimationPlaying = true;
            attackAnimation.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            attackAnimation.SetActive(false);
            _isShootAnimationPlaying = false;
        }
    }
}