using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class InGameUi : MonoBehaviour
    {
        [SerializeField] private InputActionReference togglePauseAction;

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
            var levelTimeMinutes = (int)levelManager.LevelTimeMinutes;
            var levelTimeSeconds = (int)levelManager.LevelTimeSeconds;
            timeText.text = Util.FormatLevelTime(levelTimeMinutes, levelTimeSeconds);
        }

        private void UpdatePlayerStatsDisplay()
        {
            PlayerStats stats = _player.Stats;
            var text = "Player stats\n";
            text += GetStatModifierText("Movement speed", stats.GetModifierValue(StatType.MoveSpeed));
            text += GetStatModifierText("Attack", stats.GetModifierValue(StatType.Attack));
            text += GetStatModifierText("Defense", stats.GetModifierValue(StatType.Defense));
            text += GetStatModifierText("Pickup range", stats.GetModifierValue(StatType.PickupRange));
            statsText.text = text;
        }

        private static string GetStatModifierText(string statName, float stat)
        {
            return stat < 0 ? $"{statName}: {stat}%\n" : $"{statName}: +{stat}%\n";
        }

        private void OnEnable()
        {
            togglePauseAction.action.performed += TogglePauseMenu;
        }

        private void OnDisable()
        {
            togglePauseAction.action.performed -= TogglePauseMenu;
        }

        private void TogglePauseMenu(InputAction.CallbackContext callbackContext)
        {
            var menuActive = pauseMenu.activeSelf;
            pauseMenu.SetActive(!menuActive);
        }
    }
}
