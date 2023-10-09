using System;
using TMPro;
using Towers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Represents a UI button for a specific type of tower.
    /// </summary>
    public class UITowerButton : MonoBehaviour
    {
        public static event Action<TowerType, TowerPriority> OnTowerButtonClicked;

        [SerializeField] private GameObject activeGameObject;
        [SerializeField] private TextMeshProUGUI towerPriceText;

        [SerializeField] private Color priceColorAvailable;
        [SerializeField] private Color priceColorNotAvailable;

        [SerializeField] private TowerType towerType;
        [SerializeField] private BuildingPanelUI buildingPanelUI;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            Game.OnAmountOfCoinsChanged += OnAmountOfCoinsChanged;
        }

        /// <summary>
        /// Event handler for changes in the amount of coins.
        /// </summary>
        /// <param name="moneyCount">The current amount of money/coins.</param>
        private void OnAmountOfCoinsChanged(int moneyCount)
        {
            int towerCost = buildingPanelUI.GetTowerManager().GetTowerCost(towerType);
            towerPriceText.text = towerCost.ToString();

            bool isActive = towerCost <= moneyCount;
            SetActive(isActive);
        }

        /// <summary>
        ///  Event handler for the tower button click event. Triggers the OnTowerButtonClicked event.
        /// </summary>
        public void OnButtonClicked()
        {
            OnTowerButtonClicked?.Invoke(towerType, buildingPanelUI.GetCurrentTowerPriority());
        }

        /// <summary>
        ///  Sets the active state of the tower button.
        /// </summary>
        public void SetActive(bool isActive)
        {
            activeGameObject.SetActive(isActive);
            _button.enabled = isActive;
            towerPriceText.color = isActive ? priceColorAvailable : priceColorNotAvailable;
        }

        /// <summary>
        ///  cleans up the event subscriptions.
        /// </summary>
        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
            Game.OnAmountOfCoinsChanged -= OnAmountOfCoinsChanged;
        }

        /// <summary>
        ///  Cleans up the event subscriptions.
        /// </summary>
        private void OnDisable()
        {
            Game.OnAmountOfCoinsChanged -= OnAmountOfCoinsChanged;
        }
    }
}