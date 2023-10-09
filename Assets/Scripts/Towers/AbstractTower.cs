using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Items.BonusCards;
using Items.Towers;
using UI;
using UnityEngine;

namespace Towers
{
    /// <summary>
    /// Abstract base class representing a tower in the game
    /// </summary>
    public abstract class AbstractTower : MonoBehaviour
    {
        #region Properties

        public event Action<AbstractTower> OnTowerDataChanged; // Event triggered when tower data changes

        [SerializeField] protected TowerType towerType; // The type of this tower
        [SerializeField] protected GameObject rotationImage; // Rotation visual indicator
        [SerializeField] protected GameObject towerRangeImage; // Tower range visual indicator
        [SerializeField] protected StartTowerBonus[] startTowerBonus; // Initial bonuses for the tower

        protected TowerManager towerManager; // Reference to the tower manager
        private TowerPriority currentPriority; // Current attack priority of the tower
        private ApplyingDamage applyingDamage; // Damage application manager
        private TowerBonuses towerBonuses; // Bonuses applied to the tower
        private TowerExperiences towerExperiences; // Tower experience manager

        private TowerItem item; // Tower item data
        private Coroutine _attackCoroutine; // Coroutine for tower attacks
        private float _baseRange; // Base range of the tower
        private float _baseFireRate; // Base fire rate of the tower

        private bool _isActive = true; // Flag indicating if the tower is active

        #endregion

        private void Awake()
        {
            GameUI.OnStartLoadingNewScene += OnStartLoadingNewSceneHandler;
        }

        /// <summary>
        /// Event handler for the GameUI.OnStartLoadingNewScene event. Sets the tower as inactive
        /// </summary>
        private void OnStartLoadingNewSceneHandler()
        {
            _isActive = false;
        }

        /// <summary>
        /// Initializes the tower with specified tower item and priority.
        /// </summary>
        /// <param name="manager">The tower manager.</param>
        /// <param name="towerItem">The tower item data.</param>
        /// <param name="priority">The attack priority of the tower.</param>
        public void InitializeTower(TowerManager manager, TowerItem towerItem, TowerPriority priority)
        {
            towerManager = manager;
            item = towerItem;
            currentPriority = priority;

            InitializeComponents();

            BonusCardUI.OnCardChosen += OnCardChosen;
            TowerInformationPanelUI.OnUpdateDamageButtonClicked += OnUpdateDamageButtonClicked;
        }

        /// <summary>
        /// Initializes the components of the tower based on the tower item.
        /// </summary>
        private void InitializeComponents()
        {
            _baseRange = item.range;
            _baseFireRate = item.fireRate;

            towerBonuses = new TowerBonuses();
            applyingDamage = new ApplyingDamage(towerBonuses, item);
            towerExperiences = new TowerExperiences(this);

            foreach (var bonus in startTowerBonus)
            {
                towerBonuses.AddBonusNegativeEffectForEnemy(bonus.effectType, bonus.value);
            }

            AfterInitialization();
            StartCoroutine(AttackCoroutine());
        }


        private void OnUpdateDamageButtonClicked(AbstractTower tower, DamageType damageType)
        {
            if (tower != this) return;

            towerExperiences.UpgradeDamageToNextLevel(damageType);
        }

        /// <summary>
        /// Invoked when a bonus card is chosen for the tower.
        /// </summary>
        /// <param name="bonus">The bonus card chosen.</param>
        private void OnCardChosen(AbstractBonusCard bonus)
        {
            if (bonus is not (BonusTowerCard or BonusTowerNegativeEffectCard)) return;

            // Switch-case block to handle different types of bonus cards.
            switch (bonus)
            {
                // If the bonus is a BonusTowerCard or BonusTowerNegativeEffectCard and it's not applicable to this tower type, do nothing and return.
                case BonusTowerCard towerCard when towerCard.towerType != towerType:
                case BonusTowerNegativeEffectCard negativeEffectCard when negativeEffectCard.towerType != towerType:
                    return;
                default:
                    // If the bonus is applicable to this tower type, add the bonus card to the tower's bonuses.
                    towerBonuses.AddBonusCard(bonus);
                    break;
            }

            OnTowerDataChanged?.Invoke(this);
        }

        /// <summary>
        ///  Invoked after the tower is initialized. Can be overridden in derived classes.
        /// </summary>
        protected virtual void AfterInitialization()
        {
        }

        /// <summary>
        /// Adds a list of bonus cards to the tower.
        /// </summary>
        /// <param name="bonuses">List of AbstractBonusCard instances to be added to the tower.</param>
        public void AddBonuses(List<AbstractBonusCard> bonuses)
        {
            foreach (AbstractBonusCard bonus in bonuses)
            {
                OnCardChosen(bonus);
            }
        }

        /// <summary>
        /// Sets the attack priority for the tower. The priority determines which enemies the tower will target first.
        /// </summary>
        /// <param name="priority">The attack priority for the tower.</param>
        public void SetPriority(TowerPriority priority)
        {
            currentPriority = priority;
        }

        /// <summary>
        /// This method is intended to be overridden by subclasses to define the attack behavior for the tower.
        /// </summary>
        protected abstract void Attack();

        /// <summary>
        /// Coroutine for the tower's attack behavior.
        /// </summary>
        /// <returns> an IEnumerator for the coroutine.</returns>
        private IEnumerator AttackCoroutine()
        {
            while (_isActive)
            {
                Attack();
                yield return new WaitForSeconds(1 / GetTotalTowerFireRate());
            }
        }

        /// <summary>
        /// Sets the visibility of the tower's attack range indicator.
        /// </summary>
        /// <param name="isVisible">Indicates whether the range indicator should be visible or not.</param>
        public void SetVisibleRange(bool isVisible)
        {
            towerRangeImage.SetActive(isVisible);
            towerRangeImage.transform.localScale = new Vector3(GetTotalTowerRange(), GetTotalTowerRange(), 1);
        }

        /// <summary>
        /// This method is called when the GameObject is destroyed. Unsubscribes from events. Stops the attack coroutine
        /// </summary>
        private void OnDestroy()
        {
            BonusCardUI.OnCardChosen -= OnCardChosen;
            TowerInformationPanelUI.OnUpdateDamageButtonClicked -= OnUpdateDamageButtonClicked;

            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
            }
        }

        /// <summary>
        /// Creates an instance of ApplyingDamageToEnemy to represent the damage to be applied to enemies.
        /// </summary>
        /// <returns>Returns an instance of ApplyingDamageToEnemy.</returns>
        protected ApplyingDamageToEnemy GetApplyingDamageToEnemy()
        {
            return new ApplyingDamageToEnemy(applyingDamage.GetTotalHealthDamage(),
                applyingDamage.GetTotalArmorDamage(),
                applyingDamage.GetTotalShieldDamage(), applyingDamage.GetNegativeEffects(), towerExperiences);
        }

        /// <summary>
        /// Invokes the OnTowerDataChanged event, notifying subscribers that the tower's data has changed.
        /// </summary>
        public void InvokeOnTowerDataChanged()
        {
            OnTowerDataChanged?.Invoke(this);
        }

        /// <summary>
        /// Unsubscribes from events when the GameObject is disabled to prevent unwanted behavior.
        /// </summary>
        private void OnDisable()
        {
            GameUI.OnStartLoadingNewScene -= OnStartLoadingNewSceneHandler;
        }

        #region Getters

        /// <summary>
        /// Returns the type of the tower.
        /// </summary>
        public TowerType GetTowerType() => towerType;

        /// <summary>
        /// Returns the name of the tower.
        /// </summary>
        public string GetTowerName() => item.towerName;

        /// <summary>
        /// Returns the attack priority of the tower.
        /// </summary>
        public TowerPriority GetTowerAttackPriority() => currentPriority;

        /// <summary>
        /// Returns the applying damage information for the tower.
        /// </summary>
        public ApplyingDamage GetTowerApplyingDamage() => applyingDamage;

        /// <summary>
        /// Returns the base range of the tower.
        /// </summary>
        public float GetBaseTowerRange() => _baseRange;

        /// <summary>
        /// Returns the total range of the tower, accounting for bonuses.
        /// </summary>
        public float GetTotalTowerRange() => _baseRange + towerBonuses.GetRange();

        /// <summary>
        /// Returns the formatted text for displaying the tower's range in the UI.
        /// </summary>
        public string GetTowerRangeForUI()
        {
            if (towerBonuses.GetRange() == 0) return $"{_baseRange:F1}";
            return $"{_baseRange:F1} + ({towerBonuses.GetRange():F1})";
        }

        /// <summary>
        /// Returns the base fire rate of the tower.
        /// </summary>
        public float GetBaseTowerFireRate() => _baseFireRate;

        /// <summary>
        /// Returns the total fire rate of the tower, accounting for bonuses.
        /// </summary>
        public float GetTotalTowerFireRate() => _baseFireRate + towerBonuses.GetFireRate();

        /// <summary>
        /// Returns the formatted text for displaying the tower's fire rate in the UI.
        /// </summary>
        public string GetTowerFireRateForUI()
        {
            if (towerBonuses.GetFireRate() == 0) return $"{_baseFireRate:F1}";
            return $"{_baseFireRate:F1} + ({towerBonuses.GetFireRate():F1})";
        }

        /// <summary>
        /// Returns the experience multiplier for the tower.
        /// </summary>
        public float GetExperienceMultiplier() => towerExperiences.GetExperienceMultiplier();

        /// <summary>
        /// Returns the bonuses associated with the tower.
        /// </summary>
        public TowerBonuses GetTowerBonuses() => towerBonuses;

        /// <summary>
        /// Returns the experiences associated with the tower.
        /// </summary>
        public TowerExperiences GetTowerExperiences() => towerExperiences;

        /// <summary>
        /// Returns the item information for the tower.
        /// </summary>
        public TowerItem GetTowerItem() => item;

        /// <summary>
        /// Returns the sell cost for the tower.
        /// </summary>
        public int GetTowerSellCost() => towerManager.GetTowerSellCost(towerType);

        /// <summary>
        /// Returns the game associated with this tower.
        /// </summary>
        public Game GetGame() => towerManager.GetGameScene.GetGame();

        #endregion

        /// <summary>
        /// Serializable class to hold the initial bonuses for the tower.
        /// </summary>
        [Serializable]
        public class StartTowerBonus
        {
            /// <summary>
            /// The type of negative effect applied by the bonus.
            /// </summary>
            public EnemyNegativeEffectType effectType;

            /// <summary>
            /// The value of the bonus for the effect.
            /// </summary>
            public float value;
        }
    }
}