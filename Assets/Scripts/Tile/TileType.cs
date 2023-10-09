using System;

namespace Tile
{
    /// <summary>
    ///  Enum representing the type of a tile.
    /// </summary>
    [Serializable]
    public enum TileType
    {
        Portal,
        BonusAttack,
        Empty,
        BonusXp,
        BonusFireRate,
        Regular,
        BonusRange,
        Spawn,
        Road
    }
}