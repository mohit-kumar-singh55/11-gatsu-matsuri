using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Mode { Blink, DestroyOnceEntered }

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class FloorBlink : MonoBehaviour
{
    [SerializeField] private Mode mode = Mode.Blink;
    [SerializeField] private float blinkInterval = 5.2f;
    [SerializeField] private float destroyAfterEntring = 2.0f;
    [SerializeField] private bool disappearChildMesh = false;

    private bool hasTriggeredDestroy = false;

    private Collider col;
    private MeshRenderer mesh;
    private List<GameObject> childObjects = new();

    void Awake()
    {
        col = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();

    }

    void Start()
    {
        if (mode == Mode.Blink) StartCoroutine(BlinkOrDestroyCoroutine());
        if (disappearChildMesh)
        {
            foreach (Transform child in transform) childObjects.Add(child.gameObject);
        }
    }

    void BlinkSelf(bool appear = true)
    {
        col.enabled = appear;
        mesh.enabled = appear;

        if (disappearChildMesh)
        {
            foreach (GameObject Child in childObjects)
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