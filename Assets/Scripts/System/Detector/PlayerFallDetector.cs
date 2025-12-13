using UnityEngine;

/// <summary>
/// プレイヤーの落下判定
/// </summary>
public class PlayerFallDetector : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAGS.PLAYER))
        {
            // プレイヤーが落下したらリセット
            if (GameManager.Instance) GameManager.Instance.ReloadCurrentLevelWhenFall();
        }
    }
}