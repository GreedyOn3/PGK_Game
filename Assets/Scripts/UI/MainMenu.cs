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
        [SerializeField] private GameObject achievementsScreen;
        [SerializeField] private GameObject deleteSaveGameScreen;

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

        public void OnAchievementsButtonClicked()
        {
            SwitchScreen(achievementsScreen);
        }

        public void PickCharacter(CharacterInfo characterInfo)
        {
            PersistentData.Instance.selectedCharacter = characterInfo;
            SwitchScreen(levelSelectionScreen);
        }

        public void CancelCharacterSelection()
        {
            SwitchScreen(titleScreen);
        }

        public void PickLevel(LevelInfo levelInfo)
        {
            PersistentData.Instance.selectedLevel = levelInfo;
            SceneManager.LoadScene("GameplayScene");
        }

        public void CancelLevelSelection()
        {
            SwitchScreen(characterSelectionScreen);
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

        public void GoToDeleteSaveGameScreen()
        {
            SwitchScreen(deleteSaveGameScreen);
        }

        public void ReturnFromDeleteSaveGameScreen()
        {
            SwitchScreen(permanentUpgradesScreen);
        }

        private void SwitchScreen(GameObject screen)
        {
            titleScreen.SetActive(false);
            characterSelectionScreen.SetActive(false);
            levelSelectionScreen.SetActive(false);
            permanentUpgradesScreen.SetActive(false);
            achievementsScreen.SetActive(false);
            deleteSaveGameScreen.SetActive(false);

            screen.SetActive(true);
        }
    }
}
