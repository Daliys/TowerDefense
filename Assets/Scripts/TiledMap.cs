using System.Linq;
using Enemies;
using Items;
using Items.Tiles;
using Tile;
using Towers;
using UI;
using UnityEngine;

/// <summary>
///  Class for the tiled map. Loads the map from a MapItem and keeps track of the selected tile.
/// </summary>
public class TiledMap : MonoBehaviour
{
    [SerializeField] private TileSetItem tileSetItem;
    [SerializeField] private GameObject parent;

    private MapItem _mapItem;
    private Vector2 _spawnPosition;
    private Vector2 _portalPosition;
    private TileInformation _currentSelectedTile;
    private GameScene _gameScene;

    /// <summary>
    /// Initializes the TiledMap with the specified GameScene and MapItem.
    /// </summary>
    /// <param name="gameScene">The GameScene associated with this TiledMap.</param>
    /// <param name="mapItem">The MapItem containing map information.</param>
    public void Initialize(GameScene gameScene, MapItem mapItem)
    {
        _gameScene = gameScene;
        _mapItem = mapItem;
        LoadMap();

        GameScene.OnMouseClicked += OnMouseClicked;
        TowerInformationPanelUI.OnSellTowerButtonClicked += OnSellTowerButtonClicked;
        EnemyWaveGenerator.OnWaveEnded += OnWaveEndedHandler;
    }

    /// <summary>
    /// Handler for the event when a wave ends.
    /// </summary>
    /// <param name="wave">The wave number that ended.</param>
    private void OnWaveEndedHandler(int wave)
    {
        if (wave % 2 != 0 && wave != 0)
        {
            UnSelectTile();
        }
    }

    /// <summary>
    /// Handler for the event when the sell tower button is clicked.
    /// </summary>
    /// <param name="obj">The AbstractTower object associated with the button click.</param>
    private void OnSellTowerButtonClicked(AbstractTower obj)
    {
        UnSelectTile();
        _gameScene.GetGameUI().HideAllPanels();
    }

    /// <summary>
    /// Handler for the event when a tile is clicked.
    /// </summary>
    /// <param name="obj">The TileInformation associated with the clicked tile.</param>
    private void OnMouseClicked(TileInformation obj)
    {
        UnSelectTile();

        if (obj == null)
        {
            _gameScene.GetGameUI().HideAllPanels();
            return;
        }

        _currentSelectedTile = obj;
        _currentSelectedTile.SetSelected(true);

        CallOnTileClicked();
    }

    /// <summary>
    /// Unselects the currently selected tile.
    /// </summary>
    private void UnSelectTile()
    {
        if (_currentSelectedTile != null)
        {
            _currentSelectedTile.SetSelected(false);
            if (_currentSelectedTile.HasTower())
            {
                _currentSelectedTile.GetInstalledTower().SetVisibleRange(false);
            }
        }

        _currentSelectedTile = null;
    }

    /// <summary>
    /// Calls the appropriate action when a tile is clicked based on the selected tile's properties.
    /// </summary>
    public void CallOnTileClicked()
    {
        if (_currentSelectedTile == null) throw new System.Exception("No tile selected");

        if (_currentSelectedTile.HasTower())
        {
            _gameScene.GetGameUI().ShowTowerInformationPanelUI(_currentSelectedTile);
            _currentSelectedTile.GetInstalledTower().SetVisibleRange(true);
        }
        else
        {
            _gameScene.GetGameUI().ShowBuildingPanelUI(_currentSelectedTile);
        }
    }

    /// <summary>
    ///  Builds a tower on the currently selected tile.
    /// </summary>
    /// <param name="tower"> The tower to be built.</param>
    /// <exception cref="Exception"> Throws an exception if no tile is selected.</exception>
    public void BuildTower(AbstractTower tower)
    {
        if (_currentSelectedTile != null)
        {
            _currentSelectedTile.SetTower(tower);
            ApplyTileBonusToTower();
        }
        else
        {
            throw new System.Exception("No tile selected");
        }
    }

    /// <summary>
    /// Applies bonuses from the current selected tile to the tower installed on the tile.
    /// </summary>
    private void ApplyTileBonusToTower()
    {
        if (_currentSelectedTile == null) throw new System.Exception("No tile selected");
        if (_currentSelectedTile.HasTower() == false) throw new System.Exception("No tower installed");

        AbstractTower tower = _currentSelectedTile.GetInstalledTower();

        switch (_currentSelectedTile.GetTileType())
        {
            case TileType.BonusAttack:
                tower.GetTowerBonuses().AddTowerBonus(TowerBonusType.BaseDamage, _currentSelectedTile.GetBonusValue());
                break;
            case TileType.BonusRange:
                float bonusDamage = _currentSelectedTile.GetBonusValue() * tower.GetBaseTowerRange();
                tower.GetTowerBonuses().AddTowerBonus(TowerBonusType.Range, bonusDamage);
                break;
            case TileType.BonusXp:
                float bonusXp = _currentSelectedTile.GetBonusValue() * tower.GetExperienceMultiplier();
                tower.GetTowerBonuses().AddTowerBonus(TowerBonusType.Experience, bonusXp);
                break;
            case TileType.BonusFireRate:
                float bonusFireRate = _currentSelectedTile.GetBonusValue() * tower.GetBaseTowerFireRate();
                tower.GetTowerBonuses().AddTowerBonus(TowerBonusType.FireRate, bonusFireRate);
                break;
        }
    }

    /// <summary>
    /// Loads the map based on the map information.
    /// </summary>
    private void LoadMap()
    {
        string[] str = _mapItem.mapString.Split(',');
        int[] mapValues = str.Select(int.Parse).ToArray();

        int mapSizeX = _mapItem.mapSize.x;
        int mapSizeY = _mapItem.mapSize.y;

        for (int i = 0; i < mapSizeY; i++)
        {
            for (int j = 0; j < mapSizeX; j++)
            {
                int mapIndex = mapValues[i * mapSizeX + j];
                GameObject prefab = GetTilePrefab(mapIndex);
                GameObject tile = Instantiate(prefab, parent.transform);
                tile.transform.localPosition = new Vector3(j, mapSizeY - i, 0);

                if (tile.GetComponent<TileInformation>() != null)
                {
                    float bonusValue = tileSetItem.GetTileBonus(tile.GetComponent<TileInformation>().GetTileType());
                    tile.GetComponent<TileInformation>().SetBonusValue(bonusValue);
                }
            }
        }
    }

    /// <summary>
    /// Retrieves the tile prefab for the given tile index.
    /// </summary>
    /// <param name="tileIndex">The index of the tile.</param>
    /// <returns>The tile prefab.</returns>
    private GameObject GetTilePrefab(int tileIndex)
    {
        return tileSetItem.tileItems[tileIndex - 1].tilePrefab;
    }

    /// <summary>
    /// Retrieves the path for the enemy at the specified index.
    /// </summary>
    /// <param name="index">The index of the path.</param>
    /// <returns>The path for the enemy.</returns>
    public Vector2 GetPathForEnemy(int index)
    {
        if (!IsHavePathForEnemy(index))
        {
            throw new System.Exception("Index out of bounds");
        }

        var position = parent.transform.position;
        // +-1 because in items tile point start from 1 and in unity from 0
        //  _mapItem.mapSize.y - _mapItem.pathPoints[index] because in unity y start from bottom and in items from top
        return new Vector2(_mapItem.pathPoints[index].x - 1 + position.x, _mapItem.mapSize.y - _mapItem.pathPoints[index].y + 1 + position.y);
    }

    /// <summary>
    /// Gets the length of the path for enemy movement.
    /// </summary>
    public int GetPathLength()
    {
        return _mapItem.pathPoints.Length;
    }

    /// <summary>
    /// Checks if there is a path for the enemy at the specified index.
    /// </summary>
    /// <param name="index">The index to check.</param>
    public bool IsHavePathForEnemy(int index)
    {
        return index < _mapItem.pathPoints.Length;
    }

    /// <summary>
    /// Handles cleanup when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        GameScene.OnMouseClicked -= OnMouseClicked;
        TowerInformationPanelUI.OnSellTowerButtonClicked -= OnSellTowerButtonClicked;
        EnemyWaveGenerator.OnWaveEnded -= OnWaveEndedHandler;

    }

    /// <summary>
    /// Retrieves the selected tile's information.
    /// </summary>
    /// <returns>The TileInformation of the selected tile.</returns>
    public TileInformation GetSelectedTileInformation() => _currentSelectedTile;
}