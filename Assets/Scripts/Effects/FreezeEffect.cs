using UnityEngine;

public class FreezeEffect : Effect
{
    public Material freezeMaterial;
    private EnemyMovement _movement;
    private Renderer[] _meshRenderers;
    private Material[] _previousMaterials;

    private float _prevSpeed;

    private void Start()
    {
        _movement = GetComponent<EnemyMovement>();
        if (_movement != null)
        {
            _prevSpeed = _movement.GetMoveSpeed();
            _movement.SetMoveSpeed(_prevSpeed * 0.5f);
        }

        _meshRenderers = GetComponentsInChildren<Renderer>();

        if (freezeMaterial != null && _meshRenderers != null && _meshRenderers.Length > 0)
        {
            _previousMaterials = new Material[_meshRenderers.Length];
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                if (_meshRenderers[i].GetComponent<ParticleSystem>() != null)
                {
                    _meshRenderers[i] = null;
                    continue;
                }

                Renderer renderer = _meshRenderers[i];
                _previousMaterials[i] = renderer.sharedMaterial;
                renderer.sharedMaterial = freezeMaterial;
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        /*if (_movement != null)
            _movement.enabled = false;*/
    }

    private void OnDestroy()
    {
        if (_movement != null)
            _movement.SetMoveSpeed(_prevSpeed);

        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            if (_meshRenderers[i] == null) continue;

            Material previous = _previousMaterials[i];
            if (previous)
                _meshRenderers[i].sharedMaterial = previous;
        }
    }
}
