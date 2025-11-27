using System.Collections;
using UnityEngine;

// プレイヤーがトリガーゾーンに入るとピーキングエネミーをアクティブにし、出ると非アクティブにします。
public class PeekingEnemyTrigger : MonoBehaviour
{
    [SerializeField] private GameObject peekingEnemy;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip peekingAnimClip;

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

    IEnumerator InactiveAfterAnim()
    {
        yield return new WaitForSeconds(peekingAnimClip.length);
        peekingEnemy.SetActive(false);
    }
}
