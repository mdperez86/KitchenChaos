using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum KeyBinding
    {
        Move_Up, Move_Down, Move_Left, Move_Right, Interact, InteractAlternate, Pause, Gamepad_Interact, Gamepad_InteractAlternate, Gamepad_Pause
    }

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;

        Instance = this;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;
        playerInputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    // Start is called before the first frame update
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector.normalized;
    }

    public string GetKeyBindingText(KeyBinding keyBinding)
    {
        return keyBinding switch
        {
            KeyBinding.Move_Up => playerInputActions.Player.Move.bindings[1].ToDisplayString(),
            KeyBinding.Move_Down => playerInputActions.Player.Move.bindings[2].ToDisplayString(),
            KeyBinding.Move_Left => playerInputActions.Player.Move.bindings[3].ToDisplayString(),
            KeyBinding.Move_Right => playerInputActions.Player.Move.bindings[4].ToDisplayString(),
            KeyBinding.Interact => playerInputActions.Player.Interact.bindings[0].ToDisplayString(),
            KeyBinding.InteractAlternate => playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString(),
            KeyBinding.Pause => playerInputActions.Player.Pause.bindings[0].ToDisplayString(),
            KeyBinding.Gamepad_Interact => playerInputActions.Player.Interact.bindings[1].ToDisplayString(),
            KeyBinding.Gamepad_InteractAlternate => playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString(),
            KeyBinding.Gamepad_Pause => playerInputActions.Player.Pause.bindings[1].ToDisplayString(),
            _ => null,
        };
    }

    public void RebindKey(KeyBinding keyBinding, Action onActionRebound)
    {
        playerInputActions.Player.Disable();

        InputActionRebindingExtensions.RebindingOperation rebindingOperation = null;

        switch (keyBinding) { 
            case KeyBinding.Move_Up: rebindingOperation = playerInputActions.Player.Move.PerformInteractiveRebinding(1); break;
            case KeyBinding.Move_Down: rebindingOperation = playerInputActions.Player.Move.PerformInteractiveRebinding(2); break;
            case KeyBinding.Move_Left: rebindingOperation = playerInputActions.Player.Move.PerformInteractiveRebinding(3); break;
            case KeyBinding.Move_Right: rebindingOperation = playerInputActions.Player.Move.PerformInteractiveRebinding(4); break;
            case KeyBinding.Interact: rebindingOperation = playerInputActions.Player.Interact.PerformInteractiveRebinding(0); break;
            case KeyBinding.InteractAlternate: rebindingOperation = playerInputActions.Player.InteractAlternate.PerformInteractiveRebinding(0); break;
            case KeyBinding.Pause: rebindingOperation = playerInputActions.Player.Pause.PerformInteractiveRebinding(0); break;
            case KeyBinding.Gamepad_Interact: rebindingOperation = playerInputActions.Player.Interact.PerformInteractiveRebinding(1); break;
            case KeyBinding.Gamepad_InteractAlternate: rebindingOperation = playerInputActions.Player.InteractAlternate.PerformInteractiveRebinding(1); break;
            case KeyBinding.Gamepad_Pause: rebindingOperation = playerInputActions.Player.Pause.PerformInteractiveRebinding(1); break;
        }

        rebindingOperation?
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputActions.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }
}
