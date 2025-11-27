using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public static CountdownTimer Instance { get; private set; }

    [SerializeField] private float startTime = 90f;

    private float currentTime;
    private bool isRunning = true;
    private UIManager uiManager;

    public float CurrentTime => currentTime;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        CheckIfReset();
        uiManager = UIManager.Instance;
    }

    // ** タイマーの更新 **
    void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            isRunning = false;

            GameManager.Instance.TriggerLose();
            return;
        }

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        uiManager.SetTimerText(minutes, seconds);
    }

    public void IncreaseTime(float amount) => currentTime += amount;

    // ** タイマーをリセットするか、保存された時間から開始するかを確認 **
    public void CheckIfReset()
    {
        if (PlayerPrefs.GetInt(PLAYERPREFKEYS.RESET_TIMER, 1) == 1) currentTime = startTime;
        else currentTime = PlayerPrefs.GetFloat(PLAYERPREFKEYS.TIMER_TO_START_FROM, startTime);
    }
}