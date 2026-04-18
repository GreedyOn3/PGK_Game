using UnityEngine;

[CreateAssetMenu(fileName = "EffectParams", menuName = "Scriptable Objects/EffectParams")]
public abstract class EffectParams : ScriptableObject
{
    public abstract void Apply(GameObject target);
}
