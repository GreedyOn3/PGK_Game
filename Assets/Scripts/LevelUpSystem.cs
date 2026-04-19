using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour
{
    [Header("UI")]
    public LevelUpUi levelUpUI;
    [Header("Item Pools")]
    public List<WeaponInfo> allWeapons;
    public List<PassiveItemInfo> allPassives;
    [Header("Limits")]
    public int choicesCount = 3;

    private int maxWeapons = 6;
    private int maxPassives = 6;

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
        maxWeapons = _player.Inventory.GetWeaponCapacity();
        maxPassives = _player.Inventory.GetPassivesCapacity();
    }

    private void LevelUp()
    {
        List<LevelUpChoice> randomChoices = GenerateLevelUp(_player.Inventory);
        if(randomChoices.Count > 0)
            levelUpUI.Show(randomChoices);
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

            LevelUpChoice choice = GetRandomWeighted(workingPool, c => c.item.Weight);

            results.Add(choice);
            workingPool.Remove(choice);
        }

        return results;
    }

    List<LevelUpChoice> BuildPool(PlayerInventory inv)
    {
        var pool = new List<LevelUpChoice>();

        if (inv.GetWeapons().Count < maxWeapons)
        {
            foreach (var item in allWeapons)
            {
                if (!inv.HasItem(item))
                {
                    pool.Add(new LevelUpChoice
                    {
                        item = item,
                        type = ChoiceType.NewWeapon
                    });
                }
            }
        }

        if (inv.GetPassives().Count < maxPassives)
        {
            foreach (var item in allPassives)
            {
                if (!inv.HasItem(item))
                {
                    pool.Add(new LevelUpChoice
                    {
                        item = item,
                        type = ChoiceType.NewPassive
                    });
                }
            }
        }

        foreach (PassiveItem passive in inv.GetPassives())
        {
            pool.Add(new LevelUpChoice
            {
                item = passive.info,
                type = ChoiceType.UpgradePassive
            });
        }

        return pool;
    }

    public void ApplyChoice(LevelUpChoice choice, PlayerInventory inv)
    {
        switch (choice.type)
        {
            case ChoiceType.NewWeapon:
                inv.AddWeapon((WeaponInfo)choice.item);
                break;

            case ChoiceType.NewPassive:
                inv.AddPassive((PassiveItemInfo)choice.item);
                break;

            case ChoiceType.UpgradePassive:
                inv.UpgradePassive((PassiveItemInfo)choice.item);
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
}

[Serializable]
public class LevelUpChoice
{
    public BaseItemInfo item;
    public ChoiceType type;
}

public enum ChoiceType
{
    NewWeapon,
    NewPassive,
    UpgradePassive
}