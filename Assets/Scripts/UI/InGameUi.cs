using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
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

        private PlayerReferences _player;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>();
            Assert.IsNotNull(_player, "Player object should have PlayerReferences component.");
        }

        private void Update()
        {
            healthSlider.value = _player.Health.Value;
            healthSlider.maxValue = _player.Health.MaxValue;
            xpSlider.value = _player.Xp.Value;
            xpSlider.maxValue = _player.Xp.MaxValue;
            levelText.text = $"Level {_player.Xp.Level}";
            UpdatePlayerStatsDisplay();

            var levelManager = LevelManager.Instance;
            var levelTime = (int)levelManager.LevelTimeSeconds;
            var levelTimeSeconds = levelTime % 60;
            var levelTimeSecondsText = levelTimeSeconds < 10 ? $"0{levelTimeSeconds}" : $"{levelTimeSeconds}";
            var levelTimeMinutes = levelTime / 60;
            var levelTimeMinutesText = levelTimeMinutes < 10 ? $"0{levelTimeMinutes}" : $"{levelTimeMinutes}";
            timeText.text = $"{levelTimeMinutesText}:{levelTimeSecondsText}";
        }

        private void UpdatePlayerStatsDisplay()
        {
            var modifiers = _player.Stats.Modifiers;
            var text = "Player stats\n";
            text += GetStatModifierText("Movement speed", modifiers.movementSpeedModifier);
            text += GetStatModifierText("Attack", modifiers.attackModifier);
            text += GetStatModifierText("Defense", modifiers.defenseModifier);
            text += GetStatModifierText("Pickup range", modifiers.pickupRangeModifier);
            statsText.text = text;
        }

        private static string GetStatModifierText(string statName, float stat)
        {
            return stat < 0 ? $"{statName}: {stat}%\n" : $"{statName}: +{stat}%\n";
        }

        private void OnEnable()
        {
            pauseAction.action.performed += TogglePauseMenu;
            unpauseAction.action.performed += TogglePauseMenu;
        }

        private void OnDisable()
        {
            pauseAction.action.performed -= TogglePauseMenu;
            unpauseAction.action.performed -= TogglePauseMenu;
        }

        private void TogglePauseMenu(InputAction.CallbackContext callbackContext)
        {
            var menuActive = pauseMenu.activeSelf;
            pauseMenu.SetActive(!menuActive);
        }
    }
}
