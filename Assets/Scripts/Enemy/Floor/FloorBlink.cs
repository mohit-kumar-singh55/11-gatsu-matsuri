using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(MeshRenderer))]
public class FloorBlink : MonoBehaviour
{
    [SerializeField] float blinkInterval = 3.0f;

    private Collider col;
    private MeshRenderer mesh;

    void Awake()
    {
        col = GetComponent<Collider>();
        mesh = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        StartCoroutine(BlinkCoroutine());
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
}