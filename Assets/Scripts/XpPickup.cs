using System;
using UnityEngine;

[RequireComponent(typeof(Xp))]
public class XpPickup : MonoBehaviour
{
    private Xp _xp;

    private void Start()
    {
        _xp = GetComponent<Xp>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var xp = other.GetComponent<Xp>();
            if (xp == null) return;
            xp.Add(_xp.value);
            Destroy(gameObject);
        }
    }
}
