using System.Collections.Generic;
using UnityEngine;

public class PlayerGathering : MonoBehaviour
{
    public PlayerStats stats;
    [Header("Settings")]
    public float checkRadius = 0.25f;
    public float checkDistance = 1f;
    public int maxFound = 4;
    public LayerMask resourceMask;

    Collider[] checkResults;
    List<ResourceObject> resources;

    private void Awake()
    {
        checkResults = new Collider[maxFound];
        resources = new List<ResourceObject>();
    }

    public bool CheckForResources()
    {
        resources.Clear();

        int amount = Physics.OverlapSphereNonAlloc(transform.position + transform.forward * checkDistance, checkRadius, checkResults, resourceMask);
        for (int i = 0; i < amount; i++)
        {
            ResourceObject result = checkResults[i].GetComponent<ResourceObject>();
            if (result != null && result.data)
                resources.Add(result);
        }

        return resources.Count > 0;
    }

    void Gather()
    {
        if (stats == null || resources.Count <= 0) return;

        foreach (ResourceObject res in resources)
        {
            ResourceData data = res.data;

            int amount = res.Damage(stats.resourceDamage);
            if (data.isSpecial)
                SaveManager.instance.saveData.AddResource(data, amount);
            else
                stats.resourceGathered += amount;
        }
    }
}
