using System;

namespace Enemies
{
    /// <summary>
    /// Enumeration of negative effects that can be applied to enemies
    /// </summary>
    [Serializable]
    public enum EnemyNegativeEffectType
    {
        Bleeding,
        Fire,
        Poison,
        Slowdown,
    }
}