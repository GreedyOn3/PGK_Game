using UnityEngine;
using UnityEngine.InputSystem;

public class DebugMenu : MonoBehaviour
{
    [SerializeField] private InputActionReference toggleDebugMenuAction;

    public bool menuEnabled = false;

    private PlayerReferences _player;

    private bool _invincible;
    private InputMode _savedInputMode;

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
        if (GUILayout.Button("Level Up"))
            LevelUpPlayer();
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private void ToggleDebugMenu()
    {
        if (menuEnabled)
            DisableDebugMenu();
        else
            EnableDebugMenu();
    }

    private void EnableDebugMenu()
    {
        menuEnabled = true;
        var inputManager = InputManager.Instance;
        _savedInputMode = inputManager.InputMode;
        inputManager.SwitchInputMode(InputMode.Ui);
    }

    private void DisableDebugMenu()
    {
        menuEnabled = false;
        InputManager.Instance.SwitchInputMode(_savedInputMode);
    }

    private void LevelUpPlayer()
    {
        _player.Xp.Add(_player.Xp.GainRequiredForNextLevel);
    }
}
