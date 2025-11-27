using UnityEngine;

public class SpeedUp : MonoBehaviour, IInteractable
{
    [Header("速度アップ設定")]
    [Tooltip("速度の倍率")]
    [SerializeField] private float multipleOfSpeed = 1.3f;
    [Tooltip("速度アップの持続時間")]
    [SerializeField] private float speedUpDuration = 10f;

    [Header("エフェクト設定")]
    [SerializeField] private GameObject effectPrefab;

    [Header("通知設定")]
    [Tooltip("通知テキスト")]
    [SerializeField] private string notificationText = "Speed Up!";
    [Tooltip("通知表示時間")]
    [SerializeField] private float notificationDuration = 2f;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        bool isSpeedUp = multipleOfSpeed > 1f;

        if (isSpeedUp) UIManager.Instance.ShowSpeedUpUI(true);
        else UIManager.Instance.ShowSpeedDownUI(true);

        UIManager.Instance.ShowNotification(notificationText, notificationDuration);

        player.ChangeBothSpeedForSomeTime(multipleOfSpeed, speedUpDuration, isSpeedUp);

        if (effectPrefab) Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
