using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    // 時間表示用のText
    [SerializeField]
    private TMP_Text timerText;
    // タイマー開始秒数
    [SerializeField] 
    private float startTime = 90f;
   

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

            // タイムアップでシーン切り替え
            SceneManager.LoadScene("GameOver");
        }

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // 外部から再スタートできるように
    public void ResetTimer()
    {
        currentTime = startTime;
        isRunning = true;
    }
}