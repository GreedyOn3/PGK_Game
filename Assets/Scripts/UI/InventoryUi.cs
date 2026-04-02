using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class InventoryUi : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;

        private PlayerReferences _player;
        private GameObject[] _slots;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>();

            _player.Inventory.OnInventoryChange += UpdateUi;
            var capacity = _player.Inventory.GetCapacity();
            _slots = new GameObject[capacity];

            for (var i = 0; i < capacity; i++)
            {
                _slots[i] = Instantiate(slotPrefab, transform);
            }

            Assert.IsTrue(_slots.Length == _player.Inventory.GetCapacity());
            UpdateUi();
        }

        public void UpdateUi()
        {
            var weapons = _player.Inventory.GetWeapons();
            Assert.IsTrue(weapons.Count <= _slots.Length);
            for (var i = 0; i < weapons.Count; i++)
            {
                var weapon = weapons[i];
                var slot = _slots[i];
                slot.GetComponent<Image>().sprite = weapon.WeaponInfo.image;
            }
        }
    }
}
