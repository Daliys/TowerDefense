using System;
using System.Collections;
using System.Collections.Generic;
using Items.BonusCards;
using Items.Enemies;
using Items.Waves;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    /// <summary>
    /// Class responsible for generating waves of enemies.
    /// </summary>
    public class EnemyWaveGenerator : MonoBehaviour
    {
        #region Variables

        // Events for wave and game events
        public static event Action<int> OnWaveStarted; // Event triggered when a new wave starts
        public static event Action<int> OnWaveEnded; // Event triggered when a wave ends
        public static event Action OnWin; // Event triggered when the player wins

        // Serialized field for enemy item data
        [SerializeField] private EnemiesItem enemiesItem;

        // Private variables for wave generation and enemy tracking
        private WavesItem _wavesItem; // Data for wave generation
        private Dictionary<EnemyType, int> _enemyTypeSpawnValues; // Spawn values for each enemy type
        private Dictionary<GameBonus, float> _bonusGameCards; // Bonus values associated with game bonuses
        private List<Enemy> _enemies; // List of active enemies
        private int _currentWave; // Current wave number
        private TiledMap _tiledMap; // Reference to the tiled map
        private Coroutine loopCoroutine; // Coroutine for controlling wave generation

        #endregion

        private void Awake()
        {
            GameUI.OnStartLoadingNewScene += OnStartLoadingNewSceneHandler;
        }

        /// <summary>
        /// Event handler to stop loopCoroutine when loading a new scene
        /// </summary>
        private void OnStartLoadingNewSceneHandler()
        {
            StopCoroutine(loopCoroutine);
        }

        /// <summary>
        /// Starts the generation of waves of enemies.
        /// </summary>
        public void StartGenerate(WavesItem wavesItem, TiledMap tiledMap)
        {
            // Initialization of variables
            _wavesItem = wavesItem;
            _tiledMap = tiledMap;
            _enemyTypeSpawnValues = new Dictionary<EnemyType, int>();
            _enemies = new List<Enemy>();
            _bonusGameCards = new Dictionary<GameBonus, float>();

            // cash enemy spawn values and enemy types to reduce the number of calls searching for them in EnemiesItem
            foreach (var enemyItem in enemiesItem.enemiesItems)
            {
                _enemyTypeSpawnValues.Add(enemyItem.enemyType, enemyItem.spawnValue);
            }

            EnemyHealth.OnEnemyDied += OnEnemyDied;
            Enemy.OnEnemyReachedEnd += OnEnemyReachedEnd;
            loopCoroutine = StartCoroutine(LoopCoroutine());
        }

        /// <summary>
        /// Method to handle when an enemy reaches the end of the path
        /// </summary>
        /// <param name="enemy">The enemy that reached the end</param>
        private void OnEnemyReachedEnd(Enemy enemy)
        {
            _enemies.Remove(enemy);
            ObjectPooling.instance.ReturnToPool(enemy);
        }

        /// <summary>
        /// Method to handle when an enemy dies
        /// </summary>
        /// <param name="enemy">The enemy that died</param>
        private void OnEnemyDied(Enemy enemy)
        {
            _enemies.Remove(enemy);
            ObjectPooling.instance.ReturnToPool(enemy);
        }

        /// <summary>
        /// Method to add a bonus to the game
        /// </summary>
        /// <param name="bonus">The bonus to add</param>
        public void AddBonus(AbstractBonusCard bonus)
        {
            if (bonus is not BonusGameCard bonusGameCard) return;

            if (_bonusGameCards.ContainsKey(bonusGameCard.bonusType))
            {
                _bonusGameCards[bonusGameCard.bonusType] += bonusGameCard.bonusValue;
            }
            else
            {
                _bonusGameCards.Add(bonusGameCard.bonusType, bonusGameCard.bonusValue);
            }
        }

        /// <summary>
        /// Generates an array of enemyType randomly based on the specified value.
        /// Initially adds all possible enemy types to the final enemy list to ensure at least one enemy of each type is generated.
        /// Iterates through the enemies that can be generated and selects those that fit the generation criteria.
        /// If suitable enemies can't be found and there's unused value left, adds an enemy with the lowest generation cost.
        /// If suitable enemies are found during iteration, adds a random enemy to the final list.
        /// This process continues until all value for generating enemies is exhausted.
        /// </summary>
        /// <param name="typeOfEnemiesInWave">An array of enemyTypes that can be generated.</param>
        /// <param name="waveValue">The value of the wave (generation of enemies depends on this value).</param>
        /// <returns>A list of EnemyItem instances that have been generated and shuffled randomly.</returns>
        private List<EnemyType> GenerateListOfEnemies(EnemyType[] typeOfEnemiesInWave, float waveValue)
        {
            float currentValue = 0;
            List<EnemyType> list = new List<EnemyType>();
            list.AddRange(typeOfEnemiesInWave);

            // Add at least one enemy of each type
            foreach (EnemyType enemyItem in typeOfEnemiesInWave)
            {
                currentValue += GetSpawnValueOfEnemyType(enemyItem);
            }

            // Add enemies until the value of the wave is reached
            while (currentValue < waveValue)
            {
                List<EnemyType> availableToGenerate = new List<EnemyType>();
                float remainingValue = waveValue - currentValue;

                // Add enemies that can be generated
                foreach (EnemyType enemyItem in typeOfEnemiesInWave)
                {
                    if (remainingValue > GetSpawnValueOfEnemyType(enemyItem))
                    {
                        availableToGenerate.Add(enemyItem);
                    }
                }

                if (availableToGenerate.Count == 0 && remainingValue > 0)
                {
                    list.Add(GetLowestCostEnemyTypeToGenerate(typeOfEnemiesInWave));
                    break;
                }

                int randomElement = Random.Range(0, availableToGenerate.Count);
                EnemyType chosenItem = availableToGenerate[randomElement];
                list.Add(chosenItem);
                currentValue += GetSpawnValueOfEnemyType(chosenItem);
            }


            // Shuffle the list
            for (int i = 0; i < list.Count; i++)
            {
                EnemyType temp = list[i];
                int randomIndex = Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }

            return list;
        }

        /// <summary>
        /// Gets the enemy type with the lowest generation cost to generate.
        /// </summary>
        /// <param name="listOfEnemies">The array of enemy types to consider.</param>
        /// <returns>The enemy type with the lowest generation cost.</returns>
        private EnemyType GetLowestCostEnemyTypeToGenerate(EnemyType[] listOfEnemies)
        {
            int lowestCostEnemyId = -1;
            float lowestCost = float.MaxValue;

            for (int i = 0; i < listOfEnemies.Length; i++)
            {
                int spawnValue = GetSpawnValueOfEnemyType(listOfEnemies[i]);

                if (spawnValue < lowestCost)
                {
                    lowestCost = spawnValue;
                    lowestCostEnemyId = i;
                }
            }

            return listOfEnemies[lowestCostEnemyId];
        }

        /// <summary>
        /// Gets the spawn value of a specific enemy type.
        /// </summary>
        /// <param name="enemyType">The type of enemy.</param>
        /// <returns>The spawn value of the enemy type.</returns>
        private int GetSpawnValueOfEnemyType(EnemyType enemyType)
        {
            return _enemyTypeSpawnValues[enemyType];
        }

        /// <summary>
        /// Coroutine for generating enemies in a wave.
        /// </summary>
        private IEnumerator LoopCoroutine()
        {
            while (_wavesItem.waveItems.Length > _currentWave)
            {
                OnWaveStarted?.Invoke(_currentWave);

                WaveItem waveItem = _wavesItem.waveItems[_currentWave];
                List<EnemyType> enemyItems = GenerateListOfEnemies(waveItem.enemyTypes, waveItem.waveGenerationPoints);

                yield return StartCoroutine(GenerateEnemies(enemyItems));

                OnWaveEnded?.Invoke(_currentWave);
                yield return new WaitForSeconds(_wavesItem.timeBetweenWaves);
                _currentWave++;
            }

            OnWin?.Invoke();
        }

        /// <summary>
        /// Coroutine for generating enemies in a wave.
        /// </summary>
        private IEnumerator GenerateEnemies(List<EnemyType> enemyTypes)
        {
            foreach (var enemyType in enemyTypes)
            {
                yield return new WaitForSeconds(_wavesItem.timeBetweenEnemySpawn);

                EnemyItem enemyItem = enemiesItem.GetEnemyItem(enemyType);

                Enemy enemy = ObjectPooling.instance.GetObject(enemyItem.enemyType).GetComponent<Enemy>();
                float offset = Random.Range(-_wavesItem.enemyMaxOffset, _wavesItem.enemyMaxOffset);
                enemy.Initialize(this, enemyItem, _tiledMap, offset);
                _enemies.Add(enemy);
            }
        }

        /// <summary>
        /// Handles cleanup when the object is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            EnemyHealth.OnEnemyDied -= OnEnemyDied;
            Enemy.OnEnemyReachedEnd -= OnEnemyReachedEnd;
            GameUI.OnStartLoadingNewScene -= OnStartLoadingNewSceneHandler;
        }

        /// <summary>
        /// Gets the bonus value associated with a specific game bonus type.
        /// </summary>
        /// <param name="type">The game bonus type.</param>
        /// <returns>The bonus value for the specified game bonus type.</returns>
        public float GetBonusValue(GameBonus type)
        {
            if (_bonusGameCards.TryGetValue(type, out var value))
            {
                return value;
            }

            return 0;
        }

        /// <summary>
        /// Gets the list of active enemies.
        /// </summary>
        /// <returns>The list of active enemies.</returns>
        public List<Enemy> GetEnemies() => _enemies;
    }
}