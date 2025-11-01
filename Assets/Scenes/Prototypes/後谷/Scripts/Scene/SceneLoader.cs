using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // どのシーンからゲームオーバーになったかを保持
    public static string previousScene;

    // ゲームオーバーに遷移するメソッド
    public static void GoToGameOver()
    {
        // 現在のシーン名を保存
        previousScene = SceneManager.GetActiveScene().name;

        // GameOverシーンへ
        SceneManager.LoadScene("GameOver");
    }

    // 指定したシーンに移動する汎用メソッド
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
