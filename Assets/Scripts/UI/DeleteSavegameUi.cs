using UnityEngine;

namespace UI
{
    public class DeleteSavegameUi : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;

        public void OnReturnButtonClicked()
        {
            mainMenu.ReturnFromDeleteSaveGameScreen();
        }

        public void OnDeleteSavegameButtonClicked()
        {
            SaveManager.instance.DeleteSavegame();
            PersistentData.Instance.ResetPermanentUpgrades();
            PersistentData.Instance.ResetAchievements();
            mainMenu.ReturnFromDeleteSaveGameScreen();
        }
    }
}
