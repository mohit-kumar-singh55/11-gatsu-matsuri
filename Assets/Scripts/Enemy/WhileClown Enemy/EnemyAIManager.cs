using UnityEngine;

[RequireComponent(typeof(EnemyController), typeof(EnemyPatrol))]
public class EnemyAIManager : MonoBehaviour
{
    private EnemyController enemy;
    private EnemyPatrol patrol;

    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
        patrol = GetComponent<EnemyPatrol>();
    }

    void Update()
    {
        // 敵の現在の状態に基づいて敵の行動を制御
        switch (enemy.CurrentState)
        {
            case EnemyState.Patrol:
                patrol.enabled = true;
                break;
            case EnemyState.Chasing:
                patrol.enabled = false;
                break;
        }
    }
}