using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] Transform waypointParent;
    [SerializeField] float waitTimeAtWaypoint = 2f;
    
    private List<Transform> waypoints = new();
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

        // Cache all waypoints once
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
        // if agent hasn't reached the waypoint or no waypoints are assigned, do nothing
        // agentがwaypointに到達していない場合、またはウwaypointsが割り当てられていない場合は、何もしない。
        if (agent.pathPending || waypoints?.Count == 0) return;

        // wait at waypoint
        // waypointに待機
        if (!waiting && agent.remainingDistance <= agent.stoppingDistance)
        {
            waiting = true;
            waitTimer = waitTimeAtWaypoint;
        }

        // if waiting, decrement timer and if timer runs out, go to next waypoint
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

    // go to next waypoint
    // 次のwaypointに行く
    void GoToNextWaypoint()
    {
        if (waypoints.Count == 0) return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
    }

    // for visual debugging purpose only
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
