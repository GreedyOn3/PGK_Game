using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class InGameUi : MonoBehaviour
    {
        [SerializeField] private InputActionReference pauseAction;
        [SerializeField] private InputActionReference unpauseAction;

        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider xpSlider;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI statsText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private GameObject pauseMenu;

        [SerializeField] private PlayerReferences player;

        private void Awake()
        {
            pauseAction.action.performed += TogglePauseMenu;
            unpauseAction.action.performed += TogglePauseMenu;
        }

        private void Update()
        {
            healthSlider.value = player.Health.Value;
            healthSlider.maxValue = player.Health.MaxValue;
            xpSlider.value = player.Xp.Value;
            xpSlider.maxValue = player.Xp.MaxValue;
            levelText.text = $"Level {player.Xp.Level}";
            statsText.text = $"Player stats:\nMovement speed: {player.Stats.MovementSpeed}\nAttack: {player.Stats.Attack}\nDefense: {player.Stats.Defense}";

            var levelManager = LevelManager.Instance;
            var levelTime = (int)levelManager.LevelTimeSeconds;
            var levelTimeSeconds = levelTime % 60;
            var levelTimeSecondsText = levelTimeSeconds < 10 ? $"0{levelTimeSeconds}" : $"{levelTimeSeconds}";
            var levelTimeMinutes = levelTime / 60;
            var levelTimeMinutesText = levelTimeMinutes < 10 ? $"0{levelTimeMinutes}" : $"{levelTimeMinutes}";
            timeText.text = $"{levelTimeMinutesText}:{levelTimeSecondsText}";
        }

        private void TogglePauseMenu(InputAction.CallbackContext callbackContext)
        {
            var menuActive = pauseMenu.activeSelf;
            pauseMenu.SetActive(!menuActive);
        }
    }
}
