using UnityEngine;

public class LevelFinishDetector : MonoBehaviour
{
    [SerializeField] bool isLastLevel = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAGS.PLAYER))
        {
            if (isLastLevel) GameManager.Instance.TriggerWin();
            else GameManager.Instance.GoToNextLevel();

            if (SFXManager.Instance) SFXManager.Instance.PlayDoorOpen();
        }
    }
}
