using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace UI
{
    public class GameOverUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statsText;

        private void Awake()
        {
            var levelStats = PersistentData.Instance.levelStats;
            statsText.text = levelStats.GetStatsText();
        }

        public void OnContinueButtonClicked()
        {
            PersistentData.Instance.levelStats = new LevelStats();
            SceneManager.LoadScene("MainMenu");
        }
    }
}
