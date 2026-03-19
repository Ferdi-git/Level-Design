using UnityEngine;

[CreateAssetMenu(fileName = "SSO_PlayerData", menuName = "Scriptable Objects/SSO_PlayerData")]
public class SSO_PlayerData : ScriptableObject
{
    [Header("=== PLAYER SETTINGS ===")]
    [Header("Camera movement")]
    public float sensX = 120f;
    public float sensY = 120f;
    public float smoothTime = 0.05f;
    public float maxViewUp = -80f;
    public float maxViewDown = 80f;

    [Header("Player movement")]
    public float moveSpeed = 6.0f;
    public float sprintSpeed = 10.0f;
    public float crouchSpeed = 3.0f;
    public float smoothInputSpeed = 0.2f;

    [Header("Jump & Gravity")]
    public float gravity = -16.0f;
    public float jumpHeight = 1.0f;

    [Header("Crouch")]
    public float crouchHeight = 1;
    public float standHeight = 2;
    public float crouchCameraTargetHeight = -0.2f;
    public float standCameraTargetHeight = 0.8f;
    public float crouchTransitionSpeed = 0.2f;

    [Header("Detection")]
    public float detectionRadius = 5f;
    public LayerMask enemyLayer;
}