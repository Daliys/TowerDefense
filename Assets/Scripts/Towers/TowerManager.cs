using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Items.BonusCards;
using Items.Towers;
using Tile;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Towers
{
    /// <summary>
    /// Manages towers and their interactions within the game.
    /// </summary>
    public class TowerManager : MonoBehaviour
    {

        [SerializeField] private TowersInformationItem towersInformationItem; // The towers information item containing information about all towers.

        private List<AbstractTower> _towers; // The list of towers in the game.
        private GameScene _gameScene; // The associated game scene.
        private List<AbstractBonusCard> _bonusesForTowers; // The list of bonuses for towers.

        private void Awake()
        {
            _towers = new List<AbstractTower>();
            _bonusesForTowers = new List<AbstractBonusCard>();

            UITowerButton.OnTowerButtonClicked += OnTowerButtonClicked;
            TowerInformationPanelUI.OnPriorityButtonClicked += OnPriorityButtonClicked;
            TowerInformationPanelUI.OnSellTowerButtonClicked += OnSellTowerButtonClicked;
            TowerInformationPanelUI.OnUpdateDamageButtonClicked += OnUpdateDamageButtonClicked;
        }

        /// <summary>
        /// Handles the event when the update damage button is clicked for a tower.
        /// </summary>
        /// <param name="tower">The tower being upgraded.</param>
        /// <param name="damageType">The damage type being upgraded.</param>
        private void OnUpdateDamageButtonClicked(AbstractTower tower, DamageType damageType)
        {
            _gameScene.GetGame().RemoveCoins(tower.GetTowerExperiences().GetUpgradeCost(damageType));
        }

        /// <summary>
        /// Destroys the tower and removes it from the tower list.
        /// </summary>
        /// <param name="tower">The tower to be sold.</param>
        private void OnSellTowerButtonClicked(AbstractTower tower)
        {
            _towers.Remove(tower);
            Destroy(tower.gameObject);
        }

        /// <summary>
        /// Event handler for when a priority button is clicked for a tower.
        /// Sets the priority of the installed tower on the selected tile.
        /// </summary>
        /// <param name="obj">The TowerPriority enum value representing the priority.</param>
        private void OnPriorityButtonClicked(TowerPriority obj)
        {
            TileInformation tileInformation = _gameScene.GetTileMap().GetSelectedTileInformation();

            if (tileInformation == null) throw new Exception("No tile selected");

            tileInformation.GetInstalledTower().SetPriority(obj);
        }

        /// <summary>
        /// Initializes the TowerManager with the specified GameScene.
        /// </summary>
        /// <param name="game">The GameScene to associate with this TowerManager.</param>
        public void Initialize(GameScene game)
        {
            _gameScene = game;
        }

        /// <summary>
        /// Event handler for when a tower button is clicked to build a tower.
        /// Instantiates and initializes the tower if the player has enough coins.
        /// </summary>
        /// <param name="obj">The type of tower to build.</param>
        /// <param name="priority">The priority for the tower.</param>
        private void OnTowerButtonClicked(TowerType obj, TowerPriority priority)
        {
            int towerCost = GetTowerCost(obj);

            if (_gameScene.GetGame().IsEnoughCoinsToBuy(towerCost))
            {
                TowerItem towerItem = towersInformationItem.GetTowerItem(obj);

                AbstractTower tower = Instantiate(towerItem.prefab).GetComponent<AbstractTower>();
                tower.InitializeTower(this, towerItem, priority);
                tower.AddBonuses(_bonusesForTowers);

                _towers.Add(tower);

                _gameScene.GetTileMap().BuildTower(tower);
                _gameScene.GetGame().RemoveCoins(towerCost);
                _gameScene.GetTileMap().CallOnTileClicked();
            }
        }

        /// <summary>
        /// Adds a bonus card to the list of bonuses for the towers.
        /// </summary>
        /// <param name="bonusCard">The bonus card to add.</param>
        public void AddBonusToTower(AbstractBonusCard bonusCard)
        {
            _bonusesForTowers.Add(bonusCard);
        }

        /// <summary>
        /// Gets an enemy within the tower's range based on the tower's attack priority.
        /// </summary>
        /// <param name="tower">The tower to find an enemy for.</param>
        /// <returns>An enemy based on the tower's attack priority, or null if no enemies are in range.</returns>
        public Enemy GetEnemyInTowerRange(AbstractTower tower)
        {
            List<Enemy> enemies = GetAllEnemiesInTowerRange(tower);
            if (enemies.Count == 0) return null;

            Enemy enemyToReturn = null;
            float value;

            switch (tower.GetTowerAttackPriority())
            {
                case TowerPriority.First:
                    value = float.MinValue;
                    foreach (var enemy in enemies.Where(enemy => enemy.GetPathSequenceRank() > value))
                    {
                        value = enemy.GetPathSequenceRank();
                        enemyToReturn = enemy;
                    }

                    return enemyToReturn;

                case TowerPriority.Last:
                    value = float.MaxValue;
                    foreach (var enemy in enemies.Where(enemy => enemy.GetPathSequenceRank() < value))
                    {
                        value = enemy.GetPathSequenceRank();
                        enemyToReturn = enemy;
                    }

                    return enemyToReturn;

                case TowerPriority.MostHealth:
                    value = float.MinValue;
                    foreach (var enemy in enemies.Where(enemy => enemy.GetEnemyHealth().GetCurrentHealthPoint() > value))
                    {
                        value = enemy.GetEnemyHealth().GetCurrentHealthPoint();
                        enemyToReturn = enemy;
                    }

                    return enemyToReturn;

                case TowerPriority.MostArmor:
                    value = float.MinValue;
                    foreach (var enemy in enemies.Where(enemy => enemy.GetEnemyHealth().GetCurrentArmorPoint() > value))
                    {
                        value = enemy.GetEnemyHealth().GetCurrentArmorPoint();
                        enemyToReturn = enemy;
                    }

                    return enemyToReturn;

                case TowerPriority.MostShield:
                    value = float.MinValue;
                    foreach (var enemy in enemies.Where(enemy => enemy.GetEnemyHealth().GetCurrentShieldPoint() > value))
                    {
                        value = enemy.GetEnemyHealth().GetCurrentShieldPoint();
                        enemyToReturn = enemy;
                    }

                    return enemyToReturn;

                case TowerPriority.Random: 
                    return enemies[Random.Range(0, enemies.Count)];

                default: throw new Exception("Unknown tower priority");
            }
        }

        /// <summary>
        /// Retrieves a list of enemies within the range of the specified tower.
        /// </summary>
        /// <param name="tower">The tower to find enemies for.</param>
        /// <returns>A list of enemies within the tower's range.</returns>
        public List<Enemy> GetAllEnemiesInTowerRange(AbstractTower tower)
        {
            List<Enemy> enemies = new List<Enemy>();

            float towerRange = tower.GetTotalTowerRange();
            towerRange += 0.25f; // To make sure that the enemy is in the tower range


            foreach (Enemy enemyInScene in _gameScene.GetEnemyWaveGenerator().GetEnemies())
            {
                float distance = Vector3.Distance(enemyInScene.transform.position, tower.transform.position);
                if (distance <= towerRange)
                {
                    enemies.Add(enemyInScene);
                }
            }

            return enemies;
        }

        /// <summary>
        /// Calculates the cost to build a tower of the specified type, taking into account the current number of towers of the same type.
        /// </summary>
        /// <param name="towerType">The type of the tower for which to calculate the cost.</param>
        /// <returns>The cost to build a tower of the specified type.</returns>
        public int GetTowerCost(TowerType towerType)
        {
            int cost = towersInformationItem.GetTowerItem(towerType).cost;
            int towerCount = GetTowerCount(towerType);
            return (int)(cost * Math.Pow(towersInformationItem.GetTowerItem(towerType).costMultiplier, towerCount));
        }

        /// <summary>
        /// Calculates the sell price for a tower of the specified type based on its build cost.
        /// </summary>
        /// <param name="towerType">The type of the tower for which to calculate the sell price.</param>
        /// <returns>The sell price of a tower of the specified type.</returns>
        public int GetTowerSellCost(TowerType towerType)
        {
            return (int)(GetTowerCost(towerType) * 0.85f);
        }

        /// <summary>
        /// Counts the number of towers of the specified type that have been built.
        /// </summary>
        /// <param name="towerType">The type of the tower to count.</param>
        /// <returns>The count of towers of the specified type.</returns>
        private int GetTowerCount(TowerType towerType)
        {
            int count = 0;
            foreach (AbstractTower tower in _towers)
            {
                if (tower.GetTowerType() == towerType)
                {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// Event handler called when the component is about to be destroyed.
        /// Unsubscribes from UI events to prevent memory leaks.
        /// </summary>
        private void OnDestroy()
        {
            UITowerButton.OnTowerButtonClicked -= OnTowerButtonClicked;
            TowerInformationPanelUI.OnPriorityButtonClicked -= OnPriorityButtonClicked;
            TowerInformationPanelUI.OnSellTowerButtonClicked -= OnSellTowerButtonClicked;
            TowerInformationPanelUI.OnUpdateDamageButtonClicked -= OnUpdateDamageButtonClicked;
        }

        /// <summary>
        /// Gets the associated game scene.
        /// </summary>
        public GameScene GetGameScene => _gameScene;
    }
}
