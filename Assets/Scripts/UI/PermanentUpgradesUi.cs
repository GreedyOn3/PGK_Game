using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class PermanentUpgradesUi : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private GameObject permanentUpgradeCardPrefab;

        private readonly List<PermanentUpgradeCardUi> _upgradeCards = new();

        private void Start()
        {
            var upgrades = PersistentData.Instance.permanentUpgrades;
            foreach (var upgrade in upgrades)
            {
                var card = Instantiate(permanentUpgradeCardPrefab, transform);
                var cardUi = card.GetComponent<PermanentUpgradeCardUi>();
                _upgradeCards.Add(cardUi);
                Assert.IsNotNull(cardUi, "Permanent upgrade card should have a PermanentUpgradeCardUi component.");
                cardUi.Initialize(upgrade);
            }
        }

        public void OnReturnButtonClicked()
        {
            mainMenu.ReturnToTitleScreen();
        }

        public void OnDeleteSavegameButtonClicked()
        {
            SaveManager.instance.DeleteSavegame();
            PersistentData.Instance.ResetPermanentUpgrades();
            foreach (var upgradeCard in _upgradeCards)
                upgradeCard.UpdateUi();
        }
    }
}
