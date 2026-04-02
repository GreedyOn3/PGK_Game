using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class LevelUpOptionUi : MonoBehaviour
    {
        private StatUpgradeType _upgrade;
        private LevelUpUi _levelUpUi;

        public void Initialize(StatUpgradeInfo upgrade, LevelUpUi levelUpUi)
        {
            _levelUpUi = levelUpUi;
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
            _levelUpUi.PickStatUpgrade(_upgrade);
        }
    }
}
