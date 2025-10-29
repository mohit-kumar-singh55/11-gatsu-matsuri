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
        }
    }
}
