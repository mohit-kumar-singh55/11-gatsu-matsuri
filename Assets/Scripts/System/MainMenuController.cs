using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// メインメニューの制御
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject fader;
    [SerializeField] Image faderImage;

    void Start()
    {
        faderImage = fader.GetComponent<Image>();

        // *** ゲーム中は常にカーソルを非表示にする ***
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadNewGame()
    {
        PlayerPrefs.SetInt(PLAYERPREFKEYS.RESET_TIMER, 1);
        FadeOutScreen();
        if (SFXManager.Instance) SFXManager.Instance.PlayButtonClick();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void FadeOutScreen()
    {
        if (!fader || !faderImage) return;

        fader.SetActive(true);
        StartCoroutine(SetColorAlphaValue());
    }

    // 画面をフェードアウトさせる
    IEnumerator SetColorAlphaValue()
    {
        while (faderImage.color.a < 1f)
        {
            Color newColor = faderImage.color;
            newColor.a += .1f;
            faderImage.color = newColor;

            yield return new WaitForSeconds(.04f);
        }

        SceneLoader.LoadScene(SCENES.INFO_SCENE);
    }
}