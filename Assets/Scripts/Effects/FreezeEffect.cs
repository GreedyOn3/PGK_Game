using UnityEngine;

public class FreezeEffect : Effect
{
    public Material freezeMaterial;
    private Movement _movement;
    private Renderer[] _meshRenderers;
    private Material[] _previousMaterials;

    private void Start()
    {
        _movement = GetComponent<Movement>();
        if (_movement != null)
            _movement.enabled = false;

        _meshRenderers = GetComponentsInChildren<Renderer>();

        if (freezeMaterial != null && _meshRenderers != null && _meshRenderers.Length > 0)
        {
            _previousMaterials = new Material[_meshRenderers.Length];
            for (int i = 0; i < _meshRenderers.Length; i++)
            {
                Renderer renderer = _meshRenderers[i];
                _previousMaterials[i] = renderer.sharedMaterial;
                renderer.sharedMaterial = freezeMaterial;
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_movement != null)
            _movement.enabled = false;
    }

    private void OnDestroy()
    {
        if (_movement != null)
            _movement.enabled = true;

        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            Material previous = _previousMaterials[i];
            if (previous)
                _meshRenderers[i].sharedMaterial = previous;
        }
    }
}
