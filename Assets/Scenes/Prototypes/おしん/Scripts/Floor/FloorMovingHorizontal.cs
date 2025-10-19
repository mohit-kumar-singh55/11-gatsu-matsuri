using UnityEngine;

/// <summary>
/// オブジェクトを水平に動かす
/// </summary>
public class FloorMovingHorizontal : MonoBehaviour
{
    /// <summary>
    /// Rigidbody取得用
    /// </summary>
    private Rigidbody body;

    /// <summary>
    /// オブジェクトの位置
    /// </summary>
    private Vector3 startPosition;

    /// <summary>
    /// FloorMovingの移動速度
    /// </summary>
    [SerializeField]
    private float moveSpeed;

    /// <summary>
    /// FloorMovingの移動範囲
    /// </summary>
    [SerializeField]
    private float moveRange;

    void Start()
    {
        // Rigidbodyを取得
        body = GetComponent<Rigidbody>();

        // オブジェクトの位置を取得
        startPosition = transform.position;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // 現在位置に対して往復するように設定
        float xPosition = startPosition.x + Mathf.PingPong(Time.time * moveSpeed, moveRange);

        // オブジェクトを設定した範囲、速度で往復させる
        body.MovePosition(new Vector3(xPosition, startPosition.y, startPosition.z));
    }
}
