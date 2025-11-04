using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public void GoToMainMenu() => SceneLoader.LoadScene(SCENES.MAIN_MENU);

    // restart game from level 1
    public void RestartGame() => SceneLoader.LoadScene(SCENES.LEVEL_1);

    public void RestartLevel() => SceneLoader.LoadScene(PlayerPrefs.GetInt(PLAYERPREFKEYS.LEVEL_TO_RESTART, SCENES.MAIN_MENU));
}
