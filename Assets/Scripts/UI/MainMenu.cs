using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _titleScreen;
        [SerializeField] private GameObject _characterSelectionScreen;
        [SerializeField] private GameObject _levelSelectionScreen;
        [SerializeField] private GameObject _permanentUpgradesScreen;

        private void Start()
        {
            SwitchScreen(_titleScreen);
        }

        public void OnStartButtonClicked()
        {
            SwitchScreen(_characterSelectionScreen);
        }

        public void OnUpgradesButtonClicked()
        {
            SwitchScreen(_permanentUpgradesScreen);
        }

        public void PickCharacter(CharacterInfo characterInfo)
        {
            PersistentData.Instance.selectedCharacter = characterInfo;
            SwitchScreen(_levelSelectionScreen);
        }

        public void PickLevel(LevelInfo levelInfo)
        {
            PersistentData.Instance.selectedLevel = levelInfo;
            SceneManager.LoadScene("GameplayScene");
        }

        public void ReturnToTitleScreen()
        {
            SwitchScreen(_titleScreen);
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
            _permanentUpgradesScreen.SetActive(false);

            screen.SetActive(true);
        }
    }
}
