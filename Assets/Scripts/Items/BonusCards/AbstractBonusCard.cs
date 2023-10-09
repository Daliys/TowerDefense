using System;
using UnityEngine;

namespace Items.BonusCards
{
    /// <summary>
    /// Abstract class for bonus cards, which are used to give the player a bonus when they are picked up
    /// </summary>
    [Serializable]
    public class AbstractBonusCard
    {
        public string uiText;
        public Sprite uiSprite;
        public float bonusValue;
    }
}