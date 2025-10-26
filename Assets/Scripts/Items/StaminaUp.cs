using UnityEngine;

public class StaminaUp : MonoBehaviour, Interactable
{
    [SerializeField] private float staminaFreezeDuration = 10f;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        player.FreezeStamina = true;

        player.RestartStaminaDepletionAfterDelay(staminaFreezeDuration);

        Destroy(gameObject);
    }
}
