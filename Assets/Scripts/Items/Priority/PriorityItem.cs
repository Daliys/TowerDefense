using System;
using Towers;
using UnityEngine;

namespace Items.Priority
{
    /// <summary>
    /// Serializable class representing a tower priority item.
    /// </summary>
    [Serializable]
    public class PriorityItem
    {
        /// <summary>
        /// The tower priority associated with this item.
        /// </summary>
        public TowerPriority priority;

        /// <summary>
        /// The sprite representing this priority.
        /// </summary>
        public Sprite prioritySprite;

        /// <summary>
        /// The name of this priority.
        /// </summary>
        public string priorityName;
    }
}