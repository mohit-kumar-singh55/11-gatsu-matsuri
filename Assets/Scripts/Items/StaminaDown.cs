using UnityEngine;

public class StaminaDown : MonoBehaviour, IInteractable
{
    [Tooltip("速度の倍率")]
    [SerializeField] private float multipleOfStamina = .7f;
    [Tooltip("速度アップの持続時間")]
    [SerializeField] private float staminaDecrementDuration = 10f;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        player.ChangeStaminaForSomeTime(multipleOfStamina, staminaDecrementDuration);

        Destroy(gameObject);
    }
}
