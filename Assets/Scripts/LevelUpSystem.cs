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

    private void LevelUp()
    {
        List<LevelUpChoice> randomChoices = GenerateLevelUp(_player.Inventory);
        if(randomChoices.Count > 0)
            levelUpUI.Show(randomChoices);
    }

    public void CacheWeaponUpgrades(WeaponInfo weapon)
    {
        _weaponUpgrades.Add(weapon, weapon.UpgradePool);
    }

    public List<LevelUpChoice> GenerateLevelUp(PlayerInventory inv)
    {
        List<LevelUpChoice> workingPool = BuildPool(inv);
        if (workingPool.Count == 0)
            return new List<LevelUpChoice>();

        List<LevelUpChoice> results = new();
        for (int i = 0; i < choicesCount; i++)
        {
            if (workingPool.Count == 0)
                break;

            LevelUpChoice choice = GetRandomWeighted(workingPool, c => c.Item.Weight);
            if (choice.Type == ChoiceType.UpgradeWeapon || choice.Type == ChoiceType.UpgradePassive)
            {
                choice.Rarity = GetRandomWeighted(choiceRarities, r => r.Weight);

                StatInfo stat = (choice.Type == ChoiceType.UpgradeWeapon) ? GetRandom(_weaponUpgrades[(WeaponInfo)choice.Item]) : ((PassiveItemInfo)choice.Item).Stat;
                stat.Value *= choice.Rarity.Multiplier;
                if (choice.Type == ChoiceType.UpgradePassive) 
                    stat.IsPercentage = true;

                choice.Stats.Add(stat);
            }

            results.Add(choice);
            workingPool.Remove(choice);
        }

        return results;
    }

    List<LevelUpChoice> BuildPool(PlayerInventory inv)
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

    T GetRandomWeighted<T>(List<T> items, Func<T, float> getWeight)
    {
        float totalWeight = 0f;
        foreach (T item in items)
            totalWeight += getWeight(item);

        float randomPoint = UnityEngine.Random.Range(0, totalWeight);
        float current = 0f;
        foreach (T item in items)
        {
            current += getWeight(item);
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

[Serializable]
public class LevelUpChoice
{
    public BaseItemInfo Item;
    public ChoiceType Type;

    public ChoiceRarity Rarity;
    public List<StatInfo> Stats = new();
}

[Serializable]
public class ChoiceRarity
{
    public string Name;
    public Color color;
    public float Weight;
    public float Multiplier;
}

public enum ChoiceType
{
    NewWeapon,
    NewPassive,
    UpgradePassive,
    UpgradeWeapon
}