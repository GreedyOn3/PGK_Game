using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/Character Stats")]
public class CharacterStats : ScriptableObject
{
    /*[field: SerializeField] public int MovementSpeedModifier { get; private set; }
    [field: SerializeField] public int AttackModifier { get; private set; }
    [field: SerializeField] public int DefenseModifier { get; private set; }
    [field: SerializeField] public int PickupRangeModifier { get; private set; }*/
    public List<StatInfo> Stats = new List<StatInfo>();

    private void OnValidate()
    {
        List<StatInfo> syncedList = new();

        foreach (StatType stat in Enum.GetValues(typeof(StatType)))
        {
            int existingIndex = Stats.FindIndex(s => s.Type == stat);

            FieldInfo fieldInfo = typeof(StatType).GetField(stat.ToString());
            StatInfo statInfo = (existingIndex >= 0) ? Stats[existingIndex] : new StatInfo { Type = stat, BaseValue = 0f };
            statInfo.isPercentage = !Attribute.IsDefined(fieldInfo, typeof(FlatStatAttribute));

            syncedList.Add(statInfo);
        }

        Stats = syncedList;
    }
}

[CustomEditor(typeof(CharacterStats))]
public class CharacterStatsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPropertiesExcluding(serializedObject, new string[] { "Stats", "m_Script" });

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Stats", EditorStyles.boldLabel);

        SerializedProperty statsList = serializedObject.FindProperty("Stats");

        EditorGUILayout.BeginVertical(GUI.skin.box);
        for (int i = 0; i < statsList.arraySize; i++)
        {
            SerializedProperty element = statsList.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(element, GUIContent.none);
        }
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
