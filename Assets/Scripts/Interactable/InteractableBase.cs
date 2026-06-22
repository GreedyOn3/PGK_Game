using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    public bool InteractionEnabled
    {
        get => _interactionEnabled;

        set
        {
            _interactionEnabled = value;
            OnHoverEnd();
        }
    }

    private bool _interactionEnabled = true;

    public virtual void OnHoverBegin() => ChangeLayer("HoveredInteractable");
    public virtual void OnHoverEnd() => ChangeLayer("Interactable");

    public abstract void OnInteract(PlayerReferences player);

    private void ChangeLayer(string layerName)
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}
