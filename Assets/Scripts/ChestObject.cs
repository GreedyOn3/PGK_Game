using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChestObject : MonoBehaviour
{
    public static event Action<PlayerReferences, ChestObject> OnChestOpened;
    private bool _triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;

        if (other.gameObject.TryGetComponent(out PlayerReferences references))
        {
            _triggered = true;
            OnChestOpened?.Invoke(references, this);
        }
    }
}
