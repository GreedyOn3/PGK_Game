using UnityEngine;

public class SynchronizeTransform : MonoBehaviour
{
    public Transform reference;

    private void Update()
    {
        if (reference == null) return;

        transform.position = reference.position;
        transform.rotation = reference.rotation;
        transform.localScale = reference.localScale;
    }
}
