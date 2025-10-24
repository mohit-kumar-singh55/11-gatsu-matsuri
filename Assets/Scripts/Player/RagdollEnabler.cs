using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private Transform RagdollRoot;

    private Rigidbody[] Rigidbodies;
    private CharacterJoint[] Joints;
    private Collider[] Colliders;

    private void Awake()
    {
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
        Colliders = RagdollRoot.GetComponentsInChildren<Collider>();
    }

    public void EnableRagdoll(bool enable = true)
    {
        Animator.enabled = !enable;     // animator and ragdoll can't work together

        foreach (CharacterJoint joint in Joints) joint.enableCollision = enable;
        foreach (Collider collider in Colliders) collider.isTrigger = !enable;
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.isKinematic = !enable;
            if (enable) rigidbody.linearVelocity = Vector3.zero;
        }
    }
}