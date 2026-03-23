using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class InGameUi : MonoBehaviour
    {
        [SerializeField] private InputActionReference pauseAction;

        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider xpSlider;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI statsText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private GameObject pauseMenu;

        [SerializeField] private PlayerReferences player;

        private void Update()
        {
            if (pauseAction.action.WasPressedThisFrame())
            {
                var active = pauseMenu.activeSelf;
                pauseMenu.SetActive(!active);
            }

            healthSlider.value = player.health.Value;
            healthSlider.maxValue = player.health.MaxValue;
            xpSlider.value = player.xp.Value;
            xpSlider.maxValue = player.xp.MaxValue;
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
