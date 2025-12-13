using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Mode { Blink, DestroyOnceEntered }

/// <summary>
/// 指定されたモードに応じて点滅または自己破壊する床ブリンクオブジェクトを定義するクラス
/// </summary>
[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class FloorBlink : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField] private Mode mode = Mode.Blink;
    [SerializeField] private float blinkInterval = 5.2f;
    [SerializeField] private float destroyAfterEntring = 2.0f;
    [SerializeField] private bool disappearChildMesh = false;
    #endregion

    #region Private Fields
    private Collider _col;
    private MeshRenderer _mesh;
    private List<GameObject> _childObjects = new();
    private bool hasTriggeredDestroy = false;
    #endregion

    void Awake()
    {
        _col = GetComponent<Collider>();
        _mesh = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        if (mode == Mode.Blink) StartCoroutine(BlinkOrDestroyCoroutine());
        if (disappearChildMesh)
        {
            // 子オブジェクトをリストに追加
            foreach (Transform child in transform) _childObjects.Add(child.gameObject);
        }
    }

    /// <summary>
    /// このオブジェクトのコライダーと MeshRenderer を有効／無効にする
    /// </summary>
    void BlinkSelf(bool appear = true)
    {
        _col.enabled = appear;
        _mesh.enabled = appear;

        if (disappearChildMesh)
        {
            foreach (GameObject Child in _childObjects)
                Child.SetActive(appear);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (mode != Mode.DestroyOnceEntered || hasTriggeredDestroy) return;

        if (collision.gameObject.CompareTag(TAGS.PLAYER))
        {
            hasTriggeredDestroy = true;
            StartCoroutine(BlinkOrDestroyCoroutine());
        }
    }

    /// <summary>
    /// このオブジェクトを点滅・消滅させる
    /// </summary>
    IEnumerator BlinkOrDestroyCoroutine()
    {
        // 点滅・消滅処理
        bool appear = false;

        // 点滅モードかどうか
        bool isBlink = mode == Mode.Blink;

        while (true)
        {
            yield return new WaitForSeconds(isBlink ? blinkInterval : destroyAfterEntring);

            // フロア点滅オブジェクトを切り替え
            if (isBlink) BlinkSelf(appear);
            else
            {
                Destroy(gameObject);
                yield break;
            }

            appear = !appear;
        }
    }
}