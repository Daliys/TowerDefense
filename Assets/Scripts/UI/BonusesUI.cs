using Items.BonusCards;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// UI component representing the bonuses panel containing bonus cards.
    /// </summary>
    public class BonusesUI : MonoBehaviour
    {
        // Link to the bonus card UI
        [SerializeField] private GameObject bonusPanel;
        [SerializeField] private BonusCardUI bonus1;
        [SerializeField] private BonusCardUI bonus2;
        [SerializeField] private BonusCardUI bonus3;

        private void Start()
        {
            BonusCardUI.OnCardChosen += OnCardChosenHandler;
        }

        public void Initialize(AbstractBonusCard card1, AbstractBonusCard card2, AbstractBonusCard card3)
        {
            bonus1.Initialize(card1);
            bonus2.Initialize(card2);
            bonus3.Initialize(card3);
        }

        /// <summary>
        /// Event handler when a bonus card is chosen.
        /// </summary>
        /// <param name="obj">The chosen bonus card.</param>
        private void OnCardChosenHandler(AbstractBonusCard obj)
        {
            bonusPanel.SetActive(false);
        }

        private void OnDestroy()
        {
            BonusCardUI.OnCardChosen -= OnCardChosenHandler;
        }
    }
}