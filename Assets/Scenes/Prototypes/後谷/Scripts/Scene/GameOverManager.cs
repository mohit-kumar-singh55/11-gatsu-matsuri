using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // リトライボタン用の関数
    public void OnRetryButton()
    {
        // 前のシーンが設定されていればそこに戻る
        if (!string.IsNullOrEmpty(SceneLoader.previousScene))
        {
            SceneManager.LoadScene(SceneLoader.previousScene);
        }
        else
        {
            // 万が一何も記録がない場合はデフォルトでシーン1に戻す
            SceneManager.LoadScene("Scene1");
        }
    }

    // タイトルに戻るボタン用
    public void OnTitleButton()
    {
        SceneManager.LoadScene("Title");
    }
}