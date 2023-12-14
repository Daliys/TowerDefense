using System;
using Enemies;
using Towers;
using UI;
using UnityEngine;

/// <summary>
///  Class that managing the game values as health, coins, score and waves.
/// </summary>
public class Game
{
    /// <summary>
    /// Event triggered when the amount of coins changes.
    /// </summary>
    public static event Action<int> OnAmountOfCoinsChanged;

    /// <summary>
    /// Event triggered when the amount of health changes.
    /// </summary>
    public static event Action<int> OnAmountOfHealthChanged;

    /// <summary>
    /// Event triggered when the game ends.
    /// </summary>
    public static event Action OnGameEnded;

    private int _coins; // The amount of coins the player has.
    private int _health; // The amount of health the player has.
    private int _wave; // The current wave the player is in.

    private int _gameScore; // The score of the player.

    /// <summary>
    /// Constructor for the Game class.
    /// </summary>
    public Game()
    {
        _coins = 0;
        _health = 10;

        OnAmountOfCoinsChanged?.Invoke(_coins);
        OnAmountOfHealthChanged?.Invoke(_health);

        OnDestroy();

        EnemyHealth.OnEnemyDied += OnEnemyDied;
        Enemy.OnEnemyReachedEnd += OnEnemyReachedEnd;
        TowerInformationPanelUI.OnSellTowerButtonClicked += OnSellTowerButtonClicked;
        GameUI.OnStartLoadingNewScene += OnDestroy;

    }

    /// <summary>
    /// Event handler for when a tower is sold.
    /// </summary>
    /// <param name="obj">The tower being sold.</param>
    private void OnSellTowerButtonClicked(AbstractTower obj)
    {
        AddCoins(obj.GetTowerSellCost());
    }

    /// <summary>
    /// Event handler for when an enemy reaches the end.
    /// </summary>
    /// <param name="enemy">The enemy that reached the end.</param>
    private void OnEnemyReachedEnd(Enemy enemy)
    {
        RemoveHealth(enemy.GetEnemyDamage());
    }

    /// <summary>
    /// Event handler for when an enemy is defeated.
    /// </summary>
    /// <param name="enemy">The defeated enemy.</param>
    private void OnEnemyDied(Enemy enemy)
    {
        AddCoins(enemy.GetCoinsCount());
        _gameScore += enemy.GetGameScoreValue();
    }

    /// <summary>
    /// Removes health from the player.
    /// </summary>
    /// <param name="amount">The amount of health to remove.</param>
    private void RemoveHealth(int amount)
    {
        _health -= amount;
        _health = Mathf.Max(_health, 0);
        OnAmountOfHealthChanged?.Invoke(_health);

        if (_health == 0)
        {
            Time.timeScale = 0;
            OnGameEnded?.Invoke();
        }
    }

    /// <summary>
    /// Adds coins to the player's total.
    /// </summary>
    /// <param name="amount">The amount of coins to add.</param>
    public void AddCoins(int amount)
    {
        _coins += amount;
        OnAmountOfCoinsChanged?.Invoke(_coins);
    }

    /// <summary>
    /// Removes coins from the player's total.
    /// </summary>
    /// <param name="amount">The amount of coins to remove.</param>
    public void RemoveCoins(int amount)
    {
        _coins -= amount;
        OnAmountOfCoinsChanged?.Invoke(_coins);
    }

    /// <summary>
    /// Checks if there are enough coins to buy an item.
    /// </summary>
    /// <param name="amount">The amount of coins needed.</param>
    public bool IsEnoughCoinsToBuy(int amount)
    {
        return _coins >= amount;
    }

    /// <summary>
    ///  Clears all the events.
    /// </summary>
    private void OnDestroy()
    {
        EnemyHealth.OnEnemyDied -= OnEnemyDied;
        Enemy.OnEnemyReachedEnd -= OnEnemyReachedEnd;
        TowerInformationPanelUI.OnSellTowerButtonClicked -= OnSellTowerButtonClicked;
        GameUI.OnStartLoadingNewScene -= OnDestroy;
    }
}
