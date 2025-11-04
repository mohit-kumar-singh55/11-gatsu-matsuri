using UnityEngine;

public class TimeLimitExtension : MonoBehaviour, IInteractable
{
    [Tooltip("時間制限の延長時間 (秒)")]
    [SerializeField] private float increaseTimeLimitBy = 20f;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        // TODO: chagne the time limit
        FindAnyObjectByType<CountdownTimer>().IncreaseTime(increaseTimeLimitBy);

        Destroy(gameObject);
    }
}
