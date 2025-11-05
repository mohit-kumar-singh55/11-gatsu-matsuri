using UnityEngine;

enum ObjectPropogatorAxis { X, Y, Z };

public class ObjectPropogator : MonoBehaviour
{
    [Tooltip("1秒間の振動回数")]
    [SerializeField] float speed = 1f;     // Oscillations per second (frequency)
    [Tooltip("中心からの最大オフセット距離")]
    [SerializeField] float distance = 3f;  // Max offset from center
    [Tooltip("何秒後に動き始めるか")]
    [SerializeField] float startAfterSeconds = 0;
    [Tooltip("動く軸")]
    [SerializeField] ObjectPropogatorAxis axis = ObjectPropogatorAxis.Y;

    private Vector3 startPos;
    private Vector3 direction;
    private float timer = 0f;

    // ** stuff to make player move with platform and not slide off **
    private Vector3 lastPos;
    private Vector3 platformVelocity;

    public Vector3 PlatformVelocity => platformVelocity;

    void Start()
    {
        startPos = transform.position;
        lastPos = startPos;
        timer = startAfterSeconds;

        direction = axis == ObjectPropogatorAxis.X ? Vector3.right : axis == ObjectPropogatorAxis.Y ? Vector3.up : Vector3.forward;
    }

    void Update()
    {
        timer += Time.deltaTime * speed * Mathf.PI * 2f; // radians per second

        float offset = Mathf.Sin(timer) * distance;

        transform.position = startPos + direction * offset;

        // ** stuff to make player move with platform and not slide off **
        // Calculate velocity
        platformVelocity = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
    }
}
