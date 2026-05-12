using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class LevelUpOptionUi : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI rarityText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        private LevelUpChoice _choice;
        private LevelUpUi _levelUpUi;

        public void Initialize(LevelUpChoice choice, LevelUpUi levelUpUi)
        {
            _levelUpUi = levelUpUi;
            _choice = choice;

            if (choice.Rarity != null)
            {
                rarityText.text = choice.Rarity.Name;
                rarityText.color = choice.Rarity.color;
            }
            else
            {
                rarityText.text = "";
            }

            BaseItemInfo item = choice.Item;
            image.sprite = item.Image;
            nameText.text = item.Name;

            if (choice.Type == ChoiceType.UpgradePassive || choice.Type == ChoiceType.UpgradeWeapon)
            {
                foreach (StatInfo stat in choice.Stats)
                    descriptionText.text = $"{Stat.GetDisplayName(stat.Type)} +{stat.Value}{((stat.IsPercentage) ? "%" : "")}\n";
                descriptionText.text = descriptionText.text.TrimEnd('\n');
            }
            else
                descriptionText.text = item.Description;
        }

        public void Pick()
        {
            _levelUpUi.PickUpgrade(_choice);
        }
    }
}
