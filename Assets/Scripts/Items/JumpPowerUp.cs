using UnityEngine;

public class JumpPowerUp : MonoBehaviour, IInteractable
{
    [Header("ジャンプ力アップ設定")]
    [Tooltip("ジャンプ力の倍率")]
    [SerializeField] private float multipleOfJumpForce = 1.3f;
    [Tooltip("ジャンプ力アップの持続時間")]
    [SerializeField] private float jumpForceUpDuration = 10f;

    [Header("エフェクト設定")]
    [SerializeField] private GameObject effectPrefab;

    [Header("通知設定")]
    [Tooltip("通知テキスト")]
    [SerializeField] private string notificationText = "Jump Power Up!";
    [Tooltip("通知表示時間")]
    [SerializeField] private float notificationDuration = 2f;


    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        UIManager.Instance.ShowJumpPowerUI(true);
        UIManager.Instance.ShowNotification(notificationText, notificationDuration);

        player.ChangeJumpForceForSomeTime(multipleOfJumpForce, jumpForceUpDuration);

        if (effectPrefab) Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
