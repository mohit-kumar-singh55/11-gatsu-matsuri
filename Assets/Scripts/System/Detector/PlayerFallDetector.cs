using UnityEngine;

public class PlayerFallDetector : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAGS.PLAYER))
        {
            // checking because of double collider setup on player
            if (GameManager.Instance) GameManager.Instance.ReloadCurrentLevelWhenFall();
        }
    }
}
