using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CardElements))]
    public class LevelCardUi : MonoBehaviour
    {
        private LevelInfo _levelInfo;
        private LevelSelectionUi _levelSelectionUi;

        public void Initialize(LevelInfo level, LevelSelectionUi levelSelectionUi)
        {
            _levelSelectionUi = levelSelectionUi;
            _levelInfo = level;
            var cardElements = GetComponent<CardElements>();
            cardElements.Image.sprite = level.Image;
            cardElements.MainText.text = level.LevelName;
            cardElements.SubText.text = level.Description;
        }

        public void Pick()
        {
            _levelSelectionUi.PickLevel(_levelInfo);
        }
    }
}
