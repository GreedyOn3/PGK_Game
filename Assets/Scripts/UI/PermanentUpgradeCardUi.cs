using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class PermanentUpgradeCardUi : MonoBehaviour
    {
        private PermanentUpgradeInfo _upgrade;
        private Button _buyButton;

        public void Initialize(PermanentUpgradeInfo upgrade)
        {
            _upgrade = upgrade;
            var image = transform.Find("Image").GetComponent<Image>();
            var textContainer = transform.Find("Text Container");
            var name = textContainer.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var description = textContainer.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            _buyButton = transform.Find("Button").GetComponent<Button>();
            _buyButton.interactable = !_upgrade.bought;
            image.sprite = upgrade.Image;
            name.text = upgrade.UpgradeName;
            description.text = upgrade.Description;
        }

        public void Buy()
        {
            _upgrade.bought = true;
            _buyButton.interactable = !_upgrade.bought;
            SaveManager.instance.saveData.SavePermanentUpgrade(_upgrade);
        }
    }
}
