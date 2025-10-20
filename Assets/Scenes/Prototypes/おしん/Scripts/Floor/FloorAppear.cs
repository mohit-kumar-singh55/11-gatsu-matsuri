using UnityEngine;

/// <summary>
/// プレイヤーが近づくとオブジェクトを出現させる
/// </summary>
public class FloorAppear : MonoBehaviour
{
    /// <summary>
    /// Rigidbody取得用
    /// </summary>
    private Rigidbody body;

    /// <summary>
    /// 出現させるオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject floorAppear;

    void Start()
    {
        // オブジェクトの初期状態は非アクティブ
        floorAppear.SetActive(false);
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
            // 現在のオブジェクトの状態をアクティブにする
            floorAppear.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Playerオブジェクトが触れなくなったら
        if (other.gameObject.tag == "Player")
        {
            // 現在のオブジェクトの状態を非アクティブにする
            floorAppear.SetActive(false);
        }
    }
}
