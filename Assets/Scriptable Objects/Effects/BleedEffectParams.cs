using UnityEngine;

[CreateAssetMenu(fileName = "BleedEffectParams", menuName = "Scriptable Objects/BleedEffectParams")]
public class BleedEffectParams : EffectParams
{
    [field: SerializeField] public float DurationSeconds { get; private set; }
    [field: SerializeField] public int HealthReductionPerSecond { get; private set; }

    public override void Apply(GameObject target)
    {
        var effect = target.AddComponent<BleedEffect>();
        effect.durationLeftSeconds = DurationSeconds;
        effect.healthReductionPerSecond = HealthReductionPerSecond;
    }
}
