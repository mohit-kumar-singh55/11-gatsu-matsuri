using UnityEngine;

public class InstructionsUIManager : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown) SceneLoader.LoadScene(SCENES.LEVEL_1);
    }
}