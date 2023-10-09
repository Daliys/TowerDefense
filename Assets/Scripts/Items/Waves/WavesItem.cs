using UnityEngine;

namespace Items.Waves
{
    /// <summary>
    /// ScriptableObject representing a collection of wave configurations and timing settings for enemy spawns in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "WavesItem", menuName = "Tower Defense/WavesItem", order = 2)]
    public class WavesItem : ScriptableObject
    {
        /// <summary>
        /// The time delay between enemy spawns within a wave.
        /// </summary>
        public float timeBetweenEnemySpawn;

        /// <summary>
        /// The time delay between waves.
        /// </summary>
        public float timeBetweenWaves;

        /// <summary>
        /// The maximum offset from the center of the tile where an enemy can spawn.
        /// </summary>
        /// <remarks>This affects the initial position of spawned enemies.</remarks>
        public float enemyMaxOffset;

        /// <summary>
        /// An array of WaveItem instances representing individual waves and their configurations.
        /// </summary>
        public WaveItem[] waveItems;
    }
}