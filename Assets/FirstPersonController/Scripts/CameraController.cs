using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController controller;
    [SerializeField] private Transform orientation;
    [SerializeField] private SSO_PlayerData data;

    [Header("Intensity / Speed")]
    public float walkAmplitude = 0.5f;
    public float walkFrequency = 2f;

    public float sprintAmplitude = 0.25f;
    public float sprintFrequency = 3f;

    public float crouchAmplitude = 0.1f;
    public float crouchFrequency = 1f;

    private float xRotation;
    private float yRotation;

    private Vector3 initialLocalPos;

    private Vector2 currentLook;
    private Vector2 lookVelocity;

    private Vector2 lookInput;

    void Start()
    {
        initialLocalPos = transform.localPosition;
    }

    public void OnLand()
    {
        if (controller.IsWalking)
        {
            if (controller.IsCrouching)
                CrouchShake();
            else if (controller.IsSprinting)
                SprintShake();
            else
                WalkShake();
        }
        else StopShake();
    }

    public void Look(Vector2 value)
    {
        lookInput = value;
    }

    public void HandleLook()
    {
        currentLook = Vector2.SmoothDamp(currentLook, lookInput, ref lookVelocity, data.smoothTime);

        float mouseX = currentLook.x * data.sensX * Time.deltaTime;
        float mouseY = currentLook.y * data.sensY * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, data.maxViewUp, data.maxViewDown);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

    public void KillTween()
    {
        DOTween.Kill(transform);

    }

    public void StopShake()
    {
        StartCoroutine(StopShakeCoroutine());
    }

    public void WalkShake()
    {
        KillTween();
        transform.DOLocalMove(initialLocalPos, 0.1f);
        StartCoroutine(PosCoroutine(walkAmplitude, walkFrequency));
    }

    public void SprintShake()
    {
        KillTween();
        transform.DOLocalMove(initialLocalPos, 0.1f);
        StartCoroutine(PosCoroutine(sprintAmplitude, sprintFrequency));
    }

    public void CrouchShake()
    {
        KillTween();
        StartCoroutine(PosCoroutine(crouchAmplitude, crouchFrequency));
    }

    private void StartHeadBob(float amplitude, float frequency)
    {
        transform.DOShakePosition(
            duration: 1f / frequency,
            strength: new Vector3(0f, amplitude, 0f),
            vibrato: 1,
            randomness: 0,
            fadeOut: false
        )
        .SetLoops(-1, LoopType.Restart)
        .SetEase(Ease.Linear);
    }

    private IEnumerator PosCoroutine(float amplitude, float frequency)
    {
        yield return new WaitForSeconds(0.15f);
        StartHeadBob(amplitude, frequency);
    }

    private IEnumerator StopShakeCoroutine()
    {
        yield return new WaitForSeconds(0.15f);
        KillTween();
        transform.DOLocalMove(initialLocalPos, 0.05f);
    }
}