using System;
using Enemies;
using Items.BonusCards;
using Tile;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    /// Manages the UI elements and actions in the game.
    /// </summary>
    public class GameUI : MonoBehaviour
    {
        public static event Action OnStartLoadingNewScene;

        [SerializeField] private GameObject buildingPanelGameObject;
        [SerializeField] private GameObject towerInformationPanelGameObject;
        [SerializeField] private StatsUI statsUI;
        [SerializeField] private GameObject bonusesUIGameObject;

        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;
        [SerializeField] private GameObject pausePanel;

        private TowerInformationPanelUI towerInformationPanelUI;
        private BuildingPanelUI buildingPanelUI;
        private BonusesUI bonusesUI;

        // cache the time scale before pausing the game
        private int _timeScaleBeforePause;

        private void Awake()
        {
            buildingPanelUI = buildingPanelGameObject.GetComponent<BuildingPanelUI>();
            towerInformationPanelUI = towerInformationPanelGameObject.GetComponent<TowerInformationPanelUI>();
            bonusesUI = bonusesUIGameObject.GetComponent<BonusesUI>();

            buildingPanelGameObject.SetActive(false);
            towerInformationPanelGameObject.SetActive(false);

            Game.OnGameEnded += ShowLosePanel;
            EnemyWaveGenerator.OnWin += ShowWinPanel;
        }

        /// <summary>
        /// Displays the building panel UI for a specified tile.
        /// </summary>
        /// <param name="tileType">The tile information for which the building panel is displayed.</param>
        public void ShowBuildingPanelUI(TileInformation tileType)
        {
            HideAllPanels();
            buildingPanelGameObject.SetActive(true);
            buildingPanelUI.InitializePanel(tileType);
        }

        /// <summary>
        ///  Displays the tower information panel UI for a specified tile.
        /// </summary>
        /// <param name="tileInformation">The tile information for which the tower information panel is displayed.</param>
        public void ShowTowerInformationPanelUI(TileInformation tileInformation)
        {
            HideAllPanels();
            towerInformationPanelGameObject.SetActive(true);
            towerInformationPanelUI.InitializePanel(tileInformation);
        }

        /// <summary>
        ///  Show panel with 3 bonuses cards. 
        /// </summary>
        public void ShowBonusesUI(AbstractBonusCard card1, AbstractBonusCard card2, AbstractBonusCard card3)
        {
            HideAllPanels();
            bonusesUI.gameObject.SetActive(true);
            bonusesUI.Initialize(card1, card2, card3);
        }

        /// <summary>
        ///  Displays the win panel.
        /// </summary>
        private void ShowWinPanel()
        {
            HideAllPanels();
            winPanel.SetActive(true);
        }

        /// <summary>
        ///  Displays the lose panel.
        /// </summary>
        private void ShowLosePanel()
        {
            HideAllPanels();
            losePanel.SetActive(true);
        }

        public void HideAllPanels()
        {
            buildingPanelGameObject.SetActive(false);
            towerInformationPanelGameObject.SetActive(false);
        }

        /// <summary>
        ///  Pauses the game and displays the pause panel.
        /// </summary>
        public void OnPauseButtonClicked()
        {
            pausePanel.SetActive(true);
            _timeScaleBeforePause = (int)Time.timeScale;
            Time.timeScale = 0;
        }

        /// <summary>
        ///  Resumes the game.
        /// </summary>
        public void OnButtonContinueClicked()
        {
            pausePanel.SetActive(false);
            Time.timeScale = _timeScaleBeforePause;
        }

        /// <summary>
        ///  Loads the main menu scene.
        /// </summary>
        public void OnButtonMainMenuClicked()
        {
            Time.timeScale = 1;
            OnStartLoadingNewScene?.Invoke();
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Restarts the current level.
        /// </summary>
        public void OnButtonRestartClicked()
        {
            Time.timeScale = 1;
            OnStartLoadingNewScene?.Invoke();
            SceneManager.LoadScene(1);
        }

        /// <summary>
        /// Cleans up event subscriptions when the object is destroyed.
        /// </summary>
        public void OnDestroy()
        {
            Game.OnGameEnded -= ShowLosePanel;
            EnemyWaveGenerator.OnWin -= ShowWinPanel;
        }
    }
}