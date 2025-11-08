using UnityEngine;
using UnityEngine.SceneManagement;

// Singleton Class
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private UIManager uiManager;
    private PlayerController playerController;
    private bool gameEnded = false;
    private bool pauseMenuActive = false;
    private bool hasKey = false;

    // menu keys
    private readonly KeyCode pauseMenuKey = KeyCode.Escape;

    // cheat keys
    private readonly KeyCode cheatKeyNextLevel = KeyCode.L;
    private readonly KeyCode cheatKeyInfiniteJump = KeyCode.J;

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

    void Start()
    {
        uiManager = UIManager.Instance;
        playerController = PlayerController.Instance;
    }

    void Update()
    {
        if (gameEnded) return;

        // cheat keys
        if (Input.GetKeyDown(cheatKeyNextLevel)) GoToNextLevel();
        if (Input.GetKeyDown(cheatKeyInfiniteJump) && playerController) playerController.ToggleInfiniteJump();
        // pause menu toggle
        else if (Input.GetKeyDown(pauseMenuKey)) SetShowMenu();
    }

    public void ResumeGame() => SetShowMenu();

    public void SetShowMenu()
    {
        Debug.Log("Toggling Pause Menu");
        pauseMenuActive = !pauseMenuActive;
        uiManager.ShowPauseMenu(pauseMenuActive);
        Time.timeScale = pauseMenuActive ? 0 : 1;
        ShowCursor(pauseMenuActive);
    }

    public void SetHasKey(bool value) => hasKey = value;

    // only when timer runs out
    public void TriggerLose()
    {
        if (gameEnded) return;

        gameEnded = true;

        // saving current level index for reloading
        PlayerPrefs.SetInt(PLAYERPREFKEYS.LEVEL_TO_RESTART, SceneManager.GetActiveScene().buildIndex);
        // PlayerPrefs.SetInt(PLAYERPREFKEYS.RESET_TIMER, 1);
        GameOverSequence();
        SceneLoader.LoadScene(SCENES.GAME_OVER);
    }

    // only when reaching the goal with the key
    public void TriggerWin()
    {
        if (!hasKey) return;

        if (gameEnded) return;

        gameEnded = true;

        // PlayerPrefs.SetInt(PLAYERPREFKEYS.RESET_TIMER, 1);
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

        // not to reset the timer when restarting level by falling
        // PlayerPrefs.SetInt(PLAYERPREFKEYS.RESET_TIMER, 0);
        // if (CountdownTimer.Instance) PlayerPrefs.SetFloat(PLAYERPREFKEYS.TIMER_TO_START_FROM, CountdownTimer.Instance.CurrentTime);
        SceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToNextLevel()
    {
        Instance = null;

        // PlayerPrefs.SetInt(PLAYERPREFKEYS.RESET_TIMER, 1);

        // 0 -> main menu, 1 -> info, 2, 3, 4 -> levels..., 5 -> game clear, 6 -> game over
        int nextLevelInd = (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings;
        nextLevelInd = nextLevelInd <= 1 ? 2 : nextLevelInd;
        SceneManager.LoadScene(nextLevelInd);
    }

    public void GoToMainMenu()
    {
        Instance = null;

        Time.timeScale = 1f;
        if (SFXManager.Instance) SFXManager.Instance.PlayButtonClick();
        SceneLoader.LoadScene(SCENES.MAIN_MENU);
    }

    public void QuitGame() => Application.Quit();
}
