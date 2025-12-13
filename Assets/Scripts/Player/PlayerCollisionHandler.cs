using UnityEngine;

/// <summary>
/// プレイヤーの衝突処理
/// </summary>
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
            if (SFXManager.Instance) SFXManager.Instance.PlayItemPickup();
        }
    }

    // ** プレイヤーがプラットフォームと一緒に動くようにするためのもの **
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
