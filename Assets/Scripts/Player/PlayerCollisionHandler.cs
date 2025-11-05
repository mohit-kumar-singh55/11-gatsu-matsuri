using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        playerController = PlayerController.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactable.OnInteract(playerController);
            SFXManager.Instance.PlayItemPickup();
        }
    }

    // ** stuff to make player move with platform and not slide off **
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ObjectPropogator platform))
            playerController.CurrentPlatform = platform;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out ObjectPropogator platform))
            playerController.CurrentPlatform = null;
    }
}
