using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject titleScreen;
        [SerializeField] private GameObject characterSelectionScreen;
        [SerializeField] private GameObject levelSelectionScreen;
        [SerializeField] private GameObject permanentUpgradesScreen;

        private void Start()
        {
            SwitchScreen(titleScreen);
        }

        public void OnStartButtonClicked()
        {
            SwitchScreen(characterSelectionScreen);
        }

        public void OnUpgradesButtonClicked()
        {
            SwitchScreen(permanentUpgradesScreen);
        }

        public void PickCharacter(CharacterInfo characterInfo)
        {
            PersistentData.Instance.selectedCharacter = characterInfo;
            SwitchScreen(levelSelectionScreen);
        }

        public void PickLevel(LevelInfo levelInfo)
        {
            PersistentData.Instance.selectedLevel = levelInfo;
            SceneManager.LoadScene("GameplayScene");
        }

        public void ReturnToTitleScreen()
        {
            SwitchScreen(titleScreen);
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
            titleScreen.SetActive(false);
            characterSelectionScreen.SetActive(false);
            levelSelectionScreen.SetActive(false);
            permanentUpgradesScreen.SetActive(false);

            screen.SetActive(true);
        }
    }
}
