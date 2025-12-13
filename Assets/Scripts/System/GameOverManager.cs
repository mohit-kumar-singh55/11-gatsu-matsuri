using UnityEngine;

/// <summary>
/// ゲームオーバー時の処理
/// </summary>
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

    // レベル1からゲームを再スタートする
    public void RestartGame()
    {
        // PlayerPrefs.SetInt(PLAYERPREFKEYS.RESET_TIMER, 1);
        if (sfxManager) sfxManager.PlayButtonClick();
        SceneLoader.LoadScene(SCENES.LEVEL_1);
    }

    // 現在のレベルからゲームを再スタートする
    public void RestartLevel()
    {
        // PlayerPrefs.SetInt(PLAYERPREFKEYS.RESET_TIMER, 1);
        if (sfxManager) sfxManager.PlayButtonClick();
        SceneLoader.LoadScene(PlayerPrefs.GetInt(PLAYERPREFKEYS.LEVEL_TO_RESTART, SCENES.MAIN_MENU));
    }
}
