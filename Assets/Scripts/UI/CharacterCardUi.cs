using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class CharacterCardUi : MonoBehaviour
    {
        private CharacterInfo _characterInfo;
        private CharacterSelectionUi _characterSelectionUi;

        public void Initialize(CharacterInfo character, CharacterSelectionUi characterSelectionUi)
        {
            _characterSelectionUi = characterSelectionUi;
            _characterInfo = character;
            var image = transform.Find("Image").GetComponent<Image>();
            var textContainer = transform.Find("Text Container");
            var name = textContainer.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            var description = textContainer.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            image.sprite = character.Image;
            name.text = character.CharacterName;
            description.text = character.Description;
        }

        public void Pick()
        {
            _characterSelectionUi.PickCharacter(_characterInfo);
        }
    }
}
