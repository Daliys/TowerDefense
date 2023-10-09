using System;

/// <summary>
///  Enum for game bonuses. Used to determine which type of bonus to give.
/// </summary>
[Serializable]
public enum GameBonus
{
    MoneyForKill,
    IncreaseBleedDamage,
    IncreaseFireDamage,
    IncreasePoisonDamage
}