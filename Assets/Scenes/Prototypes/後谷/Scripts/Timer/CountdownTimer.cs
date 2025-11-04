using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float startTime = 90f;

    private float currentTime;
    private bool isRunning = true;
    private UIManager uiManager;

    void Start()
    {
        currentTime = startTime;
        uiManager = UIManager.Instance;
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

        uiManager.SetTimerText(minutes, seconds);
    }

    public void IncreaseTime(float amount) => currentTime += amount;

    public void ResetTimer()
    {
        currentTime = startTime;
        isRunning = true;
    }
}