using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        private GameObject _titleScreen;
        private GameObject _characterSelectionScreen;
        private GameObject _levelSelectionScreen;

        private void Start()
        {
            _titleScreen = transform.Find("Title Screen").gameObject;
            _characterSelectionScreen = transform.Find("Character Selection").gameObject;
            _levelSelectionScreen = transform.Find("Level Selection").gameObject;
            SwitchScreen(_titleScreen);
        }

        public void OnStartButtonClicked()
        {
            SwitchScreen(_characterSelectionScreen);
        }

        public void PickCharacter(CharacterId characterId)
        {
            PersistentData.Instance.SelectedCharacter = characterId;
            SwitchScreen(_levelSelectionScreen);
        }

        public void PickLevel(LevelId levelId)
        {
            PersistentData.Instance.SelectedLevel = levelId;

            switch (levelId)
            {
                case LevelId.Forest:
                    SceneManager.LoadScene(SceneIndex.LevelForest);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(levelId), levelId, null);
            }
        }

        public void OnQuitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void SwitchScreen(GameObject screen)
        {
            _titleScreen.SetActive(false);
            _characterSelectionScreen.SetActive(false);
            _levelSelectionScreen.SetActive(false);

            screen.SetActive(true);
        }
    }
}
