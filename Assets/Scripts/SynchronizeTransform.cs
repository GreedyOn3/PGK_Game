using UnityEngine;

public class SynchronizeTransform : MonoBehaviour
{
    public Transform reference;
    public Vector3 positionOffset;

    private void Update()
    {
        if (reference == null) return;

        transform.position = reference.position + positionOffset;
        transform.rotation = reference.rotation;
        transform.localScale = reference.localScale;
    }
}
