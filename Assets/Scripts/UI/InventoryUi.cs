using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class InventoryUi : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform weaponsContainer;
        [SerializeField] private Transform passivesContainer;

        private PlayerReferences _player;
        private GameObject[] _weaponSlots;
        private GameObject[] _passiveSlots;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>();

            _player.Inventory.OnInventoryChange += UpdateUi;

            var weaponCapacity = _player.Inventory.GetWeaponCapacity();
            _weaponSlots = new GameObject[weaponCapacity];
            var passiveCapacity = _player.Inventory.GetPassivesCapacity();
            _passiveSlots = new GameObject[passiveCapacity];

            for (var i = 0; i < weaponCapacity; i++)
                _weaponSlots[i] = Instantiate(slotPrefab, weaponsContainer);
            for (var i = 0; i < passiveCapacity; i++)
                _passiveSlots[i] = Instantiate(slotPrefab, passivesContainer);

            Assert.IsTrue(_weaponSlots.Length == _player.Inventory.GetWeaponCapacity());
            UpdateUi();
        }

        public void UpdateUi()
        {
            var weapons = _player.Inventory.GetWeapons();
            var passives = _player.Inventory.GetPassives();

            Assert.IsTrue(weapons.Count <= _weaponSlots.Length);
            for (var i = 0; i < weapons.Count; i++)
            {
                var weapon = weapons[i];
                var slot = _weaponSlots[i];
                var image = slot.transform.Find("Image").GetComponent<Image>();
                image.sprite = weapon.weaponInfo.Image;
                image.color = Color.white;
            }

            for (var i = 0; i < passives.Count; i++)
            {
                var passive = passives[i];
                var slot = _passiveSlots[i];
                var image = slot.transform.Find("Image").GetComponent<Image>();
                image.sprite = passive.info.Image;
                image.color = Color.white;
            }
        }
    }
}
