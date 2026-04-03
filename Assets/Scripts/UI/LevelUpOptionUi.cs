using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class LevelUpOptionUi : MonoBehaviour
    {
        private StatUpgradeId _upgrade;
        private LevelUpUi _levelUpUi;

        public void Initialize(StatUpgradeInfo upgrade, LevelUpUi levelUpUi)
        {
            _levelUpUi = levelUpUi;
            _upgrade = upgrade.Id;
            var image = transform.Find("Image").GetComponent<Image>();
            var name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            image.sprite = upgrade.Image;
            name.text = upgrade.Title;
            description.text = upgrade.Description;
        }

        public void Pick()
        {
            _levelUpUi.PickStatUpgrade(_upgrade);
        }
    }
}
