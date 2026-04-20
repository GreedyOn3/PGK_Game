using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CharacterSelectionUi : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private CharacterInfo[] characterInfos;
        [SerializeField] private GameObject cardPrefab;

        private void Start()
        {
            foreach (var character in characterInfos)
            {
                var card = Instantiate(cardPrefab, transform);
                var cardUi = card.AddComponent<CharacterCardUi>();
                cardUi.Initialize(character, this);
                card.GetComponent<Button>().onClick.AddListener(cardUi.Pick);
            }
        }

        public void PickCharacter(CharacterInfo characterInfo)
        {
            mainMenu.PickCharacter(characterInfo);
        }
    }
}
