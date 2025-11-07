using UnityEngine;

public class JumpPowerUp : MonoBehaviour, IInteractable
{
    [Tooltip("ジャンプ力の倍率")]
    [SerializeField] private float multipleOfJumpForce = 1.3f;
    [Tooltip("ジャンプ力アップの持続時間")]
    [SerializeField] private float jumpForceUpDuration = 10f;
    [SerializeField] private GameObject effectPrefab;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        UIManager.Instance.ShowJumpPowerUI(true);

        player.ChangeJumpForceForSomeTime(multipleOfJumpForce, jumpForceUpDuration);

        if (effectPrefab) Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
