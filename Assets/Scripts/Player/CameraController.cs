using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [SerializeField] private CinemachineCamera followCam;
    [SerializeField] private CinemachineCamera cinematicCam;

    private CinemachineImpulseSource impulseSource;

    void Awake()
    {
        // ** singleton **
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ShowCinematicCam(bool show = true)
    {
        cinematicCam.gameObject.SetActive(show);
        followCam.gameObject.SetActive(!show);
    }

    // Impulse when getting attacked
    // 攻撃を受けたときの衝動
    public void ScreenShake() => impulseSource.GenerateImpulse(20f);
}
