using System.Collections;
using UnityEngine;

enum Mode { Blink, DestroyOnceEntered }

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class FloorBlink : MonoBehaviour
{
    [SerializeField] private Mode mode = Mode.Blink;
    [SerializeField] private float blinkInterval = 5.2f;
    [SerializeField] private float destroyAfterEntring = 2.0f;

    private bool hasTriggeredDestroy = false;

    private Collider col;
    private MeshRenderer mesh;

    void Awake()
    {
        col = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        if (mode == Mode.Blink) StartCoroutine(BlinkOrDestroyCoroutine());
    }

    void BlinkSelf(bool appear = true)
    {
        col.enabled = appear;
        mesh.enabled = appear;
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

    IEnumerator BlinkOrDestroyCoroutine()
    {
        bool appear = false;

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