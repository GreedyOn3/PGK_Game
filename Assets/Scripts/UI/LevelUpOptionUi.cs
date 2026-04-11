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

        private StatUpgradeId _upgrade;
        private LevelUpChoice _choice;
        private LevelUpUi _levelUpUi;

        public void Initialize(LevelUpChoice choice, LevelUpUi levelUpUi)
        {
            _levelUpUi = levelUpUi;
            //_upgrade = upgrade.Id;
            _choice = choice;

            BaseItemInfo item = choice.item;
            image.sprite = item.Image;
            nameText.text = item.Name;
            descriptionText.text = item.Description;
        }

        public void Pick()
        {
            _levelUpUi.PickStatUpgrade(_choice);
        }
    }
}
