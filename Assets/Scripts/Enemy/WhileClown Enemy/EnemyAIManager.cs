using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyController), typeof(EnemyPatrol))]
public class EnemyAIManager : MonoBehaviour
{
    private EnemyController enemy;
    private EnemyPatrol patrol;
    private Animator animator;
    private NavMeshAgent agent;



    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
        patrol = GetComponent<EnemyPatrol>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
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