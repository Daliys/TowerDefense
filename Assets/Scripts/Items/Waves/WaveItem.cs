using System;
using Enemies;

namespace Items.Waves
{
    /// <summary>
    /// Represents a wave configuration in the game.
    /// </summary>
    [Serializable]
    public class WaveItem
    {
        /// <summary>
        /// The generation points for this wave, influencing the type and number of enemies to spawn.
        /// </summary>
        public int waveGenerationPoints;

        /// <summary>
        /// An array of EnemyType representing the types of enemies in this wave.
        /// </summary>
        public EnemyType[] enemyTypes;
    }
}