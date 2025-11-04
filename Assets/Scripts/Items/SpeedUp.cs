using UnityEngine;

public class SpeedUp : MonoBehaviour, IInteractable
{
    [Tooltip("速度の倍率")]
    [SerializeField] private float multipleOfSpeed = 1.3f;
    [Tooltip("速度アップの持続時間")]
    [SerializeField] private float speedUpDuration = 10f;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        bool isSpeedUp = multipleOfSpeed > 1f;

        if (isSpeedUp) UIManager.Instance.ShowSpeedUpUI(true);
        else UIManager.Instance.ShowSpeedDownUI(true);

        player.ChangeBothSpeedForSomeTime(multipleOfSpeed, speedUpDuration, isSpeedUp);

        Destroy(gameObject);
    }
}
