using UnityEngine;
using UnityEngine.InputSystem;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference toggleDebugMenuAction;

    public bool menuEnabled = false;

    private PlayerReferences _player;

    private bool _invincible;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerReferences>();
        toggleDebugMenuAction.action.performed += _ => ToggleDebugMenu();
    }

    private void OnGUI()
    {
        if (!menuEnabled)
            return;

        GUILayout.BeginArea(new Rect(0.0f, 300.0f, 300.0f, 400.0f));
        GUILayout.BeginVertical("box");
        _invincible = GUILayout.Toggle(_invincible, "Invincible");
        _player.Health.invincible = _invincible;
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void ToggleDebugMenu()
    {
        menuEnabled = !menuEnabled;
    }
}
