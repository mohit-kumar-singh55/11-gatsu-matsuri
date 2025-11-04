using UnityEngine;

public class PlayerFallDetector : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAGS.PLAYER))
        {
            GameManager.Instance.ReloadCurrentLevelWhenFall();
        }
    }
}
