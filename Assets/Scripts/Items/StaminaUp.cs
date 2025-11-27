using UnityEngine;

public class StaminaUp : MonoBehaviour, IInteractable
{
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

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        player.FreezeStamina = true;

        UIManager.Instance.ShowStaminaFreezeUI(true);
        UIManager.Instance.ShowNotification(notificationText, notificationDuration);

        player.RestartStaminaDepletionAfterDelay(staminaFreezeDuration);

        if (effectPrefab) Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
