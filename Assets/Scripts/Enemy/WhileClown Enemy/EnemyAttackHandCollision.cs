using UnityEngine;

/// <summary>
/// 敵の手の衝突判定
/// </summary>
[RequireComponent(typeof(Collider))]
public class EnemyAttackHandCollision : MonoBehaviour
{
    private Collider _col;
    private EnemyAttack __playerController;
    private PlayerController playerController;

    void Start()
    {
        // initialize
        _col = GetComponent<Collider>();
        __playerController = GetComponentInParent<EnemyAttack>();
        playerController = PlayerController.Instance;
    }

    void FixedUpdate()
    {
        if (__playerController.IsAttacking) _col.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        // ragdoll を有効化する
        playerController.EnableRagdoll();

        // 力を与える
        Vector3 punchForce = (other.transform.position - transform.position).normalized * 500;
        other.GetComponent<Rigidbody>().AddForceAtPosition(punchForce, other.ClosestPoint(transform.position), ForceMode.Impulse);
    }
}