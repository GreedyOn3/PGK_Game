using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class LevelCardUi : MonoBehaviour
    {
        private LevelInfo _levelInfo;
        private LevelSelectionUi _levelSelectionUi;

        public void Initialize(LevelInfo level, LevelSelectionUi levelSelectionUi)
        {
            _levelSelectionUi = levelSelectionUi;
            _levelInfo = level;
            var image = transform.Find("Image").GetComponent<Image>();
            var textContainer = transform.Find("Text Container");
            var name = textContainer.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var description = textContainer.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            image.sprite = level.Image;
            name.text = level.LevelName;
            description.text = level.Description;
        }

        public void Pick()
        {
            _levelSelectionUi.PickLevel(_levelInfo);
        }
    }
}
