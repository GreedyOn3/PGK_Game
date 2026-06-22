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
        private PermanentUpgradesUi _upgradesUI;

        public void Initialize(PermanentUpgradesUi upgradesUI, PermanentUpgradeInfo upgrade)
        {
            _upgradesUI = upgradesUI;
            _upgrade = upgrade;
            UpdateUi();
        }

        public void UpdateUi()
        {
            image.sprite = _upgrade.Image;
            nameText.text = _upgrade.UpgradeName;
            descriptionText.text = _upgrade.Description;
            //buyButtonText.text = _upgrade.bought ? "Bought" : "Buy";
            buyButtonText.text = _upgrade.bought ? "Bought" : $"Buy ({_upgrade.Cost})";

            int totalResources = SaveManager.instance.saveData.GetTotalSpecialResources();
            buyButton.interactable = !_upgrade.bought && totalResources >= _upgrade.Cost;
            //buyButton.interactable = !_upgrade.bought;
            enabledToggle.isOn = _upgrade.enabled;
            enabledToggle.interactable = _upgrade.bought;
        }

        public void Buy()
        {
            /*_upgrade.bought = true;
            _upgrade.enabled = true;
            UpdateUi();
            SaveManager.instance.saveData.SavePermanentUpgrade(_upgrade);*/
            int totalResources = SaveManager.instance.saveData.GetTotalSpecialResources();

            if (!_upgrade.bought && totalResources >= _upgrade.Cost)
            {
                SaveManager.instance.saveData.DecreaseSpecialResources(_upgrade.Cost);

                _upgrade.bought = true;
                _upgrade.enabled = true;

                SaveManager.instance.saveData.SavePermanentUpgrade(_upgrade);
                SaveManager.instance.SaveGame();

                if (_upgradesUI != null)
                    _upgradesUI.Refresh();
            }
        }

        public void OnToggleEnabled()
        {
            _upgrade.enabled = enabledToggle.isOn;
            UpdateUi();
            SaveManager.instance.saveData.SavePermanentUpgrade(_upgrade);
        }
    }
}
