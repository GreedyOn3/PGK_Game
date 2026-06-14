using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField] private ItemPool itemPool;

    private void OnEnable()
    {
        ChestObject.OnChestOpened += HandleChestOpened;
    }

    private void OnDisable()
    {
        ChestObject.OnChestOpened -= HandleChestOpened;
    }

    private void HandleChestOpened(PlayerReferences player, ChestObject chest)
    {
        ChoiceRarity rarity = itemPool.GetRandomRarity(player.Stats, true);

        if (rarity != null && rarity.SpecialItems.Count > 0)
        {
            SpecialItemInfo specialItem = Util.GetRandomWeighted(rarity.SpecialItems);

            if (ChestUI.Instance != null)
                ChestUI.Instance.OpenChest(rarity, specialItem);

            player.Inventory.AddSpecial(specialItem);
        }

        Destroy(chest.gameObject);
    }
}