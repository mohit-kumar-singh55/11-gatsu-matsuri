using UnityEngine;

public class StaminaDown : MonoBehaviour, IInteractable
{
    [Tooltip("スタミナ減少速度の倍率")]
    [SerializeField] private float multipleOfStamina = .7f;
    [Tooltip("スタミナ減少速度変更の持続時間（秒）")]
    [SerializeField] private float staminaDecrementDuration = 10f;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        UIManager.Instance.ShowStaminaDownUI(true);

        player.ChangeStaminaForSomeTime(multipleOfStamina, staminaDecrementDuration);

        Destroy(gameObject);
    }
}
