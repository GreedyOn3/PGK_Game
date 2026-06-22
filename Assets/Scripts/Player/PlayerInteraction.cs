using UnityEngine;

[RequireComponent(typeof(PlayerReferences))]
public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private LayerMask interactableMask;

    private InteractableBase _hitInteractable;
    private PlayerReferences _playerReferences;
    private Camera _playerCamera;

    private void Start()
    {
        _playerReferences = GetComponent<PlayerReferences>();
        _playerCamera = _playerReferences.Camera.playerCamera.GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = _playerCamera.ScreenPointToRay(screenCenter);

        float distance = _playerReferences.Camera.distance + interactionDistance;

        if (Physics.Raycast(ray, out RaycastHit hit, distance, interactableMask))
        {
            if (_hitInteractable != null) return;

            InteractableBase interactable = hit.transform.GetComponent<InteractableBase>();
            if (interactable && interactable.InteractionEnabled)
            {
                _hitInteractable = interactable;
                _hitInteractable.OnHoverBegin();
            }
        } 
        else if(_hitInteractable != null)
        {
            _hitInteractable.OnHoverEnd();
            _hitInteractable = null;
        }
    }

    void OnInteract()
    {
        if (_hitInteractable != null && _hitInteractable.InteractionEnabled)
            _hitInteractable.OnInteract(_playerReferences);
    }
}
