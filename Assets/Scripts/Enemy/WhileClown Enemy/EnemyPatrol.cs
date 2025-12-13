using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵の移動を制御し、各ウェイポイントで一定時間待機しながら次の地点へ移動させるクラス
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] Transform waypointParent;
    [SerializeField] float waitTimeAtWaypoint = 2f;

    private readonly List<Transform> waypoints = new();
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    private float waitTimer = 0f;
    private bool waiting = false;

    void Awake()
    {
        if (waypointParent == null)
        {
            Debug.LogError("Waypoints or enemy not assigned!", this);
            enabled = false;
            return;
        }

        // waypointsを一度にキャッシュ
        foreach (Transform child in waypointParent) waypoints.Add(child);

        // Initialize
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        GoToNextWaypoint();
    }

    void Update()
    {
        // agentがwaypointに到達していない場合、またはウwaypointsが割り当てられていない場合は、何もしない。
        if (agent.pathPending || waypoints?.Count == 0) return;

        // waypointに待機
        if (!waiting && agent.remainingDistance <= agent.stoppingDistance)
        {
            waiting = true;
            waitTimer = waitTimeAtWaypoint;
        }

        // 待機中ならタイマーを減らして、タイマーが終わったら次のwaypointに行く
        if (waiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                waiting = false;
                GoToNextWaypoint();
            }
        }
    }

    // 次のwaypointに行く
    void GoToNextWaypoint()
    {
        if (waypoints.Count == 0) return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }

    // 視覚的デバッグ目的のみ
    // void OnDrawGizmosSelected()
    // {
    //     if (waypoints == null) return;

    //     Gizmos.color = Color.green;
    //     for (int i = 0; i < waypoints.Length; i++)
    //     {
    //         Vector3 wp = waypoints[i].position;
    //         Gizmos.DrawSphere(wp, 0.2f);

    //         if (i < waypoints.Length - 1)
    //             Gizmos.DrawLine(wp, waypoints[i + 1].position);
    //     }
    // }
}
