using UnityEngine;

public class DropItemOnDestroy : MonoBehaviour
{
    public GameObject item;

    private static bool _quitting = false;

    private void OnApplicationQuit()
    {
        _quitting = true;
    }

    private void OnDestroy()
    {
        if (!_quitting && gameObject.scene.isLoaded)
            Instantiate(item, transform.position, Quaternion.identity);
    }
}
