using System;
using Enemies;
using UnityEngine;

namespace Items.Enemies
{
    /// <summary>
    /// Represents an item for a specific type of enemy.
    /// </summary>
    [Serializable]
    public class EnemyItem
    {
        /// <summary>
        /// The type of enemy associated with this item.
        /// </summary>
        public EnemyType enemyType;

        /// <summary>
        /// The prefab representing this type of enemy in the game.
        /// </summary>
        public GameObject prefab;

        /// <summary>
        /// The spawn value used for generating this type of enemy.
        /// </summary>
        public int spawnValue;

        /// <summary>
        /// The movement speed of this type of enemy.
        /// </summary>
        public float moveSpeed;

        /// <summary>
        /// The health points of this type of enemy.
        /// </summary>
        public int health;

        /// <summary>
        /// The armor points of this type of enemy.
        /// </summary>
        public int armor;

        /// <summary>
        /// The shield points of this type of enemy.
        /// </summary>
        public int shield;

        /// <summary>
        /// The damage inflicted by this type of enemy.
        /// </summary>
        public int damage;

        /// <summary>
        /// The amount of gold dropped by this type of enemy when defeated.
        /// </summary>
        public int gold;
    }
}