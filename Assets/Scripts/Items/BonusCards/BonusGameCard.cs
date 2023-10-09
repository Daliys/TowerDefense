using System;

namespace Items.BonusCards
{
    /// <summary>
    ///  Bonus card for game bonuses
    /// </summary>
    [Serializable]
    public class BonusGameCard : AbstractBonusCard
    {
        public GameBonus bonusType;
    }
}