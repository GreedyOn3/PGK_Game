using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class CharacterSelectionUi : MonoBehaviour
    {
        [SerializeField] private CharacterInfo[] characterInfos;
        [SerializeField] private GameObject characterCardPrefab;

        private void Start()
        {
            foreach (var character in characterInfos)
            {
                var card = Instantiate(characterCardPrefab, transform);
                var characterCardUi = card.GetComponent<CharacterCardUi>();
                Assert.IsNotNull(characterCardUi, "Level up option should have a LevelUpOptionUi component.");
                characterCardUi.Initialize(character, this);
            }
        }

        public void PickCharacter(CharacterId characterId)
        {
            Debug.Log($"Picked character {characterId}");
        }
    }
}
