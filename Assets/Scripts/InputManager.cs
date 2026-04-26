using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    [SerializeField] private InputMode initialInputMode = InputMode.Gameplay;
    public InputMode InputMode { get; private set; }

    public static InputManager Instance { get; private set; }

    private InputActionMap _gameplayActions;
    private InputActionMap _uiActions;
    private InputActionMap _alwaysActions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _gameplayActions = inputActions.FindActionMap("Gameplay");
        _uiActions = inputActions.FindActionMap("UI");
        _alwaysActions = inputActions.FindActionMap("Always");
        SwitchInputMode(initialInputMode);
    }

    public void SwitchInputMode(InputMode mode)
    {
        InputMode = mode;

        switch (InputMode)
        {
            case InputMode.Gameplay:
                _uiActions.Disable();
                _gameplayActions.Enable();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;
            case InputMode.Ui:
                _gameplayActions.Disable();
                _uiActions.Enable();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        _alwaysActions.Enable();
    }
}

public enum InputMode
{
    Gameplay,
    Ui,
}
