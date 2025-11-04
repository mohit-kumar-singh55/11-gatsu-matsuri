using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public void GoToMainMenu() => SceneLoader.LoadScene(0);

    public void RestartLevel() => SceneLoader.LoadScene(PlayerPrefs.GetInt(PLAYERPREFKEYS.LEVEL_TO_RESTART, 0));
}
