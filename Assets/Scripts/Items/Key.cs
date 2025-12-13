using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    #region Serialize Fields
    [Header("通知設定")]
    [Tooltip("通知テキスト")]
    [SerializeField] private string notificationText = "Key Acquired!";
    [Tooltip("通知表示時間")]
    [SerializeField] private float notificationDuration = 2f;
    #endregion

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        GameManager.Instance.SetHasKey(true);

        // UI更新
        UIManager.Instance.ShowKeyUI(true);
        UIManager.Instance.ShowNotification(notificationText, notificationDuration);

        Destroy(gameObject);
    }
}