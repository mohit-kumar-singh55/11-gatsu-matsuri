using UnityEngine;

public class SpeedUp : MonoBehaviour, Interactable
{
    [Tooltip("速度の倍率")]
    [SerializeField] private float multipleOfSpeed = 1.3f;
    [Tooltip("速度アップの持続時間")]
    [SerializeField] private float speedUpDuration = 10f;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        player.ChangeBothSpeedForSomeTime(multipleOfSpeed, speedUpDuration);

        Destroy(gameObject);
    }
}
