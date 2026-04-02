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
            statsText.text = $"Player stats:\nMovement speed: {_player.Stats.MovementSpeed}\nAttack: {_player.Stats.Attack}\nDefense: {_player.Stats.Defense}\nPickup range: {_player.Stats.PickupRange}";

            var levelManager = LevelManager.Instance;
            var levelTime = (int)levelManager.LevelTimeSeconds;
            var levelTimeSeconds = levelTime % 60;
            var levelTimeSecondsText = levelTimeSeconds < 10 ? $"0{levelTimeSeconds}" : $"{levelTimeSeconds}";
            var levelTimeMinutes = levelTime / 60;
            var levelTimeMinutesText = levelTimeMinutes < 10 ? $"0{levelTimeMinutes}" : $"{levelTimeMinutes}";
            timeText.text = $"{levelTimeMinutesText}:{levelTimeSecondsText}";
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
