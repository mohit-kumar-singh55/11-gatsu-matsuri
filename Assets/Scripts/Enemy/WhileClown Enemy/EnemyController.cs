using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { Patrol, Chasing };

/*
* Patrol -> Player Detected -> 
* Chasing -> Player sight lost (after a few seconds) -> 
* Suspicious (inspecting) for a few seconds -> Return to patrol
*/
/*
* パトロール -> プレイヤー検出 ->
* 追跡 -> プレイヤーの視線を失う（数秒後） ->
* 疑わしい（調査中）数秒間 -> パトロールに戻る
*/
[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(EnemyAttack))]
public class EnemyController : MonoBehaviour
{
    #region Fields
    [Header("General Settings")]
    [SerializeField] EnemyState currentState = EnemyState.Patrol;

    [Header("Vision Settings")]
    [SerializeField] float viewRadius = 10f;
    [Range(0f, 360f)][SerializeField] float viewAngle = 90f;
    [SerializeField] LayerMask obstacleMask;      // like wall, rocks, etc (壁や岩など)

    [Header("Detection Settings")]
    [SerializeField] float detectionTime = 2f;
    private float currentDetectTimer = 0f;

    [Header("Movement Settings")]
    [SerializeField] float walkSpeed = 3f;       // enemy walk speed  -  this will override navmash agent's default speed (敵の移動速度 - これはナビメッシュエージェントのデフォルト速度を上書きします)
    [SerializeField] float chaseSpeed = 5f;       // speed to chase player (プレイヤーを追いかける速度)
    [SerializeField] float attackDistance = 2f;   // distance to attack player (プレイヤーを攻撃する距離)

    // time to lose player if player is not in sight (プレイヤーが視界にない場合は、プレイヤーを失う時間です。)
    [SerializeField] float losePlayerTime = 3f;
    private float losePlayerTimer = 0f;

    // suspicious timer -> timer to search for player (疑わしいタイマー -> プレイヤーを探すためのタイマー)
    [SerializeField] float inspectionTime = 3f;
    private float inspectionTimer = 0f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;
    private EnemyAttack enemyAttack;

    // private AudioManager audioManager;

    // for animator
    private float _velocity = 0f;
    private int _velocityHash;
    private int _isAttackingHash;

    // animator variables (アニメーター変数)
    const string ANIM_SPEED = "speed";
    const string ANIM_ATTACKING = "isAttacking";

    public EnemyState CurrentState => currentState;
    #endregion

    void Start()
    {
        player = FindAnyObjectByType<PlayerController>().gameObject.transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyAttack = GetComponent<EnemyAttack>();

        _velocityHash = Animator.StringToHash(ANIM_SPEED);
        _isAttackingHash = Animator.StringToHash(ANIM_ATTACKING);

        // audioManager = AudioManager.Instance;
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
    /// Patrol behaviour for the patrollable enemy.
    /// This function makes the enemy patrol and set to chasing state if it sees the player.
    /// パトロール可能な敵のためのパトロール行動。
    /// この機能は、敵が巡回し、プレイヤーを見ると追跡状態に設定します。
    /// </summary>
    void PatrolBehaviour()
    {
        if (player == null) return;

        agent.speed = walkSpeed;

        if (IsPlayerInSight())
        {
            currentDetectTimer += Time.deltaTime;

            if (currentDetectTimer >= detectionTime)
            {
                // sfx
                // audioManager.PlayPlayerSpottedSFX(enemyGender);

                // chasing player
                currentState = EnemyState.Chasing;
                agent.SetDestination(player.position);

                Debug.Log("❗ PLAYER DETECTED! CHASING...");
            }
        }
        else
        {
            currentDetectTimer -= Time.deltaTime;
            currentDetectTimer = Mathf.Clamp(currentDetectTimer, 0f, detectionTime);
        }
    }

    void ChasingBehaviour()
    {
        if (enemyAttack.IsAttacking) return;

        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);      // follow player

        // stopping bgm audios
        // audioManager.StopBGM();

        if (IsPlayerInSight())
        {
            // slash the player if close enough
            float distToPlayer = Vector3.Distance(transform.position, player.position);

            // ********** Attack **********
            // attack if player is close enough
            if (distToPlayer <= attackDistance)
            {
                // slow motion sfx
                // audioManager.PlaySlowMotionSFX();

                // stopping agent and attack
                agent.isStopped = true;
                enemyAttack.Attack(_isAttackingHash);

                // *** Stop enemy movement ***
                StopEnemyMovement();

                Debug.Log("🗡️ Attacking player");
            }
            else agent.isStopped = false;

            // reset timer
            losePlayerTimer = losePlayerTime;
            inspectionTimer = inspectionTime;
        }
        else
        {
            // chasing cooldown timer
            losePlayerTimer -= Time.deltaTime;

            if (losePlayerTimer < 0)
            {
                // stopping and playing suspicious (inspecting) animation
                agent.isStopped = true;
                Debug.Log("🔍 Inspecting the place");

                // suspicious (inspecting) cooldown timer
                inspectionTimer -= Time.deltaTime;

                // inspection finished and player lost, return to patrol...
                if (inspectionTimer <= 0)
                {
                    // playing bgm audios
                    // audioManager.PlayBGM();

                    agent.isStopped = false;
                    currentState = EnemyState.Patrol;

                    Debug.Log("👁️ Lost player. Returning to patrol.");
                }
            }
        }
    }

    /// <summary>
    /// Checks whether the player is in the enemy's sight.
    /// This does the following checks:
    /// 1. Is the player in the view radius?
    /// 2. Is the player in the view angle?
    /// 3. Is there an obstacle in the way (raycast check)?
    /// If any of these conditions are false, the player is not in sight
    /// プレイヤーが敵の視界にいるかどうかを確認します。
    /// これは次のチェックを行います:
    /// 1. プレイヤーは視野半径内にいるかどうか？
    /// 2. プレイヤーは視野角度内にいるかどうか？
    /// 3. 視野の間に障害物があるかどうか（レイキャストチェック）？
    /// いずれかの条件が偽の場合、プレイヤーは視界にいない
    /// </summary>
    bool IsPlayerInSight()
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

    // ** updating move animation in blend tree (移動アニメーションを更新する) **
    void UpdateMoveAnimation()
    {
        float targetVelocity;

        if (agent.velocity == Vector3.zero) targetVelocity = 0f;
        else targetVelocity = agent.velocity.magnitude / chaseSpeed;

        _velocity = Mathf.Lerp(_velocity, targetVelocity, Time.deltaTime * 5f);

        animator.SetFloat(_velocityHash, _velocity);
    }

    // Disables this script after triggering the lose condition.
    // 敵のlose条件を発生した後にこのスクリプトを無効化します。
    void StopEnemyMovement()
    {
        // stop all audios
        // audioManager.StopBGM();

        // stop enemy movement
        agent.isStopped = true;
        animator.SetFloat(_velocityHash, 0f);
        enabled = false;

        // ** trigger lose condition called in enemy attack script **
    }

    // for visual debugging purpose only
    // 視覚的デバッグ目的のみ
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2, false);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);
    }

    // for visual debugging purpose only
    // 視覚的デバッグ目的のみ
    public Vector3 DirFromAngle(float angle, bool global)
    {
        if (!global) angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}