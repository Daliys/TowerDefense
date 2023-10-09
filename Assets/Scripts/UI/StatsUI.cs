using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Manages the display of game statistics in the UI (health, wave, money, time) and the time control buttons.
    /// </summary>
    public class StatsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI moneyText;

        [SerializeField] private Image pauseImage;
        [SerializeField] private Image normalSpeedImage;
        [SerializeField] private Image fastSpeedImage;

        // Alpha value for the pause button
        [SerializeField] private float pauseAlpha;
        
        // Colors for the time buttons when active and inactive
        private Color timeActiveColor;
        private Color timeInactiveColor;

        private void Awake()
        {
            
            timeActiveColor = new Color(1, 1, 1, 1);
            timeInactiveColor = new Color(1, 1, 1, pauseAlpha);
            
            Game.OnAmountOfHealthChanged += SetHealthText;
            EnemyWaveGenerator.OnWaveStarted += SetWaveText;
            Game.OnAmountOfCoinsChanged += SetMoneyText;
            
            OnSpeedButtonClicked(1);
        }
        
        private void SetHealthText(int value)
        {
            healthText.text = value.ToString();
        }
        
        private void SetWaveText(int value)
        {
            waveText.text = (value +1).ToString();
        }
        
        private void SetMoneyText(int value)
        {
            moneyText.text = value.ToString();
        }
        
        public void OnSpeedButtonClicked(int speed)
        {
            Time.timeScale = speed;
            
            pauseImage.color = speed == 0 ? timeActiveColor : timeInactiveColor;
            normalSpeedImage.color = speed == 1 ? timeActiveColor : timeInactiveColor;
            fastSpeedImage.color = speed == 2 ? timeActiveColor : timeInactiveColor;
        }
        
        private void OnDestroy()
        {
            Game.OnAmountOfHealthChanged -= SetHealthText;
            EnemyWaveGenerator.OnWaveStarted -= SetWaveText;
            Game.OnAmountOfCoinsChanged -= SetMoneyText;
        }
    }
}