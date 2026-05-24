using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class AchievementCardUi : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private GameObject grayedOutEffect;

        private AchievementInfo _achievement;

        public void Initialize(AchievementInfo achievement)
        {
            _achievement = achievement;
            UpdateUi();
        }

        public void UpdateUi()
        {
            image.sprite = _achievement.Image;
            nameText.text = _achievement.AchievementName;
            descriptionText.text = _achievement.Description;

            grayedOutEffect.SetActive(!_achievement.unlocked);
        }
    }
}
