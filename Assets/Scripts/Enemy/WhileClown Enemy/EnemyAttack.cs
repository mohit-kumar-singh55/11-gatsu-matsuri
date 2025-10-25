using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private Animator animator;
    private GameObject player;
    private PlayerController playerController;
    private CameraController cameraController;

    private bool isAttacking = false;

    public bool IsAttacking => isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = PlayerController.Instance.gameObject;
        cameraController = CameraController.Instance;

        playerController = player.GetComponent<PlayerController>();
    }

    // NPCがプレイヤーを蹴る
    public void Attack(int attackAnimHash)
    {
        if (isAttacking) return;

        isAttacking = true;
        StartCoroutine(PlayAttackSequence(attackAnimHash));     // attacking sequence
    }

    IEnumerator PlayAttackSequence(int attackAnimHash)
    {
        // ** 🔁 Step 1: Disable player control (プレイヤーの操作を無効にする) **
        playerController.FreezePlayer(true);
        playerController.enabled = false;

        // ** 🔁 Step 2: Switch to cinematic camera (シネマティックカメラに切り替える) **
        cameraController.ShowCinematicCam(true);

        // ** 🔁 Step 3: Slow down time (時間を遅くする) **
        Time.timeScale = 0.15f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // ** 🔁 Step 4: Play attack animation (アニメーションを再生する) **
        animator.SetTrigger(attackAnimHash);

        // ** 🔁 Step 5: Wait for attack animation to over **
        yield return new WaitForSecondsRealtime(3.5f);

        // playing attacked sfx (被攻撃の効果音を再生中)
        // AudioManager.Instance.PlayKickExplosionSFX();

        // Screen Shake
        cameraController.ScreenShake();

        // ** 🔁 Step 6: Return to normal (元に戻る) **
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        cameraController.ShowCinematicCam(false);

        // playing bgm audios (BGMオーディオを再生する)
        // AudioManager.Instance.PlayBGM();

        // ** 🔁 Step 7: Disabling animations **
        playerController.enabled = false;

        isAttacking = false;
    }
}
