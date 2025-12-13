using UnityEngine;

public class TimeLimitExtension : MonoBehaviour, IInteractable
{
    #region Serialize Fields
    [Header("時間延長設定")]
    [Tooltip("時間制限の延長時間 (秒)")]
    [SerializeField] private float increaseTimeLimitBy = 20f;

    [Header("エフェクト設定")]
    [SerializeField] private GameObject effectPrefab;

    [Header("通知設定")]
    [Tooltip("通知テキスト")]
    [SerializeField] private string notificationText = "Time Extended!";
    [Tooltip("通知表示時間")]
    [SerializeField] private float notificationDuration = 2f;
    #endregion

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        // 時間制限を延長
        FindAnyObjectByType<CountdownTimer>().IncreaseTime(increaseTimeLimitBy);

        // UI更新
        UIManager.Instance.ShowNotification(notificationText, notificationDuration);

        // エフェクト
        if (effectPrefab) Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}