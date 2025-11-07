using UnityEngine;

public class StaminaUp : MonoBehaviour, IInteractable
{
    [Tooltip("スタミナの持続時間")]
    [SerializeField] private float staminaFreezeDuration = 10f;
    [SerializeField] private GameObject effectPrefab;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        player.FreezeStamina = true;

        UIManager.Instance.ShowStaminaFreezeUI(true);

        player.RestartStaminaDepletionAfterDelay(staminaFreezeDuration);

        if (effectPrefab) Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
