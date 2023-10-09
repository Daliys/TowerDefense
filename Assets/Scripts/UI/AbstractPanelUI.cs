using Items.Priority;
using Items.Tiles;
using Tile;
using TMPro;
using Towers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Base abstract class for UI panels related to tile information and priority settings.
    /// </summary>
    public abstract class AbstractPanelUI : MonoBehaviour
    {
        [SerializeField] protected TileSetItemForUI tileSetItemForUI;
        [SerializeField] protected Image tileInformationImage;
        [SerializeField] protected TextMeshProUGUI tileInformationText;
        [SerializeField] protected Image priorityTargetImage;
        [SerializeField] protected TextMeshProUGUI priorityTargetText;
        [SerializeField] protected PrioritiesItem prioritiesItem;

        protected TowerPriority currentPriority;

        
        /// <summary>
        /// Updates the tile information based on the specified tile type.
        /// </summary>
        /// <param name="tileType">The type of the tile to update the information for.</param>
        protected void UpdateTileInformation(TileType tileType)
        {
            tileInformationImage.sprite = tileSetItemForUI.GetTileItem(tileType).tileSprite;
            tileInformationText.text = tileSetItemForUI.GetTileItem(tileType).description;
        }

        /// <summary>
        /// Updates the UI elements related to tower priority.
        /// </summary>
        protected void UpdatePriority()
        {
            PriorityItem priorityItem = prioritiesItem.GetPriorityItem(currentPriority);
            priorityTargetImage.sprite = priorityItem.prioritySprite;
            priorityTargetText.text = priorityItem.priorityName;
            ApplyUpdatePriority();
        }

        /// <summary>
        /// Event handler for the next priority button click.
        /// Advances to the next tower priority and updates the UI.
        /// </summary>
        public void OnPriorityTargetNextButtonClicked()
        {
            currentPriority = prioritiesItem.GetNextPriorityItem(currentPriority).priority;
            UpdatePriority();
        }

        /// <summary>
        /// Event handler for the previous priority button click.
        /// Moves to the previous tower priority and updates the UI.
        /// </summary>
        public void OnPriorityTargetPreviousButtonClicked()
        {
            currentPriority = prioritiesItem.GetPreviousPriority(currentPriority).priority;
            UpdatePriority();
        }

        /// <summary>
        /// Gets the current tower priority.
        /// </summary>
        public TowerPriority GetCurrentTowerPriority() => currentPriority;

        /// <summary>
        /// Method to apply updates related to tower priority.
        /// Derived classes can override this to add custom behavior upon priority update.
        /// </summary>
        protected virtual void ApplyUpdatePriority()
        {
        }
    }
}