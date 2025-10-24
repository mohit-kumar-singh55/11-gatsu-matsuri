using UnityEngine;

public class EnemyAttackHandCollision : MonoBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        playerController = PlayerController.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        // activate ragdoll
        playerController.EnableRagdoll();

        // apply force
        Vector3 punchForce = (other.transform.position - transform.position).normalized * 500;
        other.GetComponent<Rigidbody>().AddForceAtPosition(punchForce, other.ClosestPoint(transform.position), ForceMode.Impulse);
    }
}
