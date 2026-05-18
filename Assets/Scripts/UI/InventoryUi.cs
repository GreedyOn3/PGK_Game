using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections.Generic;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class InventoryUi : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private GameObject specialItemPrefab;

        [SerializeField] private Transform weaponsContainer;
        [SerializeField] private Transform passivesContainer;
        [SerializeField] private Transform specialItemsContainer;

        private PlayerReferences _player;
        private GameObject[] _weaponSlots;
        private GameObject[] _passiveSlots;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>();

            _player.Inventory.OnInventoryChange += UpdateUi;
            _player.Inventory.OnSpecialsChange += UpdateSpecialsUi;

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
            UpdateSpecialsUi();
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

        public void UpdateSpecialsUi()
        {
            Debug.Log("TEST");
            Dictionary<SpecialItemInfo, SpecialItem> specials = _player.Inventory.GetSpecialItems();

            foreach (Transform child in specialItemsContainer)
            {
                Destroy(child.gameObject);
            }

            foreach(var item in specials) 
            {
                Image img = Instantiate(specialItemPrefab, specialItemsContainer).GetComponent<Image>();
                img.sprite = item.Key.Image;
            }
        }
    }
}
