using System.Collections.Generic;
using Enemies;
using Items.Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Manages the user interface for displaying enemy information, including health, armor, shield, and negative effects.
    /// Provides methods to update and visualize this information on the UI.
    /// </summary>
    public class EnemyUI : MonoBehaviour
    {
        // Setting via Unity Editor Inspector all UI elements that need to be updated
        
        [SerializeField] private NegativeEffectsUIItem negativeEffectsUIItem;

        [SerializeField] private Image healthBar;
        [SerializeField] private Image armorBar;
        [SerializeField] private Image shieldBar;

        [SerializeField] private Image negativeEffect1Image;
        [SerializeField] private TextMeshProUGUI negativeEffect1Text;

        [SerializeField] private Image negativeEffect2Image;
        [SerializeField] private TextMeshProUGUI negativeEffect2Text;

        [SerializeField] private Image negativeEffect3Image;
        [SerializeField] private TextMeshProUGUI negativeEffect3Text;

        [SerializeField] private Image negativeEffect4Image;
        [SerializeField] private TextMeshProUGUI negativeEffect5Text;


        private void Awake()
        {
            HideAll();
            healthBar.fillAmount = 1;
        }

        /// <summary>
        /// Sets the health, armor, and shield values on the UI.
        /// </summary>
        /// <param name="health">The health value (normalized between 0 and 1).</param>
        /// <param name="armor">The armor value (normalized between 0 and 1).</param>
        /// <param name="shield">The shield value (normalized between 0 and 1).</param>
        public void SetHealthBar(float health, float armor, float shield)
        {
            if (health is < 0 or > 1 || armor is < 0 or > 1 || shield is < 0 or > 1)
            {
                throw new System.Exception("Health, armor and shield must be between 0 and 1");
            }

            healthBar.fillAmount = health;
            armorBar.fillAmount = armor;
            shieldBar.fillAmount = shield;

        }

        /// <summary>
        /// Sets the negative effects on the UI, including icons and values where applicable.
        /// </summary>
        /// <param name="effects">Dictionary containing negative effects and their corresponding values.</param>
        public void SetNegativeEffects(Dictionary<EnemyNegativeEffectType, float> effects)
        {
            HideAll();
            int i = 0;
            foreach (var effect in effects)
            {
                i++;
                switch (i)
                {
                    case 1:
                        FillNegativeEffectImage(negativeEffect1Image, negativeEffect1Text, effect.Key, effect.Value);
                        break;
                    case 2:
                        FillNegativeEffectImage(negativeEffect2Image, negativeEffect2Text, effect.Key, effect.Value);
                        break;
                    case 3:
                        FillNegativeEffectImage(negativeEffect3Image, negativeEffect3Text, effect.Key, effect.Value);
                        break;
                    case 4:
                        FillNegativeEffectImage(negativeEffect4Image, negativeEffect5Text, effect.Key, effect.Value);
                        break;
                }
            }
        }

        /// <summary>
        /// Fills the UI image and text for a specific negative effect type with the provided value.
        /// </summary>
        /// <param name="image">The UI image to display the negative effect icon.</param>
        /// <param name="text">The UI text to display the negative effect value.</param>
        /// <param name="type">The type of negative effect to display.</param>
        /// <param name="value">The value of the negative effect to display.</param>
        private void FillNegativeEffectImage(Image image, TextMeshProUGUI text, EnemyNegativeEffectType type,
            float value)
        {
            if (value <= 0) return;

            image.gameObject.SetActive(true);
            image.sprite = negativeEffectsUIItem.GetIcon(type);

            if (type == EnemyNegativeEffectType.Slowdown)
            {
                value *= 100;
            }

            text.text = $"{value:F0}";
        }
        
        /// <summary>
        ///  Hides all negative effect UI elements.
        /// </summary>
        private void HideAll()
        {
            negativeEffect1Image.gameObject.SetActive(false);
            negativeEffect2Image.gameObject.SetActive(false);
            negativeEffect3Image.gameObject.SetActive(false);
            negativeEffect4Image.gameObject.SetActive(false);
        }
    }
}