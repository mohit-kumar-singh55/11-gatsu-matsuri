using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI を管理するクラス
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    #region Serialize Fields
    [Header("InGame UI")]
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text notificationText;

    [Header("Pause Menu UI")]
    [SerializeField] GameObject pauseMenuUI;

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
    #endregion

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

    public void ShowPauseMenu(bool show) => pauseMenuUI.SetActive(show);

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

    public void ShowNotification(string message, float duration)
    {
        StopAllCoroutines();
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        StartCoroutine(HideNotificationAfterDelay(duration));
    }

    // coroutines
    IEnumerator HideNotificationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        notificationText.gameObject.SetActive(false);
    }
}
