using System.Collections.Generic;
using UnityEngine;

public class FloorAppearance : MonoBehaviour
{
    [Tooltip("もしtrueならプレイヤーがトリガー内にいるときに床が表示され、falseなら非表示になります。")]
    [SerializeField] private bool appearOnTrigger = true;

    private GameObject _floor;

    void Start()
    {
        // 1番目の子オブジェクトを取得
        _floor = transform.GetChild(0).gameObject;

        _floor.SetActive(!appearOnTrigger);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TAGS.PLAYER)) _floor.SetActive(appearOnTrigger);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TAGS.PLAYER)) _floor.SetActive(!appearOnTrigger);
    }
}
