using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class FlatStatAttribute : Attribute { }
public enum StatType
{
    [FlatStat] MaxHealth,
    Attack, Cooldown, MoveSpeed, PickupRange, Defense
}

[Serializable]
public class Stat
{
    public float baseValue;
    private readonly List<StatModifier> modifiers = new List<StatModifier>();

    public Stat(float baseValue)
    {
        this.baseValue = baseValue;
    }

    public void AddModifier(StatModifier mod) => modifiers.Add(mod);
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
    public float BaseValue;
    public bool isPercentage;
}

[CustomPropertyDrawer(typeof(StatInfo))]
public class StatInfoDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        float gap = 5f;
        float percentWidth = 30f;
        float halfWidth = (position.width / 2f) - gap;

        Rect typeRect = new Rect(position.x, position.y, halfWidth, position.height);
        Rect valueRect = new Rect(position.x + halfWidth + gap, position.y, halfWidth, position.height);
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

        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("BaseValue"), GUIContent.none);
        if (isCharacterStats && isPercentage)
        {
            GUIStyle suffixStyle = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleLeft };
            EditorGUI.LabelField(suffixRect, "%", suffixStyle);
        }

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
