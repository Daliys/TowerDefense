using Towers;
using UnityEngine;

namespace Tile
{
    /// <summary>
    /// Class representing information and behavior for a tile on the game grid.
    /// </summary>
    public class TileInformation : MonoBehaviour
    {
        [SerializeField] private TileType tileType; // The type of this tile (e.g., grass, path, etc.)
        [SerializeField] private GameObject selectedTile; // Visual indication of tile selection
        [SerializeField] private GameObject towerPlaceGameObject; // Location where a tower can be placed

        private float _bonusValue; // Bonus value associated with this tile
        private AbstractTower _tower; // The tower installed on this tile

        /// <summary>
        /// Sets the bonus value for this tile.
        /// </summary>
        /// <param name="value">The bonus value to set.</param>
        public void SetBonusValue(float value)
        {
            _bonusValue = value;
        }

        /// <summary>
        /// Sets the selection state for this tile, activating or deactivating the selected visual indicator.
        /// </summary>
        /// <param name="isSelected">Whether the tile is selected.</param>
        public void SetSelected(bool isSelected)
        {
            selectedTile.SetActive(isSelected);
        }

        /// <summary>
        /// Sets a tower on this tile.
        /// </summary>
        /// <param name="tower">The tower to install.</param>
        public void SetTower(AbstractTower tower)
        {
            _tower = tower;
            tower.transform.position = towerPlaceGameObject.transform.position;
            tower.transform.SetParent(towerPlaceGameObject.transform);
        }

        /// <summary>
        /// Checks if a tower is installed on this tile.
        /// </summary>
        /// <returns>True if a tower is installed, false otherwise.</returns>
        public bool HasTower()
        {
            return _tower;
        }

        /// <summary>
        /// Gets the type of this tile.
        /// </summary>
        /// <returns>The tile type.</returns>
        public TileType GetTileType() => tileType;

        /// <summary>
        /// Gets the bonus value associated with this tile.
        /// </summary>
        /// <returns>The bonus value.</returns>
        public float GetBonusValue() => _bonusValue;

        /// <summary>
        /// Gets the installed tower on this tile.
        /// </summary>
        /// <returns>The installed tower.</returns>
        public AbstractTower GetInstalledTower() => _tower;
    }
}