using UnityEngine;

namespace UI
{
    public class LevelUpUi : MonoBehaviour
    {
        public PlayerReferences player;
        public GameObject levelUpScreen;
        public LevelUpOptionUi[] options;
        public StatUpgradeInfo[] upgrades;

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

            for (var i = 0; i < 3; i++)
            {
                options[i].levelUpUi = this;
                options[i].SetStatUpgrade(upgrades[i]);
            }
        }
    }
}
