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
            UpdateUi();
        }

        public void UpdateUi()
        {
            image.sprite = _upgrade.Image;
            nameText.text = _upgrade.UpgradeName;
            descriptionText.text = _upgrade.Description;
            buyButtonText.text = _upgrade.bought ? "Bought" : "Buy";

            buyButton.interactable = !_upgrade.bought;
            enabledToggle.isOn = _upgrade.enabled;
            enabledToggle.interactable = _upgrade.bought;
        }

        public void Buy()
        {
            _upgrade.bought = true;
            _upgrade.enabled = true;
            UpdateUi();
            SaveManager.instance.saveData.SavePermanentUpgrade(_upgrade);
        }

        public void OnToggleEnabled()
        {
            _upgrade.enabled = enabledToggle.isOn;
            UpdateUi();
            SaveManager.instance.saveData.SavePermanentUpgrade(_upgrade);
        }
    }
}
