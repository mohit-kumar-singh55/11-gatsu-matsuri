using System.Collections;
using UnityEngine;

/// <summary>
/// プレイヤーがトリガーゾーンに入るとピーキングエネミーをアクティブにし、出ると非アクティブにします
/// </summary>
public class PeekingEnemyTrigger : MonoBehaviour
{
    #region Serialize Fields
    [SerializeField] private GameObject peekingEnemy;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip peekingAnimClip;
    #endregion

    private const string RIGHT_PEEK_FORWARD = "Right_Peek_Forward";
    private const string RIGHT_PEEK_BACKWARD = "Right_Peek_Backward";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TAGS.PLAYER))
        {
            peekingEnemy.SetActive(true);
            animator.SetTrigger(RIGHT_PEEK_FORWARD);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TAGS.PLAYER))
        {
            animator.SetTrigger(RIGHT_PEEK_BACKWARD);
            StartCoroutine(InactiveAfterAnim());
        }
    }

    // アニメーション終了後に非アクティブにする
    IEnumerator InactiveAfterAnim()
    {
        yield return new WaitForSeconds(peekingAnimClip.length);
        peekingEnemy.SetActive(false);
    }
}