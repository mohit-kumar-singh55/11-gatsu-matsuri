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
        // Changing BGM depending on scene
        int sceneIndex = scene.buildIndex;

        if (sceneIndex <= SCENES.LEVEL_2) PlayBGM(mainMenuBGM);
        else if (sceneIndex == SCENES.LEVEL_3) PlayBGM(level3BGM);
    }

    void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip && bgmSource.isPlaying)
            return; // Already playing correct BGM

        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }
}
