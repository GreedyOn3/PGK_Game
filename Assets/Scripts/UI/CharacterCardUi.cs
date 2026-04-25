using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CardElements))]
    public class CharacterCardUi : MonoBehaviour
    {
        private CharacterInfo _characterInfo;
        private CharacterSelectionUi _characterSelectionUi;

        public void Initialize(CharacterInfo character, CharacterSelectionUi characterSelectionUi)
        {
            _characterSelectionUi = characterSelectionUi;
            _characterInfo = character;
            var cardElements = GetComponent<CardElements>();
            cardElements.Image.sprite = character.Image;
            cardElements.MainText.text = character.CharacterName;
            cardElements.SubText.text = character.Description;
        }

        public void Pick()
        {
            _characterSelectionUi.PickCharacter(_characterInfo);
        }
    }
}
