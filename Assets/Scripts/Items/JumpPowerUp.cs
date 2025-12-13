using UnityEngine;

public class JumpPowerUp : MonoBehaviour, IInteractable
{
    #region Serialize Fields
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
    #endregion

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        // UI更新
        UIManager.Instance.ShowJumpPowerUI(true);
        UIManager.Instance.ShowNotification(notificationText, notificationDuration);

        // プレイヤーのジャンプ力をアップ
        player.ChangeJumpForceForSomeTime(multipleOfJumpForce, jumpForceUpDuration);

        // エフェクト
        if (effectPrefab) Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}