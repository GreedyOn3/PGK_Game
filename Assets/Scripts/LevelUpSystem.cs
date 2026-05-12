using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    [Header("UI")]
    public LevelUpUi levelUpUI;
    [Header("Item Pools & Rarities")]
    public List<WeaponInfo> allWeapons;
    public List<PassiveItemInfo> allPassives;
    public List<ChoiceRarity> choiceRarities;
    [Header("Limits")]
    public int choicesCount = 3;

    private Dictionary<WeaponInfo, List<StatInfo>> _weaponUpgrades = new();
    private int _maxWeapons = 6;
    private int _maxPassives = 6;

    public static LevelUpSystem Instance { get; private set; }
    private PlayerReferences _player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>();
        _player.Xp.OnLevelUp += LevelUp;
        _maxWeapons = _player.Inventory.GetWeaponCapacity();
        _maxPassives = _player.Inventory.GetPassivesCapacity();
    }

    private void LevelUp(PlayerXp xp)
    {
        List<LevelUpChoice> randomChoices = GenerateLevelUp(_player.Inventory, xp.Level);
        if(randomChoices.Count > 0)
            levelUpUI.Show(randomChoices);
    }

    public void CacheWeaponUpgrades(WeaponInfo weapon)
    {
        _weaponUpgrades.Add(weapon, weapon.UpgradePool);
    }

    public List<LevelUpChoice> GenerateLevelUp(PlayerInventory inv, int level)
    {
        List<LevelUpChoice> newItems = BuildNewPool(inv);
        List<LevelUpChoice> upgrades = BuildUpgradePool(inv);
        List<LevelUpChoice> results = new();

        int amount = (UnityEngine.Random.value < CalculateAdditionalChance()) ? choicesCount + 1 : choicesCount;

        if (!inv.IsFull() && newItems.Count != 0)
        {
            float ownedChance = CalculateOwnedChance(level);
            for (int i = 0; i < choicesCount - 1; i++)
            {
                if (upgrades.Count <= 0) break;
                bool guarantee = ( amount - (newItems.Count + results.Count) ) > 0;

                if (guarantee || UnityEngine.Random.value < ownedChance)
                    results.Add(GetRandomUpgrade(upgrades));
            }

            for (int i = results.Count; i < amount; i++)
            {
                if (newItems.Count <= 0) break;

                // Choosing random new item and adding to results
                results.Add(GetRandomNewItem(newItems));
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                if (upgrades.Count <= 0) break;

                // Choosing random upgrade and adding to results
                results.Add(GetRandomUpgrade(upgrades));
            }
        }

        return results;
    }

    List<LevelUpChoice> BuildNewPool(PlayerInventory inv)
    {
        List<LevelUpChoice> pool = new();

        if (inv.GetWeapons().Count < _maxWeapons)
        {
            foreach (WeaponInfo item in allWeapons)
            {
                if (!inv.HasItem(item))
                {
                    pool.Add(new LevelUpChoice
                    {
                        Item = item,
                        Type = ChoiceType.NewWeapon
                    });
                }
            }
        }

        if (inv.GetPassives().Count < _maxPassives)
        {
            foreach (PassiveItemInfo item in allPassives)
            {
                if (!inv.HasItem(item))
                {
                    pool.Add(new LevelUpChoice
                    {
                        Item = item,
                        Type = ChoiceType.NewPassive
                    });
                }
            }
        }

        return pool;
    }

    List<LevelUpChoice> BuildUpgradePool(PlayerInventory inv)
    {
        List<LevelUpChoice> pool = new();

        foreach (PassiveItem passive in inv.GetPassives())
        {
            pool.Add(new LevelUpChoice
            {
                Item = passive.info,
                Type = ChoiceType.UpgradePassive
            });
        }

        foreach (var (weapon, _) in _weaponUpgrades)
        {
            pool.Add(new LevelUpChoice
            {
                Item = weapon,
                Type = ChoiceType.UpgradeWeapon
            });
        }

        return pool;
    }

    public void ApplyChoice(LevelUpChoice choice, PlayerInventory inv)
    {
        switch (choice.Type)
        {
            case ChoiceType.NewWeapon:
                inv.AddWeapon((WeaponInfo)choice.Item);
                break;

            case ChoiceType.NewPassive:
                inv.AddPassive((PassiveItemInfo)choice.Item);
                break;

            case ChoiceType.UpgradePassive:
                inv.UpgradePassive((PassiveItemInfo)choice.Item, choice.Stats[0].Value);
                break;
            case ChoiceType.UpgradeWeapon:
                inv.UpgradeWeapon((WeaponInfo)choice.Item, choice.Stats);
                break;
        }
    }

    float CalculateAdditionalChance()
    {
        PlayerStats stats = _player.Stats;
        float luck = 1f + (stats.GetStatModifier(StatType.Luck).value / 100f);

        return 1f - (1f / luck);
    }

    float CalculateOwnedChance(int level)
    {
        PlayerStats stats = _player.Stats;
        float luck = 1f + ( stats.GetStatModifier(StatType.Luck).value / 100f );
        int x = (level % 2 == 0) ? 2 : 1;

        return 1f + 0.3f * x - (1f / luck);
    }

    LevelUpChoice GetRandomUpgrade(List<LevelUpChoice> upgrades)
    {
        LevelUpChoice choice = GetRandomWeighted(upgrades);
        choice.Rarity = GetRandomRarity();

        // Choosing stat to upgrade
        StatInfo stat = (choice.Type == ChoiceType.UpgradeWeapon) ?
            GetRandom(_weaponUpgrades[(WeaponInfo)choice.Item]) :
            ((PassiveItemInfo)choice.Item).Stat;

        stat.Value *= choice.Rarity.Multiplier;
        if (choice.Type == ChoiceType.UpgradePassive)
            stat.IsPercentage = _player.Stats.GetStatModifier(stat.Type).isPercentage;

        choice.Stats.Add(stat);

        // Removing from current upgrades pool
        upgrades.Remove(choice);
        return choice;
    }

    LevelUpChoice GetRandomNewItem(List<LevelUpChoice> newItems)
    {
        // Choosing random new item
        LevelUpChoice choice = GetRandomWeighted(newItems);

        // Removing from current pool
        newItems.Remove(choice);
        return choice;
    }

    ChoiceRarity GetRandomRarity()
    {
        float luck = 1f + (_player.Stats.GetStatModifier(StatType.Luck).value / 100f);

        float luckStrength = Mathf.Log(luck) * 1.5f;
        float adjustValue = 1.5f;

        int raritiesCount = choiceRarities.Count;

        List<float> adjustedWeights = new();
        float totalWeight = 0f;

        for (int i = 0; i < raritiesCount; i++)
        {
            float exponent = ( (raritiesCount - 1) - i) * luckStrength;
            float adjusted = choiceRarities[i].Weight * Mathf.Pow(adjustValue, -exponent);

            adjustedWeights.Add(adjusted);
            totalWeight += adjusted;
        }

        float randomPoint = UnityEngine.Random.Range(0, totalWeight);
        float current = 0f;
        for (int i = 0; i < raritiesCount; i++)
        {
            current += adjustedWeights[i];
            if (randomPoint <= current)
                return choiceRarities[i];
        }

        return choiceRarities[0];
    }

    T GetRandomWeighted<T>(List<T> items) where T : IWeighted
    {
        float totalWeight = 0f;
        foreach (T item in items)
            totalWeight += item.Weight;

        float randomPoint = UnityEngine.Random.Range(0, totalWeight);
        float current = 0f;
        foreach (T item in items)
        {
            current += item.Weight;
            if (randomPoint <= current)
                return item;
        }

        return items[^1];
    }

    T GetRandom<T>(List<T> items)
    {
        return items[UnityEngine.Random.Range(0, items.Count)];
    }
}

public interface IWeighted
{
    float Weight { get; }
}

[Serializable]
public class LevelUpChoice : IWeighted
{
    public BaseItemInfo Item;
    public ChoiceType Type;

    public ChoiceRarity Rarity;
    public List<StatInfo> Stats = new();

    public float Weight => (Item != null) ? Item.Weight : 0f;
}

[Serializable]
public class ChoiceRarity : IWeighted
{
    public string Name;
    public Color color;
    [field: SerializeField] public float Weight { get; set; }
    public float Multiplier;
}

public enum ChoiceType
{
    NewWeapon,
    NewPassive,
    UpgradePassive,
    UpgradeWeapon
}