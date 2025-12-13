using UnityEngine;

/// <summary>
/// ragdoll の有効／無効を切り替える
/// </summary>
public class RagdollEnabler : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private Transform RagdollRoot;

    private Rigidbody[] Rigidbodies;
    private CharacterJoint[] Joints;
    private Collider[] Colliders;

    private void Awake()
    {
        // initialize
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
        Colliders = RagdollRoot.GetComponentsInChildren<Collider>();
    }

    public void EnableRagdoll(bool enable = true)
    {
        Animator.enabled = !enable;     // Animator とラグドールは同時に動作できない

        foreach (CharacterJoint joint in Joints) joint.enableCollision = enable;
        foreach (Collider collider in Colliders) collider.isTrigger = !enable;
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.isKinematic = !enable;
            if (enable) rigidbody.linearVelocity = Vector3.zero;
        }
    }
}