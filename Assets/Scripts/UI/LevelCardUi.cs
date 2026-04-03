using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class LevelCardUi : MonoBehaviour
    {
        private LevelId _levelId;
        private LevelSelectionUi _levelSelectionUi;

        public void Initialize(LevelInfo level, LevelSelectionUi levelSelectionUi)
        {
            _levelSelectionUi = levelSelectionUi;
            _levelId = level.Id;
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
            _levelSelectionUi.PickLevel(_levelId);
        }
    }
}
