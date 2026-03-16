using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class LevelUpOptionUi : MonoBehaviour
    {
        public LevelUpUi levelUpUi;

        private Upgrade _upgrade;

        public void SetUpgrade(Upgrade upgrade)
        {
            _upgrade = upgrade;
            var image = transform.Find("Image").GetComponent<Image>();
            var name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var description = transform.Find("Description").GetComponent<TextMeshProUGUI>();
            image.sprite = upgrade.image;
            name.text = upgrade.title;
            description.text = upgrade.description;
        }

        public void Pick()
        {
            levelUpUi.PickUpgrade(_upgrade);
        }
    }
}
