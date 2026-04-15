using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        private bool _levelWasPaused;
        private InputMode _savedInputMode;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void OnResumeButtonClicked()
        {
            gameObject.SetActive(false);
        }

        public void OnQuitButtonClicked()
        {
            SceneManager.LoadScene("MainMenu");
        }

        private void OnEnable()
        {
            var levelManager = LevelManager.Instance;
            var inputManager = InputManager.Instance;
            _levelWasPaused = levelManager.LevelPaused;
            LevelManager.Instance.PauseLevel();
            _savedInputMode = inputManager.InputMode;
            inputManager.SwitchInputMode(InputMode.Ui);
        }

        private void OnDisable()
        {
            if (!_levelWasPaused)
            {
                var levelManager = LevelManager.Instance;
                if (levelManager != null)
                {
                    levelManager.UnpauseLevel();
                }
            }

            var inputManager = InputManager.Instance;
            if (inputManager != null)
            {
                InputManager.Instance.SwitchInputMode(_savedInputMode);
            }
        }
    }
}
