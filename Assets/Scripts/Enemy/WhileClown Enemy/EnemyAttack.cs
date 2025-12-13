using System.Collections;
using UnityEngine;

/// <summary>
/// 敵の攻撃を制御するクラス
/// </summary>
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

    // 敵の攻撃
    public void Attack(int attackAnimHash)
    {
        if (isAttacking) return;

        isAttacking = true;
        StartCoroutine(PlayAttackSequence(attackAnimHash));     // 攻撃開始
    }

    IEnumerator PlayAttackSequence(int attackAnimHash)
    {
        // ** 1: プレイヤーを完全に止める **
        playerController.FreezePlayer(true);
        playerController.enabled = false;

        // ** 2: cinematic カメラに切り替える **
        cameraController.ShowCinematicCam(true);

        // ** 3: 時間を遅くする **
        Time.timeScale = 0.15f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // ** 4: 攻撃アニメション **
        animator.SetTrigger(attackAnimHash);

        // ** 5: 攻撃アニメションが終わるまで待つ **
        yield return new WaitForSecondsRealtime(attackCinematicDuration);

        // 画面を揺らす
        cameraController.ScreenShake();

        // ** 6: 全てを元に戻す **
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        cameraController.ShowCinematicCam(false);
        isAttacking = false;

        // リスタートする前に、ちょっと待つ
        yield return new WaitForSeconds(postAttackDelay);

        // ** 7: このレベルをリスタート **
        GameManager.Instance.ReloadCurrentLevelWhenFall();
    }
}