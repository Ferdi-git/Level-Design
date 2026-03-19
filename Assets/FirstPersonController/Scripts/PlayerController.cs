using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private SSO_PlayerData data;

    // STATES
    [HideInInspector] public bool IsCrouching;
    [HideInInspector] public bool IsSprinting;
    [HideInInspector] public bool IsJumping;
    [HideInInspector] public bool IsWalking;
    [HideInInspector] public bool IsGrounded;
    private bool IsStanding;
    private bool IsAiming;

    private void FixedUpdate()
    {
        playerMovement.ProcessMove();

        if (!IsCrouching && !IsStanding)
        {
            Stand();
            IsStanding = true;
        }

        if(playerMovement.IsGrounded && IsJumping)
        {
            IsJumping = false;
            cameraController.OnLand();
        }
    }

    private void LateUpdate()
    {
        cameraController.HandleLook();
        
    }

    private void OnEnable()
    {
        inputManager.OnLook += cameraController.Look;

        inputManager.OnMove += OnMove;

        inputManager.OnJump += OnJump;

        inputManager.OnSprint += OnSprint;

        inputManager.OnCrouch += OnCrouch;
    }

    private void OnDisable()
    {
        inputManager.OnLook -= cameraController.Look;

        inputManager.OnMove -= OnMove;

        inputManager.OnJump -= OnJump;

        inputManager.OnSprint -= OnSprint;

        inputManager.OnCrouch -= OnCrouch;
    }

    private void OnMove(Vector2 value)
    {
        playerMovement.Move(value);

        if (value.magnitude >= 0.5f)
        {
            IsWalking = true;

            if (IsCrouching) {OnCrouch(true); return; }
            else if (IsSprinting) {OnSprint(true); return; }
            else if (IsJumping) return;

            cameraController.WalkShake();

            if (IsAiming) return;
        }
        else
        {
            IsWalking = false;

            if (IsCrouching) return;
            cameraController.StopShake();

            if (IsAiming) return;
        }
    }

    private void OnJump(bool isJumping)
    {
        IsJumping = true;
        if (!IsCrouching) playerMovement.Jump();

        if (IsJumping)
        {
            if (IsAiming) return;
            cameraController.StopShake();
        }
    }

    private void OnCrouch(bool isCrouching)
    {
        IsCrouching = isCrouching;
        IsStanding = false;

        if (IsJumping) return;
        playerMovement.Crouch();

        if (!IsWalking) return;
        cameraController.CrouchShake();
    }

    private void Stand()
    {
        playerMovement.Stand();
        cameraController.OnLand();
    }
    
    private void OnSprint(bool isSprinting)
    {
        IsSprinting = isSprinting;

        if (IsJumping || IsCrouching || !IsWalking) return;

        if (isSprinting)
        {
            cameraController.SprintShake();
        }
        else
        {
            cameraController.OnLand();
        }
    }
}