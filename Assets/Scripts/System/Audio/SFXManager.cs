using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    [SerializeField] AudioClip buttonClickSFX;
    [SerializeField] AudioClip itemPickupSFX;
    [SerializeField] AudioClip doorOpenSFX;

    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxSource = GetComponent<AudioSource>();
    }

    public void PlayButtonClick() => sfxSource.PlayOneShot(buttonClickSFX);

    public void PlayItemPickup() => sfxSource.PlayOneShot(itemPickupSFX);

    public void PlayDoorOpen() => sfxSource.PlayOneShot(doorOpenSFX);
}
