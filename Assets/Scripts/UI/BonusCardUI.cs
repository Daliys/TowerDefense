using System;
using Items.BonusCards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// UI component representing a bonus card in the game.
    /// </summary>
    public class BonusCardUI : MonoBehaviour
    {
        /// <summary>
        /// Event triggered when this card is selected.
        /// </summary>
        public static event Action<BonusCardUI> OnCardSelected;

        /// <summary>
        /// Event triggered when a bonus card is chosen.
        /// </summary>
        public static event Action<AbstractBonusCard> OnCardChosen;

        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private GameObject selected;

        private AbstractBonusCard _card; // The abstract bonus card this UI represents.
        private bool _isSelected;


        private void Awake()
        {
            OnCardSelected += OnCardSelectedHandler;
        }

        /// <summary>
        /// Event handler when this card is selected.
        /// </summary>
        /// <param name="obj">The selected bonus card UI.</param>
        private void OnCardSelectedHandler(BonusCardUI obj)
        {
            if (obj == this)
            {
                _isSelected = true;
                selected.SetActive(true);
            }
            else
            {
                _isSelected = false;
                selected.SetActive(false);
            }
        }

        /// <summary>
        /// Initializes the bonus card UI with the provided bonus card data.
        /// </summary>
        /// <param name="card">The abstract bonus card to initialize the UI with.</param>
        public void Initialize(AbstractBonusCard card)
        {
            _card = card;
            image.sprite = card.uiSprite;
            text.text = card.uiText;
        }

        /// <summary>
        /// Event handler when the card button is clicked.
        /// Invokes appropriate events based on card selection state.
        /// </summary>
        public void OnButtonClicked()
        {
            if (_isSelected)
            {
                OnCardChosen?.Invoke(_card);
            }
            else
            {
                OnCardSelected?.Invoke(this);
            }
        }

        private void OnDestroy()
        {
            OnCardSelected -= OnCardSelectedHandler;
        }
    }
}