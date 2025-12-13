using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { Patrol, Chasing };

/*
* パトロール -> プレイヤー検出 ->
* 追跡 -> プレイヤーの視線を失う（数秒後） ->
* 検査中数秒間 -> パトロールに戻る
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
    private Transform _player;
    private NavMeshAgent _agent;
    private Animator _animator;
    private EnemyAttack _enemyAttack;

    // タイマー
    private float _currentDetectTimer = 0f;
    private float _losePlayerTimer = 0f;
    private float _inspectionTimer = 0f;

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
        // initialize
        _player = FindAnyObjectByType<PlayerController>().gameObject.transform;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemyAttack = GetComponent<EnemyAttack>();

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
        if (!_player) return;

        // 敵を動かす
        _agent.speed = walkSpeed;
        _agent.isStopped = false;

        // ** プレイヤーが視界に入っていない場合 **
        if (!IsPlayerInSight())
        {
            _currentDetectTimer = Mathf.Max(0f, _currentDetectTimer - Time.deltaTime);
            return;
        }

        // ** プレイヤーが視界にいる場合 **
        _currentDetectTimer += Time.deltaTime;

        if (_currentDetectTimer >= detectionTime)
        {
            // 敵の状態を追跡に変更
            currentState = EnemyState.Chasing;
            _agent.SetDestination(_player.position);
            // Debug.Log("❗ PLAYER DETECTED! CHASING...");
        }
    }

    private void ChasingBehaviour()
    {
        if (_enemyAttack.IsAttacking) return;

        // プレイヤーを追いかける 
        _agent.speed = chaseSpeed;
        _agent.SetDestination(_player.position);

        // ** プレイヤーが視界にいる場合 **
        if (IsPlayerInSight())
        {
            CheckDistanceAndAttack();
            return;
        }

        // ** プレイヤーが視界から外れた場合 **
        SearchPlayer();
    }

    // 追跡中、プレイヤーが視界にいる場合
    private void CheckDistanceAndAttack()
    {
        // 敵からプレイヤーまでの距離
        float distToPlayer = Vector3.Distance(transform.position, _player.position);

        // ********** Attack **********
        // プレイヤーが十分に近い場合に攻撃する
        if (distToPlayer <= attackDistance)
        {
            // 敵を完全に止めて、攻撃する
            StopEnemyMovement();
            _enemyAttack.Attack(_isAttackingHash);       // 攻撃
            // Debug.Log("🗡️ Attacking _player");
        }
        else _agent.isStopped = false;

        // タイマーをリセット
        _losePlayerTimer = losePlayerTime;
        _inspectionTimer = inspectionTime;
    }

    // 追跡中、プレイヤーが視界から外れた場合
    private void SearchPlayer()
    {
        // プレイヤーを失った場合、タイマーの更新
        _losePlayerTimer -= Time.deltaTime;

        // プレイヤーが失ったら、検査する
        if (_losePlayerTimer < 0)
        {
            // 敵を止めて、検査中アニメーションを再生する
            _agent.isStopped = true;
            // Debug.Log("🔍 Inspecting the place");

            // 検査時間を減らす
            _inspectionTimer -= Time.deltaTime;

            // 検査が終了し、プレイヤーが失われた場合、巡回状態に戻る
            if (_inspectionTimer <= 0)
            {
                _agent.isStopped = false;
                currentState = EnemyState.Patrol;
                // Debug.Log("👁️ Lost _player. Returning to patrol.");
            }
        }
    }

    // プレイヤーが敵の視界にいるかどうかを確認します。
    private bool IsPlayerInSight()
    {
        // ** プレイヤーは視野半径内にいるかどうか **
        Vector3 enemyPosition = transform.position;
        Vector3 dirToPlayer = (_player.position - enemyPosition).normalized;
        float distToPlayer = Vector3.Distance(enemyPosition, _player.position);

        // 視野半径内にプレイヤーがいない場合
        if (distToPlayer > viewRadius) return false;

        // ** プレイヤーは視野角度内にいるかどうか **
        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);
        if (angleToPlayer > viewAngle / 2f) return false;

        // プレイヤーが障害物の後ろに隠れている場合
        if (Physics.Raycast(enemyPosition, dirToPlayer, distToPlayer, obstacleMask)) return false;

        return true;
    }

    // ** blend treeでの移動アニメーションを更新する **
    private void UpdateMoveAnimation()
    {
        float targetVelocity;

        if (_agent.velocity == Vector3.zero) targetVelocity = 0f;
        else targetVelocity = _agent.velocity.magnitude / chaseSpeed;

        _velocity = Mathf.Lerp(_velocity, targetVelocity, Time.deltaTime * 5f);

        _animator.SetFloat(_velocityHash, _velocity);
    }

    // 敵を完全に止める
    private void StopEnemyMovement()
    {
        _agent.isStopped = true;
        _animator.SetFloat(_velocityHash, 0f);
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