using UnityEngine;

public class StaminaDown : MonoBehaviour, IInteractable
{
    #region Serialize Fields
    [Header("スタミナ減少設定")]
    [Tooltip("スタミナ減少速度の倍率")]
    [SerializeField] private float multipleOfStamina = .7f;
    [Tooltip("スタミナ減少速度変更の持続時間（秒）")]
    [SerializeField] private float staminaDecrementDuration = 10f;

    [Header("エフェクト設定")]
    [SerializeField] private GameObject effectPrefab;

    [Header("通知設定")]
    [Tooltip("通知テキスト")]
    [SerializeField] private string notificationText = "Stamina Down!";
    [Tooltip("通知表示時間")]
    [SerializeField] private float notificationDuration = 2f;
    #endregion

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        // UI更新
        UIManager.Instance.ShowStaminaDownUI(true);
        UIManager.Instance.ShowNotification(notificationText, notificationDuration);

        // プレイヤーのスタミナを変更
        player.ChangeStaminaForSomeTime(multipleOfStamina, staminaDecrementDuration);

        // エフェクト
        if (effectPrefab) Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}