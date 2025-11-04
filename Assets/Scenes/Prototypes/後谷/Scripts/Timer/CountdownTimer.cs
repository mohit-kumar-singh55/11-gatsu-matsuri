using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float startTime = 90f;

    private float currentTime;
    private bool isRunning = true;

    void Start()
    {
        currentTime = startTime;
    }

    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;

            // SceneManager.LoadScene("GameOver");
        }

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        isRunning = true;
    }
}