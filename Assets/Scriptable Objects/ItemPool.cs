using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Pool", menuName = "Scriptable Objects/Item Pool")]
public class ItemPool : ScriptableObject
{
    [Header("Rarities & Special Items")]
    [SerializeField] private List<ChoiceRarity> choiceRarities;
    [Header("Weapons & Passives")]
    [SerializeField] private List<WeaponInfo> allWeapons;
    [SerializeField] private List<PassiveItemInfo> allPassives;

    public ChoiceRarity GetRandomRarity(PlayerStats playerStats, bool withSpecialItems = false)
    {
        List<ChoiceRarity> validRarities = (withSpecialItems) ? choiceRarities.FindAll(r => r.SpecialItems.Count > 0) : choiceRarities;
        if (validRarities.Count == 0) return null;

        float luck = 1f + (playerStats.GetStatModifier(StatType.Luck).value / 100f);

        float luckStrength = Mathf.Log(luck) * 1.5f;
        float adjustValue = 1.5f;

        int raritiesCount = validRarities.Count;

        List<float> adjustedWeights = new();
        float totalWeight = 0f;

        for (int i = 0; i < raritiesCount; i++)
        {
            float exponent = ((raritiesCount - 1) - i) * luckStrength;
            float adjusted = validRarities[i].Weight * Mathf.Pow(adjustValue, -exponent);

            adjustedWeights.Add(adjusted);
            totalWeight += adjusted;
        }

        float randomPoint = UnityEngine.Random.Range(0, totalWeight);
        float current = 0f;
        for (int i = 0; i < raritiesCount; i++)
        {
            current += adjustedWeights[i];
            if (randomPoint <= current)
                return validRarities[i];
        }

        return validRarities[0];
    }

    public List<WeaponInfo> GetAllWeapons() => allWeapons;
    public List<PassiveItemInfo> GetAllPassives() => allPassives;
}

[Serializable]
public class ChoiceRarity : IWeighted
{
    public string Name;
    public Color Color;
    [field: SerializeField] public float Weight { get; set; }
    public float Multiplier;
    public List<SpecialItemInfo> SpecialItems;
}