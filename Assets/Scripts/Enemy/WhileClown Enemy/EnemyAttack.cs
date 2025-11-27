using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAttack : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;
    private CameraController cameraController;

    private bool isAttacking = false;

    private const float attackCinematicDuration = 3.5f;
    private const float postAttackDelay = 3f;

    public bool IsAttacking => isAttacking;

    void Start()
    {
        // initialize
        animator = GetComponent<Animator>();
        cameraController = CameraController.Instance;
        playerController = PlayerController.Instance;
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
        // ** 🔁 Step 1: Disable player control **
        playerController.FreezePlayer(true);
        playerController.enabled = false;

        // ** 🔁 Step 2: Switch to cinematic camera **
        cameraController.ShowCinematicCam(true);

        // ** 🔁 Step 3: Slow down time **
        Time.timeScale = 0.15f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // ** 🔁 Step 4: Play attack animation **
        animator.SetTrigger(attackAnimHash);

        // ** 🔁 Step 5: Wait for attack animation to over **
        yield return new WaitForSecondsRealtime(attackCinematicDuration);

        // Screen Shake
        cameraController.ScreenShake();

        // ** 🔁 Step 6: Return to normal **
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        cameraController.ShowCinematicCam(false);

        isAttacking = false;

        // Wait for animation to over
        yield return new WaitForSeconds(postAttackDelay);

        // ** 🔁 Step 7: Trigger restart level **
        GameManager.Instance.ReloadCurrentLevelWhenFall();
    }
}
