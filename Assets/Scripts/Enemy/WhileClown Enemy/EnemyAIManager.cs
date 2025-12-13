using UnityEngine;

/// <summary>
/// 敵の AI を制御するクラス
/// </summary>
[RequireComponent(typeof(EnemyController), typeof(EnemyPatrol))]
public class EnemyAIManager : MonoBehaviour
{
    private EnemyController _enemy;
    private EnemyPatrol _patrol;

    private void Awake()
    {
        _enemy = GetComponent<EnemyController>();
        _patrol = GetComponent<EnemyPatrol>();
    }

    void Update()
    {
        // 敵の現在の状態に基づいて敵の行動を制御
        switch (_enemy.CurrentState)
        {
            case EnemyState.Patrol:
                _patrol.enabled = true;
                break;
            case EnemyState.Chasing:
                _patrol.enabled = false;
                break;
        }
    }
}