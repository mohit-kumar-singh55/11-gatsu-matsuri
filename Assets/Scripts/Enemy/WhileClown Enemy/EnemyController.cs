using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { Patrol, Chasing };

/*
* パトロール -> プレイヤー検出 ->
* 追跡 -> プレイヤーの視線を失う（数秒後） ->
* 疑わしい（調査中）数秒間 -> パトロールに戻る
*/

/// <summary>
/// 敵の行動を制御するクラス
/// </summary>
[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(EnemyAttack))]
public class EnemyController : MonoBehaviour
{
    #region Serialized Fields
    [Header("General Settings")]
    [SerializeField] EnemyState currentState = EnemyState.Patrol;

    [Header("Vision Settings")]
    [SerializeField] float viewRadius = 10f;
    [Range(0f, 360f)][SerializeField] float viewAngle = 90f;
    [SerializeField] LayerMask obstacleMask;      // 壁や岩など

    [Header("Detection Settings")]
    [SerializeField] float detectionTime = 2f;


    [Header("Movement Settings")]
    [SerializeField] float walkSpeed = 3f;       // 敵の移動速度 - これはナビメッシュエージェントのデフォルト速度を上書きします
    [SerializeField] float chaseSpeed = 5f;       // プレイヤーを追いかける速度
    [SerializeField] float attackDistance = 2f;   // プレイヤーを攻撃する距離

    // プレイヤーが視界にない場合は、プレイヤーを失う時間です。
    [SerializeField] float losePlayerTime = 3f;

    // 疑わしいタイマー -> プレイヤーを探すためのタイマー
    [SerializeField] float inspectionTime = 3f;
    #endregion

    #region Private Fields
    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyAttack enemyAttack;

    // タイマー
    private float currentDetectTimer = 0f;
    private float losePlayerTimer = 0f;
    private float inspectionTimer = 0f;

    // アニメーター用
    private float _velocity = 0f;
    private int _velocityHash;
    private int _isAttackingHash;

    // アニメーター変数
    private const string ANIM_SPEED = "speed";
    private const string ANIM_ATTACKING = "isAttacking";
    #endregion

    public EnemyState CurrentState => currentState;

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().gameObject.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();

        _velocityHash = Animator.StringToHash(ANIM_SPEED);
        _isAttackingHash = Animator.StringToHash(ANIM_ATTACKING);
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                PatrolBehaviour();
                break;
            case EnemyState.Chasing:
                ChasingBehaviour();
                break;
        }
    }

    void FixedUpdate()
    {
        UpdateMoveAnimation();
    }

    /// <summary>
    /// パトロール可能な敵のためのパトロール行動。
    /// この機能は、敵が巡回し、プレイヤーを見ると追跡状態に設定します。
    /// </summary>
    private void PatrolBehaviour()
    {
        if (!player) return;

        agent.speed = walkSpeed;
        agent.isStopped = false;

        if (IsPlayerInSight())
        {
            currentDetectTimer += Time.deltaTime;

            if (currentDetectTimer >= detectionTime)
            {
                // 敵の状態を追跡に変更
                currentState = EnemyState.Chasing;
                agent.SetDestination(player.position);

                Debug.Log("❗ PLAYER DETECTED! CHASING...");
            }
        }
        else currentDetectTimer = Mathf.Max(0f, currentDetectTimer - Time.deltaTime);
    }

    private void ChasingBehaviour()
    {
        if (enemyAttack.IsAttacking) return;

        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);      // プレイヤーを追いかける

        if (IsPlayerInSight())
        {
            // 近くにいる場合は、プレイヤーを切る
            float distToPlayer = Vector3.Distance(transform.position, player.position);

            // ********** Attack **********
            // プレイヤーが十分に近い場合に攻撃する
            if (distToPlayer <= attackDistance)
            {
                // 敵を止めて攻撃
                agent.isStopped = true;
                enemyAttack.Attack(_isAttackingHash);

                // *** Stop enemy movement ***
                StopEnemyMovement();

                Debug.Log("🗡️ Attacking player");
            }
            else agent.isStopped = false;

            // タイマーをリセット
            losePlayerTimer = losePlayerTime;
            inspectionTimer = inspectionTime;
        }
        else
        {
            // chasing cooldown timer
            losePlayerTimer -= Time.deltaTime;

            if (losePlayerTimer < 0)
            {
                // 敵を止めて、疑わしい（調査中）アニメーションを再生する
                agent.isStopped = true;
                Debug.Log("🔍 Inspecting the place");

                // 疑わしい（調査中）アニメーションの再生後、疑わしい（調査中）時間を減らす
                inspectionTimer -= Time.deltaTime;

                // 調査が終了し、プレイヤーが失われた場合、パトロールに戻ります...
                if (inspectionTimer <= 0)
                {
                    agent.isStopped = false;
                    currentState = EnemyState.Patrol;

                    Debug.Log("👁️ Lost player. Returning to patrol.");
                }
            }
        }
    }

    /// <summary>
    /// プレイヤーが敵の視界にいるかどうかを確認します。
    /// これは次のチェックを行います:
    /// 1. プレイヤーは視野半径内にいるかどうか？
    /// 2. プレイヤーは視野角度内にいるかどうか？
    /// 3. 視野の間に障害物があるかどうか（レイキャストチェック）？
    /// いずれかの条件が偽の場合、プレイヤーは視界にいない
    /// </summary>
    private bool IsPlayerInSight()
    {
        Vector3 enemyPosition = transform.position;
        Vector3 dirToPlayer = (player.position - enemyPosition).normalized;
        float distToPlayer = Vector3.Distance(enemyPosition, player.position);

        if (distToPlayer > viewRadius) return false;

        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
        if (angleToPlayer > viewAngle / 2f) return false;

        // this obstacle mask is so that if player is hiding behind any obstacle this raycast should be blocked by the obstacle
        // この障害物マスクは、プレイヤーが障害物の後ろに隠れている場合、このレイキャストが障害物によってブロックされるようにするためのものです。
        if (Physics.Raycast(enemyPosition, dirToPlayer, distToPlayer, obstacleMask)) return false;

        return true;
    }

    // ** blend treeでの移動アニメーションを更新する **
    private void UpdateMoveAnimation()
    {
        float targetVelocity;

        if (agent.velocity == Vector3.zero) targetVelocity = 0f;
        else targetVelocity = agent.velocity.magnitude / chaseSpeed;

        _velocity = Mathf.Lerp(_velocity, targetVelocity, Time.deltaTime * 5f);

        animator.SetFloat(_velocityHash, _velocity);
    }

    // 敵のlose条件を発生した後にこのスクリプトを無効化します。
    private void StopEnemyMovement()
    {
        // stop enemy movement
        agent.isStopped = true;
        animator.SetFloat(_velocityHash, 0f);
        enabled = false;
    }

    // 視覚的デバッグ目的のみ
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2, false);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);
    }

    // 視覚的デバッグ目的のみ
    private Vector3 DirFromAngle(float angle, bool global)
    {
        if (!global) angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}