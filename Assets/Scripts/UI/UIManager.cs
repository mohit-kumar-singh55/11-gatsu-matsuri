using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("InGame UI")]
    [SerializeField] TMP_Text timerText;

    [Header("Items UI")]
    [SerializeField] GameObject KeyUI;
    [SerializeField] GameObject JumpPowerUI;
    [SerializeField] GameObject SpeedDownUI;
    [SerializeField] GameObject SpeedUpUI;
    [SerializeField] GameObject StaminaDownUI;
    [SerializeField] GameObject StaminaFreezeUI;

    [Header("Stamina UI")]
    [SerializeField] Image staminaImage;
    [SerializeField] Sprite[] staminaFrames;

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

    public void ShowKeyUI(bool show) => KeyUI.SetActive(show);

    public void ShowJumpPowerUI(bool show) => JumpPowerUI.SetActive(show);

    public void ShowSpeedDownUI(bool show) => SpeedDownUI.SetActive(show);

    public void ShowSpeedUpUI(bool show) => SpeedUpUI.SetActive(show);

    public void ShowStaminaDownUI(bool show) => StaminaDownUI.SetActive(show);

    public void ShowStaminaFreezeUI(bool show) => StaminaFreezeUI.SetActive(show);

    public void SetStaminaImage(float currentStamina, float maxStamina)
    {
        int index = Mathf.Clamp(Mathf.FloorToInt(currentStamina / maxStamina * (staminaFrames.Length - 1)), 0, staminaFrames.Length - 1);
        staminaImage.sprite = staminaFrames[staminaFrames.Length - 1 - index];
    }
}
