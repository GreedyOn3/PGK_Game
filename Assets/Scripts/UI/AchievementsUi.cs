using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class AchievementsUi : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private GameObject achievementCardPrefab;
        [SerializeField] private Transform cardsContainer;

        private readonly List<AchievementCardUi> _achievementCards = new();

        private void Start()
        {
            var achievements = PersistentData.Instance.achievements;
            foreach (var achievement in achievements)
            {
                var card = Instantiate(achievementCardPrefab, cardsContainer);
                var cardUi = card.GetComponent<AchievementCardUi>();
                _achievementCards.Add(cardUi);
                Assert.IsNotNull(cardUi, "Achievement card should have a AchievementCardUi component.");
                cardUi.Initialize(achievement);
            }
        }

        private void OnEnable()
        {
            foreach (var card in _achievementCards)
                card.UpdateUi();
        }

        public void OnReturnButtonClicked()
        {
            mainMenu.ReturnToTitleScreen();
        }
    }
}
