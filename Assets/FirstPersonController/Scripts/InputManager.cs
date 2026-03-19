using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    private PlayerInputSystem inputs;

    public event Action<Vector2> OnLook;
    public event Action<Vector2>OnMove;
    public event Action OnInteract;
    public event Action<bool> OnAim;
    public event Action<bool> OnSprint;
    public event Action <bool>OnJump;
    public event Action OnShoot;
    public event Action<bool> OnCrouch;
    public event Action OnDrawProjection;
    public event Action OnThrow;

    private void Awake()
    {
        inputs = new PlayerInputSystem();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        inputs.Player.Enable();

        inputs.Player.Look.performed += OnLookInput;
        inputs.Player.Look.canceled += OnLookInput;

        inputs.Player.Move.performed += OnMoveInput;
        inputs.Player.Move.canceled += OnMoveInput;
        
        inputs.Player.Interact.performed += OnInteractInput;

        inputs.Player.Aim.performed += OnAimInput;
        inputs.Player.Aim.canceled += OnAimInput;

        inputs.Player.Sprint.performed += OnSprintInput;
        inputs.Player.Sprint.canceled += OnSprintInput;

        inputs.Player.Jump.performed += OnJumpInput;
        inputs.Player.Jump.canceled += OnJumpInput;

        inputs.Player.Shoot.performed += OnShootInput;

        inputs.Player.Crouch.performed += OnCrouchInput;
        inputs.Player.Crouch.canceled += OnCrouchInput;

        inputs.Player.Throw.performed += OnThrowInput;
        inputs.Player.Throw.canceled += OnThrowInput;

    }

    private void OnDisable()
    {
        inputs.Player.Look.performed -= OnLookInput;
        inputs.Player.Look.canceled -= OnLookInput;

        inputs.Player.Move.performed -= OnMoveInput;
        inputs.Player.Move.canceled -= OnMoveInput;
        
        inputs.Player.Interact.performed -= OnInteractInput;

        inputs.Player.Aim.performed -= OnAimInput;
        inputs.Player.Aim.canceled -= OnAimInput;

        inputs.Player.Sprint.performed -= OnSprintInput;
        inputs.Player.Sprint.canceled -= OnSprintInput;

        inputs.Player.Jump.performed -= OnJumpInput;
        inputs.Player.Jump.canceled -= OnJumpInput;

        inputs.Player.Shoot.performed -= OnShootInput;

        inputs.Player.Crouch.performed -= OnCrouchInput;
        inputs.Player.Crouch.canceled -= OnCrouchInput;

        inputs.Player.Throw.performed -= OnThrowInput;
        inputs.Player.Throw.canceled -= OnThrowInput;

        inputs.Player.Disable();
    }

    private void OnLookInput(InputAction.CallbackContext obj)
    {
        OnLook?.Invoke(obj.ReadValue<Vector2>());
    }

    private void OnMoveInput(InputAction.CallbackContext obj)
    {
        OnMove?.Invoke(obj.ReadValue<Vector2>());
    }

    private void OnInteractInput(InputAction.CallbackContext obj)
    {
        if (obj.ReadValueAsButton())
        {
            OnInteract?.Invoke();
        }
    }

    private void OnAimInput(InputAction.CallbackContext obj)
    {
        if (obj.ReadValueAsButton())
        {
            OnAim?.Invoke(true);
        }
        else
        {
            OnAim?.Invoke(false);
        }
    }

    private void OnSprintInput(InputAction.CallbackContext obj)
    {
        if (obj.ReadValueAsButton())
        {
            OnSprint?.Invoke(true);
        }
        else
        {
            OnSprint?.Invoke(false);
        }
    }

    private void OnJumpInput(InputAction.CallbackContext obj)
    {
        if (obj.ReadValueAsButton())
        {
            OnJump?.Invoke(true);
        }
        else
        {
            OnJump?.Invoke(false);
        }
    }

    private void OnShootInput(InputAction.CallbackContext obj)
    {
        if (obj.ReadValueAsButton())
        {
            OnShoot?.Invoke();
        }
    }

    private void OnCrouchInput(InputAction.CallbackContext obj)
    {
        if (obj.ReadValueAsButton())
        {
            OnCrouch?.Invoke(true);
        }
        else
        {
            OnCrouch?.Invoke(false);
        }
    }

    private void OnThrowInput(InputAction.CallbackContext obj)
    {
        if (obj.ReadValueAsButton())
        {
            OnDrawProjection?.Invoke();
        }
        else
        {
            OnThrow?.Invoke();
        }
    }
}