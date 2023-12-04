using Items;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    /// <summary>
    ///  Manages the UI elements and actions in the main menu.
    /// </summary>
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private GameInformationSO gameInformation;
        [SerializeField] private LevelItem levelItem1;
        [SerializeField] private LevelItem levelItem2;
        [SerializeField] private LevelItem levelItem3;

        private void Start()
        {
            AudioManager.instance?.PlayMenuMusic();
        }
       
        /// <summary>
        ///  Loads the game scene with the selected level.
        /// </summary>
        /// <param name="levelIndex"> The level index to load.</param>
        public void OnLevelButtonClicked(int levelIndex)
        {
            switch (levelIndex)
            {
                case 1:
                    gameInformation.currentLevel = levelItem1;
                    break;
                case 2:
                    gameInformation.currentLevel = levelItem2;
                    break;
                case 3:
                    gameInformation.currentLevel = levelItem3;
                    break;
            }

            SceneManager.LoadScene(1);
        }

        /// <summary>
        ///  Exits the game.
        /// </summary>
        public void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}
