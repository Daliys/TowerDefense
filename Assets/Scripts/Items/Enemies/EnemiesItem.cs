using System.Linq;
using Enemies;
using UnityEngine;

namespace Items.Enemies
{
    /// <summary>
    ///   Scriptable object that contains all enemies items
    /// </summary>
    [CreateAssetMenu(fileName = "EnemiesItem", menuName = "Tower Defense/EnemiesItem")]
    public class EnemiesItem : ScriptableObject
    {
        public EnemyItem[] enemiesItems;
        
        /// <summary>
        /// Gets the enemy item associated with a specific enemy type.
        /// </summary>
        /// <param name="enemyType">The enemy type to retrieve.</param>
        /// <returns>The enemy item corresponding to the specified enemy type, or null if not found.</returns>
        public EnemyItem GetEnemyItem(EnemyType enemyType)
        {
            return enemiesItems.FirstOrDefault(item => item.enemyType == enemyType);
        }
    }
}