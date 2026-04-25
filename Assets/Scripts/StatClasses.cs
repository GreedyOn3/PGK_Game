using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class FlatStatAttribute : Attribute { }
[Obfuscation(Exclude = true)]
public enum StatType
{
    [FlatStat] MaxHealth,
    Attack, Cooldown, MoveSpeed, PickupRange, Defense
}

[Serializable]
public class Stat
{
    public float baseValue;
    public readonly List<StatModifier> modifiers = new List<StatModifier>();

    public Stat(float baseValue)
    {
        this.baseValue = baseValue;
    }

    public void AddModifier(StatModifier mod, bool asNew=false)
    {
        if (asNew)
        {
            modifiers.Add(mod);
            return;
        }

        StatModifier foundMod = modifiers.Find(x => x.isPercentage == mod.isPercentage);
        if (foundMod != null)
            foundMod.value += mod.value;
        else
            modifiers.Add(mod);
    }
    public void RemoveModifier(StatModifier mod) => modifiers.Remove(mod);

    public float GetValue()
    {
        float finalValue = baseValue;
        float percentageSum = 0;

        foreach (StatModifier mod in modifiers)
        {
            if (!mod.isPercentage)
                finalValue += mod.value;
            else
                percentageSum += mod.value;
        }

        return finalValue * (1 + (percentageSum / 100f));
    }

    public static string GetDisplayName(StatType type)
    {
        return type switch
        {
            StatType.MaxHealth => "Max Health",
            StatType.Attack => "Damage",
            StatType.Cooldown => "Attack Speed",
            StatType.MoveSpeed => "Move Speed",
            StatType.PickupRange => "Pickup Range",
            StatType.Defense => "Defense",
            _ => type.ToString()
        };
    }
}

public class StatModifier
{
    public float value;
    public bool isPercentage;

    public StatModifier(float value, bool isPercentage)
    {
        this.value = value;
        this.isPercentage = isPercentage;
    }
}

[Serializable]
public struct StatInfo
{
    public StatType Type;
    public float Value;
    public bool IsPercentage;
}

[CustomPropertyDrawer(typeof(StatInfo))]
public class StatInfoDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float spacing = 5f;
        float percentWidth = 30f;
        float halfWidth = (position.width / 2f) - spacing;

        Rect typeRect = new Rect(position.x, position.y, halfWidth, position.height);
        Rect valueRect = new Rect(position.x + halfWidth + spacing, position.y, halfWidth, position.height);
        Rect suffixRect = new Rect(position.x + position.width - percentWidth, position.y, percentWidth, position.height);

        SerializedProperty typeProp = property.FindPropertyRelative("Type");
        bool isCharacterStats = property.serializedObject.targetObject is CharacterStats;

        FieldInfo fieldInfo = typeof(StatType).GetField(typeProp.enumNames[typeProp.enumValueIndex]);
        bool isPercentage = !Attribute.IsDefined(fieldInfo, typeof(FlatStatAttribute));

        if (isCharacterStats)
        {
            string displayName = typeProp.enumDisplayNames[typeProp.enumValueIndex];
            EditorGUI.LabelField(typeRect, displayName);

            if (isPercentage) valueRect.width -= percentWidth;
        }
        else
        {
            EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);
        }

        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("Value"), GUIContent.none);
        if (isCharacterStats && isPercentage)
        {
            GUIStyle suffixStyle = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleLeft };
            EditorGUI.LabelField(suffixRect, "%", suffixStyle);
        }

        EditorGUI.EndProperty();
    }
}
