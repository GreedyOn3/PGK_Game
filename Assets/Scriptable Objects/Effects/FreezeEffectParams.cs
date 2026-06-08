using UnityEngine;

[CreateAssetMenu(fileName = "FreezeEffectParams", menuName = "Scriptable Objects/FreezeEffectParams")]
public class FreezeEffectParams : EffectParams
{
    [field: SerializeField] public float DurationSeconds { get; private set; }
    [field: SerializeField] public Material FreezeMaterial { get; private set; }

    public override void Apply(GameObject target)
    {
        var effect = target.GetComponent<FreezeEffect>();

        if (effect != null)
        {
            effect.durationLeftSeconds += DurationSeconds;
            return;
        }

        effect = target.AddComponent<FreezeEffect>();
        effect.durationLeftSeconds = DurationSeconds;
        effect.freezeMaterial = FreezeMaterial;
    }
}
