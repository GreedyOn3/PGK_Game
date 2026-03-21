using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class LevelUpOptionUi : MonoBehaviour
    {
        public LevelUpUi levelUpUi;

        private StatUpgradeType _upgrade;

        public void SetStatUpgrade(StatUpgradeInfo upgrade)
        {
            _upgrade = upgrade.type;
            var image = transform.Find("Image").GetComponent<Image>();
            var name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            image.sprite = upgrade.image;
            name.text = upgrade.title;
            description.text = upgrade.description;
        }

        public void Pick()
        {
            levelUpUi.PickStatUpgrade(_upgrade);
        }
    }
}
