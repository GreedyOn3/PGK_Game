using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class LevelUpUi : MonoBehaviour
    {
        [SerializeField] private GameObject optionPrefab;
        [SerializeField] private PlayerReferences player;
        [SerializeField] private GameObject levelUpScreen;
        [SerializeField] private StatUpgradeInfo[] upgrades;

        private GameObject[] _options = Array.Empty<GameObject>();

        private void Awake()
        {
            player.Xp.OnLevelUp += LevelUp;
            levelUpScreen.SetActive(false);
        }

        public void PickStatUpgrade(StatUpgradeType upgrade)
        {
            player.Stats.ApplyStatUpgrade(upgrade);
            LevelManager.Instance.UnpauseLevel();
            InputManager.Instance.SwitchInputMode(InputMode.Gameplay);
            levelUpScreen.SetActive(false);
        }

        private void LevelUp()
        {
            LevelManager.Instance.PauseLevel();
            InputManager.Instance.SwitchInputMode(InputMode.Ui);
            levelUpScreen.SetActive(true);

            foreach (var option in _options)
            {
                Destroy(option);
            }

            _options = new GameObject[upgrades.Length];

            for (var i = 0; i < upgrades.Length; i++)
            {
                _options[i] = Instantiate(optionPrefab, levelUpScreen.transform);
                var optionUi = _options[i].GetComponent<LevelUpOptionUi>();
                Assert.IsNotNull(optionUi, "Level up option should have a LevelUpOptionUi component.");
                optionUi.Initialize(upgrades[i], this);
            }
        }
    }
}
