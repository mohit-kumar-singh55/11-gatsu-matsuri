using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲーム全体を管理するクラス
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    #region Private Fields
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
    #endregion

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);
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
        pauseMenuActive = !pauseMenuActive;
        uiManager.ShowPauseMenu(pauseMenuActive);
        Time.timeScale = pauseMenuActive ? 0 : 1;
    }

    public void SetHasKey(bool value) => hasKey = value;

    // タイマーが切れたときのみ
    public void TriggerLose()
    {
        if (gameEnded) return;
        gameEnded = true;

        // リロード用に現在のレベルインデックスを保存する
        PlayerPrefs.SetInt(PLAYERPREFKEYS.LEVEL_TO_RESTART, SceneManager.GetActiveScene().buildIndex);
        // PlayerPrefs.SetInt(PLAYERPREFKEYS.RESET_TIMER, 1);
        SceneLoader.LoadScene(SCENES.GAME_OVER);
    }

    // キーを持った状態でゴールに到達したときのみ
    public void TriggerWin()
    {
        if (!hasKey) return;
        if (gameEnded) return;

        gameEnded = true;

        // PlayerPrefs.SetInt(PLAYERPREFKEYS.RESET_TIMER, 1);
        SceneLoader.LoadScene(SCENES.GAME_CLEAR);
    }

    // 落下によるリスタート
    public void ReloadCurrentLevelWhenFall()
    {
        Instance = null;

        // 落下によるリスタート時にはタイマーをリセットしないため
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

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}