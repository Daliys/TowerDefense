using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Represents a UI element for updating damage on a tower.
    /// </summary>
    public class UpdateDamageButtonUI : MonoBehaviour
    {
        [SerializeField] private Color activeButtonTextColor;
        [SerializeField] private Color inactiveButtonTextColor;

        [SerializeField] private Color activePriceTextColor;
        [SerializeField] private Color inactivePriceTextColor;

        [SerializeField] private GameObject buttonText;
        [SerializeField] private GameObject priceText;
        [SerializeField] private Button button;

        private TextMeshProUGUI _buttonText;
        private TextMeshProUGUI _priceText;

        private void Awake()
        {
            _buttonText = buttonText.GetComponent<TextMeshProUGUI>();
            _priceText = priceText.GetComponent<TextMeshProUGUI>();

            SetActiveElements(false);
        }

        /// <summary>
        /// Sets the active state and visual appearance of the button and price text.
        /// </summary>
        /// <param name="isActive">A boolean indicating whether the button should be active.</param>
        public void SetActiveElements(bool isActive)
        {
            button.enabled = isActive;

            _buttonText.color = isActive ? activeButtonTextColor : inactiveButtonTextColor;
            _priceText.color = isActive ? activePriceTextColor : inactivePriceTextColor;

        }

    }
}
