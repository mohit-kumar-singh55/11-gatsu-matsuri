using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    [SerializeField] AudioSource bgmSource;
    [Tooltip("MainMenu, Level1, Level2 BGM")]
    [SerializeField] AudioClip mainMenuBGM; // Menu–Level2
    [Tooltip("Level3 BGM")]
    [SerializeField] AudioClip level3BGM; // Level3
    [Tooltip("Game Over BGM")]
    [SerializeField] AudioClip gameOverBGM; // game over

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Changing BGM depending on scene (シーンに応じてBGMを変更)
        int sceneIndex = scene.buildIndex;

        if (sceneIndex <= SCENES.LEVEL_2) PlayBGM(mainMenuBGM);
        else if (sceneIndex == SCENES.LEVEL_3) PlayBGM(level3BGM);
        else if (sceneIndex == SCENES.GAME_OVER) PlayBGM(gameOverBGM, false);
    }

    void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return; // Already playing correct BGM (すでに正しいBGMが再生されている)

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }
}
