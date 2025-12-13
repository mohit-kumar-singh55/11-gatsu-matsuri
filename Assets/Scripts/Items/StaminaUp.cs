using UnityEngine;

public class StaminaUp : MonoBehaviour, IInteractable
{
    #region Serialize Fields
    [Header("スタミナフリーズ設定")]
    [Tooltip("スタミナの持続時間")]
    [SerializeField] private float staminaFreezeDuration = 10f;

    [Header("エフェクト設定")]
    [SerializeField] private GameObject effectPrefab;

    [Header("通知設定")]
    [Tooltip("通知テキスト")]
    [SerializeField] private string notificationText = "Stamina Freeze!";
    [Tooltip("通知表示時間")]
    [SerializeField] private float notificationDuration = 2f;
    #endregion

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        // スタミナフリーズ
        player.FreezeStamina = true;

        // UI更新
        UIManager.Instance.ShowStaminaFreezeUI(true);
        UIManager.Instance.ShowNotification(notificationText, notificationDuration);

        // プレイヤーのスタミナを復活
        player.RestartStaminaDepletionAfterDelay(staminaFreezeDuration);

        // エフェクト
        if (effectPrefab) Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}