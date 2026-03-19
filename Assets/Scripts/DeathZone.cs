using DG.Tweening;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] Transform posRespawn;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>()?.DOKill();
            other.GetComponent<PlayerController>()?.DOKill();

            // Disable CC before moving, re-enable after
            CharacterController cc = other.GetComponent<CharacterController>();
            cc.enabled = false;
            other.transform.position = posRespawn.position;
            cc.enabled = true;
        }
    }
}