using UnityEngine;

/// <summary>
/// プレイヤーが近づくとオブジェクトを消失させる
/// </summary>
public class FloorDisappear : MonoBehaviour
{
    /// <summary>
    /// Rigidbody取得用
    /// </summary>
    private Rigidbody body;

    /// <summary>
    /// 消失させるオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject floorDisappear;

    void Start()
    {
        // オブジェクトの初期状態はアクティブ
        floorDisappear.SetActive(true);

    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        // Playerタグのオブジェクトに触れている間
        if (other.gameObject.tag == "Player")
        {
            // 現在のオブジェクトの状態を非アクティブにする
            floorDisappear.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Playerオブジェクトが触れなくなったら
        if (other.gameObject.tag == "Player")
        {
            // 現在のオブジェクトの状態をアクティブにする
            floorDisappear.SetActive(true);
        }
    }
}
