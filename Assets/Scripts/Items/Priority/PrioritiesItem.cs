using Towers;
using UnityEngine;

namespace Items.Priority
{
    /// <summary>
    /// ScriptableObject containing an array of priority items
    /// </summary>
    [CreateAssetMenu(fileName = "PrioritiesItem", menuName = "Tower Defense/PrioritiesItem", order = 0)]
    public class PrioritiesItem : ScriptableObject
    {
        /// <summary>
        /// Array of priority items
        /// </summary>
        public PriorityItem[] priorityItems;

        /// <summary>
        /// Get the next priority item in the array based on the current priority.
        /// </summary>
        /// <param name="currentPriority">The current tower priority.</param>
        /// <returns>The next priority item.</returns>
        public PriorityItem GetNextPriorityItem(TowerPriority currentPriority)
        {
            for (var i = 0; i < priorityItems.Length; i++)
            {
                if (priorityItems[i].priority == currentPriority)
                {
                    var index = (i + i) >= priorityItems.Length ? 0 : i + 1;
                    return priorityItems[index];
                }
            }

            throw new System.Exception("Priority not found");
        }

        /// <summary>
        /// Get the previous priority item in the array based on the current priority.
        /// </summary>
        /// <param name="currentPriority">The current tower priority.</param>
        /// <returns>The previous priority item.</returns>
        public PriorityItem GetPreviousPriority(TowerPriority currentPriority)
        {
            for (var i = 0; i < priorityItems.Length; i++)
            {
                if (priorityItems[i].priority == currentPriority)
                {
                    var index = (i - 1) < 0 ? priorityItems.Length - 1 : i - 1;
                    return priorityItems[index];
                }
            }

            throw new System.Exception("Priority not found");
        }

        /// <summary>
        /// Get the priority item based on the specified tower priority.
        /// </summary>
        /// <param name="priority">The tower priority.</param>
        /// <returns>The corresponding priority item.</returns>
        public PriorityItem GetPriorityItem(TowerPriority priority)
        {
            foreach (var priorityItem in priorityItems)
            {
                if (priorityItem.priority == priority)
                {
                    return priorityItem;
                }
            }

            return priorityItems[0];
        }

    }
}