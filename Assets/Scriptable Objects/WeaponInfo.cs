using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "Scriptable Objects/Weapon Info")]
public class WeaponInfo : BaseItemInfo
{
    [field: SerializeField] public Weapon Prefab { get; private set; }
    [SerializeField] private List<WeaponStatEntry> Stats = new List<WeaponStatEntry>();

    public List<StatInfo> BaseStats => Stats.Select(x => x.Stat).ToList();
    public List<StatInfo> UpgradePool =>
        Stats.Where(x => x.IsUpgradable)
        .Select(x => new StatInfo { Type = x.Stat.Type, Value = x.UpgradeIncrement, IsPercentage = x.IsPercentage })
        .ToList();

    private void Reset()
    {
        EnsureRequiredStats();
    }

    private void OnValidate()
    {
        if (Stats != null)
            EnsureRequiredStats();
    }

    private void EnsureRequiredStats()
    {
        AddStatIfMissing(StatType.Attack, 5f);
        AddStatIfMissing(StatType.Cooldown, 1f);
    }

    private void AddStatIfMissing(StatType type, float defaultVal)
    {
        int index = Stats.FindIndex(x => x.Stat.Type == type);

        if (index == -1)
        {
            Stats.Add(new WeaponStatEntry
            {
                Stat = new StatInfo { Type = type, Value = defaultVal },
                IsUpgradable = false
            });
        }
    }
}

[Serializable]
public struct WeaponStatEntry
{
    public StatInfo Stat;
    //public StatInfo Upgrade;

    // UPGRADE INFO
    public float UpgradeIncrement;
    public bool IsPercentage;
    public bool IsUpgradable;
}

[CustomPropertyDrawer(typeof(WeaponStatEntry))]
public class WeaponStatEntryDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty statProp = property.FindPropertyRelative("Stat");
        //SerializedProperty upgradeProp = property.FindPropertyRelative("Upgrade");
        SerializedProperty isUpgradableProp = property.FindPropertyRelative("IsUpgradable");

        SerializedProperty incProp = property.FindPropertyRelative("UpgradeIncrement");
        SerializedProperty isPercProp = property.FindPropertyRelative("IsPercentage");

        float spacing = 5f;
        float toggleWidth = 18f;

        float usableWidth = position.width - (spacing * 3) - (toggleWidth * 2);
        float statWidth = usableWidth * 0.6f;
        float incWidth = usableWidth * 0.4f;

        Rect statRect = new Rect(position.x, position.y, statWidth, position.height);
        Rect incRect = new Rect(statRect.xMax + spacing, position.y, incWidth, position.height);
        Rect percRect = new Rect(incRect.xMax + spacing, position.y, toggleWidth, position.height);
        Rect upgrRect = new Rect(percRect.xMax + spacing, position.y, toggleWidth, position.height);

        EditorGUI.PropertyField(statRect, statProp, GUIContent.none);

        if (isUpgradableProp.boolValue)
        {
            EditorGUI.PropertyField(incRect, incProp, GUIContent.none);

            isPercProp.boolValue = EditorGUI.Toggle(percRect, isPercProp.boolValue);
            GUI.Label(percRect, new GUIContent("%", "Is percentage?"), EditorStyles.miniLabel);
        }
        else
        {
            EditorGUI.LabelField(incRect, "Not Upgradable", EditorStyles.centeredGreyMiniLabel);
        }

        EditorGUI.PropertyField(upgrRect, isUpgradableProp, GUIContent.none);

        EditorGUI.EndProperty();
    }
}
