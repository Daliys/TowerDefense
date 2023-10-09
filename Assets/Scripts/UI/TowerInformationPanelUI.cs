using System;
using Tile;
using TMPro;
using Towers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Manages the tower information panel in the UI.
    /// </summary>
    public class TowerInformationPanelUI : AbstractPanelUI
    {
        #region Variables
        
        public static event Action<AbstractTower,DamageType> OnUpdateDamageButtonClicked; 
        public static event Action<TowerPriority> OnPriorityButtonClicked;
        public static event Action<AbstractTower> OnSellTowerButtonClicked;

        [SerializeField] private TextMeshProUGUI towerNameText;
        
        [SerializeField] private TextMeshProUGUI towerLevelText;
        
        [SerializeField] private TextMeshProUGUI baseDamageText;
        [SerializeField] private TextMeshProUGUI healthMultiplierText;
        [SerializeField] private TextMeshProUGUI armorMultiplierText;
        [SerializeField] private TextMeshProUGUI shieldMultiplierText;
        [SerializeField] private TextMeshProUGUI rangeText;
        [SerializeField] private TextMeshProUGUI fireRateText;
        
        [SerializeField] private TextMeshProUGUI bloodBuffText;
        [SerializeField] private TextMeshProUGUI fireBuffText;
        [SerializeField] private TextMeshProUGUI poisonBuffText;
        [SerializeField] private TextMeshProUGUI slowDownBuffText;

        [SerializeField] private UpdateDamageButtonUI updateHealthDamageButtonUI;
        [SerializeField] private Image progressBarHealthImage;
        [SerializeField] private TextMeshProUGUI healthDamageUpdateText;
        
        [SerializeField] private UpdateDamageButtonUI updateArmorDamageButtonUI;
        [SerializeField] private Image progressBarArmorImage;
        [SerializeField] private TextMeshProUGUI armorDamageUpdateText;
        
        [SerializeField] private UpdateDamageButtonUI updateShieldDamageButtonUI;
        [SerializeField] private Image progressBarShieldImage;
        [SerializeField] private TextMeshProUGUI shieldDamageUpdateText;
        
        [SerializeField] private TextMeshProUGUI towerSellPriceText;

        private TileInformation _tileInformation;
        
        #endregion
        
        /// <summary>
        /// Initializes the tower information panel with the given tile information.
        /// </summary>
        /// <param name="tileInformation">The tile information associated with the tower.</param>
        public void InitializePanel(TileInformation tileInformation)
        {
            if (_tileInformation != null)
            {
                _tileInformation.GetInstalledTower().OnTowerDataChanged -= OnTowerDataChanged;
                Game.OnAmountOfCoinsChanged -= OnAmountOfCoinsChanged;
                _tileInformation = null;
            }
            
            _tileInformation = tileInformation;
            UpdateTileInformation(tileInformation.GetTileType());
            OnTowerDataChanged(tileInformation.GetInstalledTower());
            UpdatePriority();
            
            tileInformation.GetInstalledTower().OnTowerDataChanged += OnTowerDataChanged;
            Game.OnAmountOfCoinsChanged += OnAmountOfCoinsChanged;
        }

        /// <summary>
        ///  Event handler for the amount of coins changed event. Updates the UI. 
        /// </summary>
        /// <param name="obj"> The new amount of coins.</param>
        private void OnAmountOfCoinsChanged(int obj)
        {
            if (_tileInformation != null)
            {
                UpdateTileInformation(_tileInformation.GetTileType());
            }
        }
        
        /// <summary>
        ///  Fills the UI elements with the data from the specified tower.
        /// </summary>
        /// <param name="tower"> The tower to get the data from.</param>
        private void OnTowerDataChanged(AbstractTower tower)
        {
            towerNameText.text = tower.GetTowerName();
            towerLevelText.text = tower.GetTowerExperiences().GetTowerLevel().ToString();
            baseDamageText.text = tower.GetTowerApplyingDamage().GetBaseDamageForUI();
            healthMultiplierText.text = tower.GetTowerApplyingDamage().GetHealthDamageForUI();
            armorMultiplierText.text = tower.GetTowerApplyingDamage().GetArmorDamageForUI();
            shieldMultiplierText.text = tower.GetTowerApplyingDamage().GetShieldDamageForUI();
            rangeText.text = tower.GetTowerRangeForUI();
            fireRateText.text = tower.GetTowerFireRateForUI();
            currentPriority = tower.GetTowerAttackPriority();
            
            TowerBonuses bonuses = tower.GetTowerBonuses();
            bloodBuffText.text = $"{bonuses.GetNegativeBleeding()*100:F0}%";
            fireBuffText.text = $"{bonuses.GetNegativeFire()*100:F0}%";
            poisonBuffText.text = $"{bonuses.GetNegativePoison()*100:F0}%";
            slowDownBuffText.text = $"{bonuses.GetNegativeSlowdown()*100:F0}%";
            
            TowerExperiences experiences = tower.GetTowerExperiences();
            progressBarHealthImage.fillAmount = experiences.GetExperiencePercentForUI(DamageType.Health);
            progressBarArmorImage.fillAmount = experiences.GetExperiencePercentForUI(DamageType.Armor);
            progressBarShieldImage.fillAmount = experiences.GetExperiencePercentForUI(DamageType.Shield);
            healthDamageUpdateText.text = experiences.GetUpgradeCost(DamageType.Health).ToString();
            armorDamageUpdateText.text = experiences.GetUpgradeCost(DamageType.Armor).ToString();
            shieldDamageUpdateText.text = experiences.GetUpgradeCost(DamageType.Shield).ToString();
            
            updateHealthDamageButtonUI.SetActiveElements(experiences.IsEnoughMoneyToUpgradeDamage(DamageType.Health));
            updateArmorDamageButtonUI.SetActiveElements(experiences.IsEnoughMoneyToUpgradeDamage(DamageType.Armor));
            updateShieldDamageButtonUI.SetActiveElements(experiences.IsEnoughMoneyToUpgradeDamage(DamageType.Shield));

            towerSellPriceText.text = tower.GetTowerSellCost().ToString();
        }

        protected override void ApplyUpdatePriority()
        {
            OnPriorityButtonClicked?.Invoke(currentPriority);
        }

        /// <summary>
        /// Handler for the health button click event.
        /// </summary>
        public void OnHealthButtonClicked()
        {
            OnUpdateDamageButtonClicked?.Invoke(_tileInformation.GetInstalledTower(),DamageType.Health);
        }
        
        /// <summary>
        ///  Handler for the armor button click event.
        /// </summary>
        public void OnArmorButtonClicked()
        {
            OnUpdateDamageButtonClicked?.Invoke(_tileInformation.GetInstalledTower(),DamageType.Armor);
        }
        
        /// <summary>
        ///  Handler for the shield button click event.
        /// </summary>
        public void OnShieldButtonClicked()
        {
            OnUpdateDamageButtonClicked?.Invoke(_tileInformation.GetInstalledTower(),DamageType.Shield);
        }

        /// <summary>
        ///  Handler for the sell button click event.
        /// </summary>
        public void OnSellUIButtonClicked()
        {
            OnSellTowerButtonClicked?.Invoke(_tileInformation.GetInstalledTower());
        }

        /// <summary>
        ///  cleans up the event handlers.
        /// </summary>
        private void OnDisable()
        {
            Game.OnAmountOfCoinsChanged -= OnAmountOfCoinsChanged;
            _tileInformation.GetInstalledTower().OnTowerDataChanged -= OnTowerDataChanged;
            _tileInformation = null;
        }
    }
}