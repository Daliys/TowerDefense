using UnityEngine;

namespace Items.BonusCards
{
    /// <summary>
    /// Scriptable object for bonus cards, which are used to give the player a bonus when they are picked up
    /// </summary>
    [ CreateAssetMenu(fileName = "BonusCards", menuName = "Tower Defense/BonusCards")]
    public class BonusCards : ScriptableObject
    {
        public BonusTowerCard[] towerCards;
        public BonusTowerNegativeEffectCard[] towerNegativeEffectCards;
        public BonusGameCard[] gameCards;
    }
}