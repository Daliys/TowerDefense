using System;

namespace Towers
{
    /// <summary>
    ///  Represents the priority of a tower.
    /// </summary>
    [Serializable]
    public enum TowerPriority
    {
        First,
        Last,
        MostHealth,
        MostArmor,
        MostShield,
        Random
    }
}