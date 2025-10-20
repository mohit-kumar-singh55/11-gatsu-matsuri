using UnityEngine;

/// <summary>
/// オブジェクトの出現と消滅を繰り返す
/// </summary>
public class FloorBlink : MonoBehaviour
{
    /// <summary>
    /// Rigidbody取得用
    /// </summary>
    private Rigidbody body;

    /// <summary>
    /// 点滅させるオブジェクト
    /// </summary>
    [SerializeField]
    private GameObject[] floorBlink;

    /// <summary>
    /// オブジェクトの出現・消失の間隔
    /// </summary>
    [SerializeField]
    private float blinkInterval = 5.0f;

    /// <summary>
    /// 時間経過を格納
    /// </summary>
    private float elapsedTime = 0;

    void Start()
    {
        // Rigidbodyを取得
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // 時間を計測
        elapsedTime += Time.fixedDeltaTime;

        // 指定時間を経過したら
        if (elapsedTime >= blinkInterval)
        {
            // 点滅させるオブジェクトの数分処理する
            for (int i = 0; i < floorBlink.Length; i++)
            {
                // 現在のオブジェクトの状態を反転させる
                floorBlink[i].SetActive(!floorBlink[i].activeSelf);
            }

            // 経過時間を初期化
            elapsedTime = 0;
        }
    }
}