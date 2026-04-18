using UnityEngine;

[CreateAssetMenu(fileName = "FreezeEffectParams", menuName = "Scriptable Objects/FreezeEffectParams")]
public class FreezeEffectParams : EffectParams
{
    [field: SerializeField] public float DurationSeconds { get; private set; }

    public override void Apply(GameObject target)
    {
        var effect = target.AddComponent<FreezeEffect>();
        effect.durationLeftSeconds = DurationSeconds;
    }
}
