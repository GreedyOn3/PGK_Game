using UnityEngine;

public class FreezeEffect : Effect
{
    public Material freezeMaterial;
    private Movement _movement;
    private MeshRenderer _mesh;
    private Material _previousMaterial;

    private void Start()
    {
        _movement = GetComponent<Movement>();
        if (_movement != null)
            _movement.enabled = false;

        _mesh = GetComponent<MeshRenderer>();

        if (freezeMaterial != null && _mesh != null)
        {
            Debug.Log("TEST");
            _previousMaterial = _mesh.material;
            _mesh.material = freezeMaterial;
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

        if (_previousMaterial != null)
            _mesh.material = _previousMaterial;
    }
}
