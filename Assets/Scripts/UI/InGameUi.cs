using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class InGameUi : MonoBehaviour
    {
        public InputActionReference pauseAction;

        public Slider healthSlider;
        public Slider xpSlider;
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI statsText;
        public TextMeshProUGUI timeText;
        public GameObject pauseMenu;

        public PlayerReferences player;

        private void Update()
        {
            if (pauseAction.action.WasPressedThisFrame())
            {
                var active = pauseMenu.activeSelf;
                pauseMenu.SetActive(!active);
            }

            healthSlider.value = player.health.value;
            healthSlider.maxValue = player.health.maxValue;
            xpSlider.value = player.xp.value;
            xpSlider.maxValue = player.xp.maxValue;
            levelText.text = $"Level {player.xp.Level}";
            statsText.text = $"Player stats:\nMovement speed: {player.stats.movementSpeed}\nAttack: {player.stats.attack}\nDefense: {player.stats.defense}";

            var levelManager = LevelManager.Instance;
            var levelTime = (int)levelManager.LevelTimeSeconds;
            var levelTimeSeconds = levelTime % 60;
            var levelTimeSecondsText = levelTimeSeconds < 10 ? $"0{levelTimeSeconds}" : $"{levelTimeSeconds}";
            var levelTimeMinutes = levelTime / 60;
            var levelTimeMinutesText = levelTimeMinutes < 10 ? $"0{levelTimeMinutes}" : $"{levelTimeMinutes}";
            timeText.text = $"{levelTimeMinutesText}:{levelTimeSecondsText}";
        }
    }
}
