using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace UI
{
    public class InventoryUi : MonoBehaviour
    {
        public PlayerReferences player;
        public GameObject[] slots;

        private void Awake()
        {
            player.inventory.OnInventoryChange += UpdateUi;
        }

        private void Start()
        {
            Assert.IsTrue(slots.Length == player.inventory.GetCapacity());
        }

        public void UpdateUi()
        {
            var weapons = player.inventory.GetWeapons();
            Assert.IsTrue(weapons.Count <= slots.Length); // No ability to dynamically change the number of displayed inventory slots for now.
            for (var i = 0; i < weapons.Count; i++)
            {
                var weapon = weapons[i];
                var slot = slots[i];
                slot.GetComponent<Image>().sprite = weapon.weaponInfo.image;
            }
        }
    }
}
