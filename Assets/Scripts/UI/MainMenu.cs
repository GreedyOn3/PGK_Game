using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        public void OnStartButtonClicked()
        {
            SceneManager.LoadScene("TestScene");
        }
    }
}
