using UnityEngine;
using UnityEngine.SceneManagement;

// Singleton Class
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    // private AudioManager audioManager;
    private bool gameEnded = false;
    private bool hasKey = false;

    private readonly KeyCode cheatKeyNextLevel = KeyCode.L;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);

        ShowCursor(false);
    }

    void Update()
    {
        // cheat to go to next level
        if (Input.GetKeyDown(cheatKeyNextLevel)) GoToNextLevel();
    }

    public void SetHasKey(bool value) => hasKey = value;

    // only when timer runs out
    public void TriggerLose()
    {
        if (gameEnded) return;

        gameEnded = true;

        // saving current level index for reloading
        PlayerPrefs.SetInt(PLAYERPREFKEYS.LEVEL_TO_RESTART, SceneManager.GetActiveScene().buildIndex);
        GameOverSequence();
        SceneLoader.LoadScene(SCENES.GAME_OVER);
    }

    public void TriggerWin()
    {
        if (!hasKey) return;

        if (gameEnded) return;

        gameEnded = true;

        GameOverSequence();
        SceneLoader.LoadScene(SCENES.GAME_CLEAR);
    }

    public void GameOverSequence()
    {
        ShowCursor(true);
        // Timer.Instance.StopTimer(true);
        // AudioManager.Instance.StopAllAudios(true);
    }

    void ShowCursor(bool show = true)
    {
        Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = show;
    }

    public void ReloadCurrentLevelWhenFall()
    {
        Instance = null;
        SceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToNextLevel()
    {
        Instance = null;

        // 0 -> main menu, 1, 2, 3 -> levels..., 4 -> game over, 5 -> game clear
        int nextLevelInd = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        nextLevelInd = nextLevelInd <= 0 ? 1 : nextLevelInd;
        SceneManager.LoadScene(nextLevelInd);
    }

    public void QuitGame() => Application.Quit();
}
