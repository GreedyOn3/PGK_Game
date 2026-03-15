using UnityEngine;
using TMPro;

namespace UI
{
    public class LevelUpOptionUi : MonoBehaviour
    {
        public LevelUpUi levelUpUi;

        private Upgrade _upgrade;

        public void SetUpgrade(Upgrade upgrade)
        {
            _upgrade = upgrade;
            var name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            name.text = upgrade.title;
            description.text = upgrade.description;
        }

        public void Pick()
        {
            levelUpUi.PickUpgrade(_upgrade);
        }
    }
}
