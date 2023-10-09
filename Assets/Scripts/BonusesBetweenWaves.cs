using System.Collections.Generic;
using Enemies;
using Items.BonusCards;
using Towers;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Manages bonuses given between waves.
/// </summary>
public class BonusesBetweenWaves : MonoBehaviour
{
    [SerializeField] private BonusCards bonusCards; 

    private List<AbstractBonusCard> _bonusesCardsRemainingList; // The list of bonuses cards that are not yet used.
    private GameScene _gameScene; // The game scene.
    private const int EveryWaveCount = 2; // The number of waves between bonuses.
    private int _waveCounter; // The number of waves since the last bonuses.
    private float previousTimeScale; // The time scale before the bonuses UI was shown.


    /// <summary>
    /// Initializes the bonuses manager with the provided game scene.
    /// </summary>
    /// <param name="gameScene">The game scene to initialize with.</param>
    public void Initialize(GameScene gameScene)
    {
        _gameScene = gameScene;
    }

    private void Start()
    {
        _bonusesCardsRemainingList = new List<AbstractBonusCard>();

        AddAllCards();

        EnemyWaveGenerator.OnWaveEnded += OnWaveEndedHandler;
        BonusCardUI.OnCardChosen += OnCardChosenHandler;
    }

    /// <summary>
    /// Handles the chosen bonus card.
    /// </summary>
    /// <param name="bonus">The chosen bonus card.</param>
    private void OnCardChosenHandler(AbstractBonusCard bonus)
    {
        if (bonus is BonusTowerCard or BonusTowerNegativeEffectCard)
        {
            _gameScene.GetTowerManager().AddBonusToTower(bonus);
        }
        else if (bonus is BonusGameCard)
        {
            _gameScene.GetEnemyWaveGenerator().AddBonus(bonus);
        }

        Time.timeScale = previousTimeScale;
    }

    /// <summary>
    /// Handles the end of a wave.
    /// </summary>
    /// <param name="obj">The wave number.</param>
    private void OnWaveEndedHandler(int obj)
    {
        _waveCounter++;
        if (_waveCounter % EveryWaveCount != 0) return;

        previousTimeScale = Time.timeScale;
        Time.timeScale = 0;

        AbstractBonusCard[] cards = new AbstractBonusCard[3];

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, _bonusesCardsRemainingList.Count);
            cards[i] = _bonusesCardsRemainingList[randomIndex];

            _bonusesCardsRemainingList.RemoveAt(randomIndex);
        }

        _gameScene.GetGameUI().ShowBonusesUI(cards[0], cards[1], cards[2]);
    }

    /// <summary>
    ///  Adds all the cards to the list of remaining cards.
    /// </summary>
    private void AddAllCards()
    {
        foreach (var card in bonusCards.towerCards)
        {
            _bonusesCardsRemainingList.Add(GetCardForUI(card));
        }

        foreach (var card in bonusCards.towerNegativeEffectCards)
        {
            _bonusesCardsRemainingList.Add(GetCardForUI(card));
        }

        foreach (var card in bonusCards.gameCards)
        {
            _bonusesCardsRemainingList.Add(GetCardForUI(card));
        }
    }

    /// <summary>
    /// Creates a new instance of a bonus card suitable for UI.
    /// </summary>
    /// <param name="card">The original bonus card.</param>
    /// <returns>The bonus card suitable for UI.</returns>
    private AbstractBonusCard GetCardForUI(AbstractBonusCard card)
    {
        AbstractBonusCard cardForUI = null;

        if (card is BonusTowerCard towerCard)
        {
            cardForUI = new BonusTowerCard();
            cardForUI.bonusValue = towerCard.bonusValue;
            cardForUI.uiSprite = towerCard.uiSprite;
            ((BonusTowerCard)cardForUI).bonusType = towerCard.bonusType;
            ((BonusTowerCard)cardForUI).towerType = towerCard.towerType;

            if (towerCard.bonusType is TowerBonusType.DamageHealth or TowerBonusType.DamageArmor
                or TowerBonusType.DamageShield)
            {
                cardForUI.uiText = string.Format(towerCard.uiText, towerCard.bonusValue, towerCard.towerType);
            }
            else
            {
                cardForUI.uiText = string.Format(towerCard.uiText, towerCard.bonusValue * 100, towerCard.towerType);
            }
        }
        else if (card is BonusTowerNegativeEffectCard towerNegativeEffectCard)
        {
            cardForUI = new BonusTowerNegativeEffectCard();
            cardForUI.bonusValue = towerNegativeEffectCard.bonusValue;
            cardForUI.uiSprite = towerNegativeEffectCard.uiSprite;
            ((BonusTowerNegativeEffectCard)cardForUI).bonusType = towerNegativeEffectCard.bonusType;
            ((BonusTowerNegativeEffectCard)cardForUI).towerType = towerNegativeEffectCard.towerType;

            cardForUI.uiText = string.Format(towerNegativeEffectCard.uiText, towerNegativeEffectCard.bonusValue * 100,
                towerNegativeEffectCard.towerType);
        }
        else if (card is BonusGameCard bonusCard)
        {
            cardForUI = new BonusGameCard();
            cardForUI.bonusValue = bonusCard.bonusValue;
            cardForUI.uiSprite = bonusCard.uiSprite;
            ((BonusGameCard)cardForUI).bonusType = bonusCard.bonusType;
            cardForUI.uiText = card.uiText;
        }

        return cardForUI;
    }

    /// <summary>
    ///  Unsubscribes from events. 
    /// </summary>
    private void OnDestroy()
    {
        EnemyWaveGenerator.OnWaveEnded -= OnWaveEndedHandler;
        BonusCardUI.OnCardChosen -= OnCardChosenHandler;
    }
}