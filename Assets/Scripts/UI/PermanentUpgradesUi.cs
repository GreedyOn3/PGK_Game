using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class PermanentUpgradesUi : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private GameObject permanentUpgradeCardPrefab;
        [SerializeField] private Transform cardsContainer;
        [SerializeField] private TextMeshProUGUI resourceAmount;

        private readonly List<PermanentUpgradeCardUi> _upgradeCards = new();

        private void OnEnable()
        {
            Refresh();
        }

        private void Start()
        {
            var upgrades = PersistentData.Instance.permanentUpgrades;
            foreach (PermanentUpgradeInfo upgrade in upgrades)
            {
                var card = Instantiate(permanentUpgradeCardPrefab, cardsContainer);
                var cardUi = card.GetComponent<PermanentUpgradeCardUi>();
                _upgradeCards.Add(cardUi);
                Assert.IsNotNull(cardUi, "Permanent upgrade card should have a PermanentUpgradeCardUi component.");
                cardUi.Initialize(this, upgrade);
            }
        }

        public void Refresh()
        {
            resourceAmount.text = "SR: " + SaveManager.instance.saveData.GetTotalSpecialResources();

            foreach (PermanentUpgradeCardUi upgradeCard in _upgradeCards)
                upgradeCard.UpdateUi();
        }

        public void OnReturnButtonClicked()
        {
            mainMenu.ReturnToTitleScreen();
        }

        public void OnDeleteSavegameButtonClicked()
        {
            mainMenu.GoToDeleteSaveGameScreen();
        }
    }
}
