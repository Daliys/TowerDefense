using Tile;
using Towers;
using UnityEngine;

namespace UI
{
    /// <summary>
    ///  UI component representing the building panel containing tower information and priority settings.
    /// </summary>
    public class BuildingPanelUI : AbstractPanelUI
    {
        // Link to the tower manager
        [SerializeField] private TowerManager towerManager;
        
        public void InitializePanel(TileInformation tileInformation)
        {
            UpdateTileInformation(tileInformation.GetTileType());
        }
        
        public TowerManager GetTowerManager()
        {
            return towerManager;
        }
    }
}