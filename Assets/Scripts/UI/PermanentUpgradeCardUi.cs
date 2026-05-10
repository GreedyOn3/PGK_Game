using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class PermanentUpgradeCardUi : MonoBehaviour
    {
        [SerializeField] private Button buyButton;
        [SerializeField] private Toggle enabledToggle;
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI buyButtonText;

        private PermanentUpgradeInfo _upgrade;

        public void Initialize(PermanentUpgradeInfo upgrade)
        {
            _upgrade = upgrade;
            buyButton.interactable = !_upgrade.bought;
            enabledToggle.isOn = _upgrade.enabled;
            enabledToggle.interactable = _upgrade.bought;
            image.sprite = upgrade.Image;
            nameText.text = upgrade.UpgradeName;
            descriptionText.text = upgrade.Description;
            buyButtonText.text = _upgrade.bought ? "Bought" : "Buy";
        }

        public void Buy()
        {
            _upgrade.bought = true;
            _upgrade.enabled = true;
            buyButton.interactable = false;
            enabledToggle.interactable = true;
            enabledToggle.isOn = _upgrade.enabled;
            buyButtonText.text = "Bought";
            SaveManager.instance.saveData.SavePermanentUpgrade(_upgrade);
        }

        public void OnToggleEnabled()
        {
            _upgrade.enabled = enabledToggle.isOn;
            SaveManager.instance.saveData.SavePermanentUpgrade(_upgrade);
        }
    }
}
