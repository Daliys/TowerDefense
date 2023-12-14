using System;
using Enemies;
using Items;
using Tile;
using Towers;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///  Class for the game scene. Keeps track of all important components.
/// </summary>
public class GameScene : MonoBehaviour
{
    [SerializeField] private TiledMap tiledMap;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private TowerManager towerManager;
    [SerializeField] private EnemyWaveGenerator enemyWaveGenerator;
    [SerializeField] private BonusesBetweenWaves bonusesBetweenWaves;
    [SerializeField] private GameInformationSO gameInformation;
    
    private Game game;
    private Camera mainCamera;
    
    /// <summary>
    /// Event triggered when a tile is clicked.
    /// </summary>
    public static event Action<TileInformation> OnMouseClicked;
    
    private void Awake()
    {
        game = new Game();
        game.AddCoins(gameInformation.currentLevel.startingCoins);
        mainCamera = Camera.main;
        
        tiledMap.Initialize(this,gameInformation.currentLevel.mapItem);
        towerManager.Initialize(this);
        bonusesBetweenWaves.Initialize(this);
        enemyWaveGenerator.StartGenerate(gameInformation.currentLevel.wavesItem, tiledMap);
        
    }

    private void Start()
    {
        AudioManager.instance?.PlayGameMusic();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if(EventSystem.current.IsPointerOverGameObject()) return;
            
            if (hit.collider != null)
            {
                if( hit.collider.CompareTag("Tile"))
                {
                    TileInformation tileInformation = hit.collider.GetComponent<TileInformation>();
                    OnMouseClicked?.Invoke(tileInformation);
                }
                else
                {
                      OnMouseClicked?.Invoke(null);
                }
            }
            else
            {
                OnMouseClicked?.Invoke(null);
            }
      
        }
    }

    /// <summary>
    /// Gets the TiledMap component.
    /// </summary>
    public TiledMap GetTileMap() => tiledMap;

    /// <summary>
    /// Gets the GameUI component.
    /// </summary>
    public GameUI GetGameUI() => gameUI;

    /// <summary>
    /// Gets the current level information.
    /// </summary>
    public LevelItem GetLevelItem() => gameInformation.currentLevel;

    /// <summary>
    /// Gets the Game instance.
    /// </summary>
    public Game GetGame() => game;

    /// <summary>
    /// Gets the TowerManager component.
    /// </summary>
    public TowerManager GetTowerManager() => towerManager;

    /// <summary>
    /// Gets the EnemyWaveGenerator component.
    /// </summary>
    public EnemyWaveGenerator GetEnemyWaveGenerator() => enemyWaveGenerator;
}
