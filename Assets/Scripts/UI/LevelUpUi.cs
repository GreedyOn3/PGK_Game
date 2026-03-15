using System;
using UnityEngine;

namespace UI
{
    public class LevelUpUi : MonoBehaviour
    {
        public PlayerXp playerXp;
        public PlayerStats playerStats;
        public GameObject levelUpScreen;
        public LevelUpOptionUi[] options;
        public Upgrade[] upgrades;

        private void Awake()
        {
            playerXp.OnLevelUp += LevelUp;
            levelUpScreen.SetActive(false);
        }

        public void PickUpgrade(Upgrade upgrade)
        {
            switch (upgrade.type)
            {
                case UpgradeType.MovementSpeed:
                    playerStats.movementSpeed += 1.0f;
                    break;
                case UpgradeType.Attack:
                    playerStats.attack += 1.0f;
                    break;
                case UpgradeType.Defense:
                    playerStats.defense += 1.0f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            levelUpScreen.SetActive(false);
            Time.timeScale = 1.0f;
        }

        private void LevelUp()
        {
            Time.timeScale = 0.0f;
            levelUpScreen.SetActive(true);

            for (var i = 0; i < 3; i++)
            {
                options[i].levelUpUi = this;
                options[i].SetUpgrade(upgrades[i]);
            }
        }
    }
}
