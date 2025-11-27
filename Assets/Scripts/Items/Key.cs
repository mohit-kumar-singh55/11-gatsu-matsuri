using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    [Header("通知設定")]
    [Tooltip("通知テキスト")]
    [SerializeField] private string notificationText = "Key Acquired!";
    [Tooltip("通知表示時間")]
    [SerializeField] private float notificationDuration = 2f;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        GameManager.Instance.SetHasKey(true);
        UIManager.Instance.ShowKeyUI(true);
        UIManager.Instance.ShowNotification(notificationText, notificationDuration);

        Destroy(gameObject);
    }
}
