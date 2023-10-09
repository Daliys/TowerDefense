using System;
using Towers;

namespace Items.BonusCards
{
    /// <summary>
    ///  Bonus card for tower bonuses
    /// </summary>
    [Serializable]
    public class BonusTowerCard : AbstractBonusCard
    {
        public TowerBonusType bonusType;
        public TowerType towerType;
    }
}