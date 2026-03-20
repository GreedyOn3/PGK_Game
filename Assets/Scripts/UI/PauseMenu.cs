using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        private float _prevTimeScale = 1.0f;

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
            _prevTimeScale = Time.timeScale;
            Time.timeScale = 0.0f;
        }

        private void OnDisable()
        {
            Time.timeScale = _prevTimeScale;
        }
    }
}
