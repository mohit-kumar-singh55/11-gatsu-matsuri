using UnityEngine;

public class JumpPowerUp : MonoBehaviour, IInteractable
{
    [Tooltip("ジャンプ力の倍率")]
    [SerializeField] private float multipleOfJumpForce = 1.3f;
    [Tooltip("ジャンプ力アップの持続時間")]
    [SerializeField] private float jumpForceUpDuration = 10f;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        player.ChangeJumpForceForSomeTime(multipleOfJumpForce, jumpForceUpDuration);

        Destroy(gameObject);
    }
}
