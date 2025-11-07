using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    private SFXManager sfxManager;

    void Start()
    {
        sfxManager = SFXManager.Instance;
    }

    public void GoToMainMenu()
    {
        if (sfxManager) sfxManager.PlayButtonClick();
        SceneLoader.LoadScene(SCENES.MAIN_MENU);
    }

    // restart game from level 1
    public void RestartGame()
    {
        PlayerPrefs.SetInt(PLAYERPREFKEYS.RESET_TIMER, 1);
        if (sfxManager) sfxManager.PlayButtonClick();
        SceneLoader.LoadScene(SCENES.LEVEL_1);
    }

    public void RestartLevel()
    {
        PlayerPrefs.SetInt(PLAYERPREFKEYS.RESET_TIMER, 1);
        if (sfxManager) sfxManager.PlayButtonClick();
        SceneLoader.LoadScene(PlayerPrefs.GetInt(PLAYERPREFKEYS.LEVEL_TO_RESTART, SCENES.MAIN_MENU));
    }
}
