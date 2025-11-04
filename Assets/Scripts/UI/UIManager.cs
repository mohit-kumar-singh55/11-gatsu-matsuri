using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("InGame UI")]
    [SerializeField] TMP_Text timerText;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetTimerText(float min, float sec) => timerText.text = string.Format("{0:00}:{1:00}", min, sec);
}
