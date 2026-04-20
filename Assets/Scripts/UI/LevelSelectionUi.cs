using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectionUi : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private LevelInfo[] levelInfos;
        [SerializeField] private GameObject cardPrefab;

        private void Start()
        {
            foreach (var level in levelInfos)
            {
                var card = Instantiate(cardPrefab, transform);
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
