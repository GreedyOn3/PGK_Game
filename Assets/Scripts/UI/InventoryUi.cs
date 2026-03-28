using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class InventoryUi : MonoBehaviour
    {
        [SerializeField] private PlayerReferences player;
        [SerializeField] private GameObject slotPrefab;

        private GameObject[] _slots;

        private void Awake()
        {
            player.Inventory.OnInventoryChange += UpdateUi;

            var capacity = player.Inventory.GetCapacity();
            _slots = new GameObject[capacity];

            for (var i = 0; i < capacity; i++)
            {
                _slots[i] = Instantiate(slotPrefab, transform);
            }
        }

        private void Start()
        {
            Assert.IsTrue(_slots.Length == player.Inventory.GetCapacity());
        }

        public void UpdateUi()
        {
            var weapons = player.Inventory.GetWeapons();
            Assert.IsTrue(weapons.Count <= _slots.Length);
            for (var i = 0; i < weapons.Count; i++)
            {
                var weapon = weapons[i];
                var slot = _slots[i];
                slot.GetComponent<Image>().sprite = weapon.weaponInfo.image;
            }
        }
    }
}
