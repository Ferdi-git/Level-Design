using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask ceilingLayer;

    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject playerHead;
    [SerializeField] private SSO_PlayerData data;

    private Vector2 rawInputVector;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    private Vector3 playerVelocity;
    
    [HideInInspector] public bool IsGrounded => controller.isGrounded;

    public float CurrentSpeed
    {  
        get 
        { 
            if(playerController.IsCrouching) return data.crouchSpeed;
            else if(playerController.IsSprinting) return data.sprintSpeed;
            return data.moveSpeed;
        }
    }


    private bool IsUnderCeiling()
    {
        float targetHeight = data.standHeight;

        Vector3 center = transform.position;
        float radius = controller.radius;

        Vector3 pointBottom = center + Vector3.down * (targetHeight / 2f - radius);

        Vector3 pointTop = center + Vector3.up * (targetHeight / 2f - radius);

        return Physics.CheckCapsule(pointBottom, pointTop, radius - 0.1f, ceilingLayer);
    }

    public void ProcessMove()
    {
        currentInputVector = Vector2.SmoothDamp(currentInputVector, rawInputVector, ref smoothInputVelocity, data.smoothInputSpeed);

        Vector3 moveDirection = new Vector3(currentInputVector.x, 0, currentInputVector.y);

        controller.Move(transform.TransformDirection(moveDirection) * CurrentSpeed * Time.deltaTime);

        playerVelocity.y += data.gravity * Time.deltaTime;

        if (IsGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Move(Vector2 value)
    {
        rawInputVector = value;
    }


    public void Jump()
    {
        if (IsGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(data.jumpHeight * -3f * data.gravity);
        }
    }

    public void Crouch()
    {
        if (IsGrounded)
        {
            Sequence standSeq = DOTween.Sequence();

            standSeq.Join(DOTween.To(() => controller.height, x => controller.height = x, data.crouchHeight, data.crouchTransitionSpeed));
            standSeq.Join(DOTween.To(() => controller.center, x => controller.center = x, new Vector3(0, -data.crouchHeight * 0.5f, 0), data.crouchTransitionSpeed));
            standSeq.Join(playerHead.transform.DOLocalMove(new Vector3(0, data.crouchCameraTargetHeight, 0), data.crouchTransitionSpeed));

            standSeq.SetEase(Ease.OutQuad);
        }
    }

    public void Stand()
    {
        if(!IsUnderCeiling())
        {
            Sequence standSeq = DOTween.Sequence();

            standSeq.Join(DOTween.To(() => controller.height, x => controller.height = x, data.standHeight, data.crouchTransitionSpeed));
            standSeq.Join(DOTween.To(() => controller.center, x => controller.center = x, Vector3.zero, data.crouchTransitionSpeed));
            standSeq.Join(playerHead.transform.DOLocalMove(new Vector3(0, data.standCameraTargetHeight, 0), data.crouchTransitionSpeed));

            standSeq.SetEase(Ease.OutQuad);
        }
    }
}