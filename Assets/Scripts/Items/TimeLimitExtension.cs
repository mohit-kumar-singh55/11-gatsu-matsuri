using UnityEngine;

public class TimeLimitExtension : MonoBehaviour, IInteractable
{
    [Tooltip("時間制限の延長時間 (秒)")]
    [SerializeField] private float increaseTimeLimitBy = 20f;
    [SerializeField] private GameObject effectPrefab;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        FindAnyObjectByType<CountdownTimer>().IncreaseTime(increaseTimeLimitBy);

        if (effectPrefab) Instantiate(effectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
