using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class FloorBlink : MonoBehaviour
{
    private enum ProcessSelect
    {
        Blink,
        Stand
    }

    [SerializeField] private ProcessSelect processSelect;
    [SerializeField] private float blinkInterval = 5.2f;
    [SerializeField] private float standInterval = 2.0f;

    private bool isStand = false;

    private Collider col;
    private MeshRenderer mesh;

    void Awake()
    {
        col = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        if (processSelect == ProcessSelect.Blink)
        {
            StartCoroutine(BlinkCoroutine());
        }
    }

    void BlinkSelf(bool appear = true)
    {
        col.enabled = appear;
        mesh.enabled = appear;
    }

    IEnumerator BlinkCoroutine()
    {
        bool appear = false;

        while (true)
        {
            yield return new WaitForSeconds(blinkInterval);

            // フロア点滅オブジェクトを切り替え
            BlinkSelf(appear);

            appear = !appear;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isStand = true;
            StartCoroutine(StandDelayCoroutine());
        }
    }

    IEnumerator StandDelayCoroutine()
    {
        if (isStand)
        {
            yield return new WaitForSeconds(standInterval);

            Destroy(gameObject);
        }
    }
}