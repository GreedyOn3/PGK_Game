using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class LevelUpOptionUi : MonoBehaviour
    {
        public Image image;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;

        private LevelUpChoice _choice;
        private LevelUpUi _levelUpUi;

        public void Initialize(LevelUpChoice choice, LevelUpUi levelUpUi)
        {
            _levelUpUi = levelUpUi;
            _choice = choice;

            BaseItemInfo item = choice.Item;
            image.sprite = item.Image;
            nameText.text = item.Name;

            if (choice.Type == ChoiceType.UpgradePassive || choice.Type == ChoiceType.UpgradeWeapon)
            {
                GetComponent<Image>().color = choice.Rarity.color;

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
