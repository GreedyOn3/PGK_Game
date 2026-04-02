using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        private GameObject _titleScreen;
        private GameObject _characterSelectionScreen;

        private void Start()
        {
            _titleScreen = transform.Find("Title Screen").gameObject;
            _characterSelectionScreen = transform.Find("Character Selection").gameObject;
            SwitchScreen(_titleScreen);
        }

        public void OnStartButtonClicked()
        {
            SwitchScreen(_characterSelectionScreen);
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

            screen.SetActive(true);
        }
    }
}
