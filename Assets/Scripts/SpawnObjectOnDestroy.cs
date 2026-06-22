using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectOnDestroy : MonoBehaviour
{
    public GameObject obj;
    public GameObject chestPrefab;

    [Range(0f, 1f)]
    public float powerupChance = 0.1f;
    public List<GameObject> powerUps;


    private static bool _quitting = false;

    private void OnApplicationQuit()
    {
        _quitting = true;
    }

    private void OnDestroy()
    {
        if (!_quitting && gameObject.scene.isLoaded)
        {
            Instantiate(obj, transform.position, Quaternion.identity);
            if(chestPrefab) Instantiate(chestPrefab, transform.position, Quaternion.identity);

            if (powerUps.Count > 0 && Random.value < powerupChance)
                Instantiate(powerUps[Random.Range(0, powerUps.Count - 1)], transform.position, Quaternion.identity);
        }
    }
}
