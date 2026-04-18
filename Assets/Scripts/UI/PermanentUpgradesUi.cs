using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class PermanentUpgradesUi : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private GameObject permanentUpgradeCardPrefab;

        private void Start()
        {
            var upgrades = PersistentData.Instance.permanentUpgrades;
            foreach (var upgrade in upgrades)
            {
                var card = Instantiate(permanentUpgradeCardPrefab, transform);
                var cardUi = card.GetComponent<PermanentUpgradeCardUi>();
                Assert.IsNotNull(cardUi, "Permanent upgrade card should have a PermanentUpgradeCardUi component.");
                cardUi.Initialize(upgrade);
            }
        }

        public void Return()
        {
            mainMenu.ReturnToTitleScreen();
        }
    }
}
