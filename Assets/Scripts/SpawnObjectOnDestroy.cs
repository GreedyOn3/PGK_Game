using UnityEngine;

public class SpawnObjectOnDestroy : MonoBehaviour
{
    public GameObject obj;

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
        }
    }
}
