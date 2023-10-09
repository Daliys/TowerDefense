using System;
using Enemies;
using UnityEngine;

namespace Items.Enemies
{
    /// <summary>
    /// Represents a UI item for a specific negative effect type
    /// </summary>
    [Serializable]
    public class NegativeEffectUIItem
    {
        /// <summary>
        /// The type of negative effect associated with this UI item
        /// </summary>
        public EnemyNegativeEffectType type;
        
        /// <summary>
        /// The icon representing the negative effect in the UI
        /// </summary>
        public Sprite icon;
    }
}