using System.Linq;
using Enemies;
using UnityEngine;

namespace Items.Enemies
{
    /// <summary>
    /// Scriptable object containing data for UI representations of negative effects
    /// </summary>
    [ CreateAssetMenu(fileName = "NegativeEffectsUIItem", menuName = "Tower Defense/NegativeEffectsUIItem") ]
    public class NegativeEffectsUIItem : ScriptableObject
    {
        /// <summary>
        /// Array of UI representations for different negative effects
        /// </summary>
        public NegativeEffectUIItem[] negativeEffectsUIItems;
        
        /// <summary>
        /// Gets the icon associated with a specific negative effect type
        /// </summary>
        /// <param name="type">The negative effect type.</param>
        /// <returns>The icon representing the specified negative effect type, or null if not found</returns>
        public Sprite GetIcon(EnemyNegativeEffectType type)
        {
            return (from negativeEffectUIItem in negativeEffectsUIItems where negativeEffectUIItem.type == type select negativeEffectUIItem.icon).FirstOrDefault();
        }
    }
}