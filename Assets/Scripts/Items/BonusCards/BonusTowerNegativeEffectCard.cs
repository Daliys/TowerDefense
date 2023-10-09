using System;
using Enemies;
using Towers;

namespace Items.BonusCards
{
    /// <summary>
    ///  Bonus card for tower negative effects
    /// </summary>
    [Serializable]
    public class BonusTowerNegativeEffectCard : AbstractBonusCard
    {
        public EnemyNegativeEffectType bonusType;
        public TowerType towerType;
    }
}