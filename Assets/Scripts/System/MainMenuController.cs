using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject fader;
    [SerializeField] Image faderImage;

    void Start()
    {
        faderImage = fader.GetComponent<Image>();
    }

    public void LoadNewGame()
    {
        PlayerPrefs.SetInt(PLAYERPREFKEYS.RESET_TIMER, 1);
        FadeOutScreen();
        if (SFXManager.Instance) SFXManager.Instance.PlayButtonClick();
    }

    public void Quit() => Application.Quit();

    void FadeOutScreen()
    {
        if (!fader || !faderImage) return;

        fader.SetActive(true);
        StartCoroutine(SetColorAlphaValueAndVolume());
    }

    IEnumerator SetColorAlphaValueAndVolume()
    {
        while (faderImage.color.a < 1f)
        {
            Color newColor = faderImage.color;
            newColor.a += .1f;
            faderImage.color = newColor;

            yield return new WaitForSeconds(.04f);
        }

        SceneLoader.LoadScene(SCENES.LEVEL_1);
    }
}
