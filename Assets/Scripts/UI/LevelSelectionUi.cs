using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectionUi : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private LevelInfo[] levelInfos;
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private Transform cardsContainer;

        private void Start()
        {
            foreach (var level in levelInfos)
            {
                var card = Instantiate(cardPrefab, cardsContainer);
                var cardUi = card.AddComponent<LevelCardUi>();
                cardUi.Initialize(level, this);
                card.GetComponent<Button>().onClick.AddListener(cardUi.Pick);
            }
        }

        public void PickLevel(LevelInfo levelInfo)
        {
            mainMenu.PickLevel(levelInfo);
        }
    }
}
