using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    bool hasKey = false;

    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        hasKey = true;

        UIManager.Instance.ShowKeyUI(true);

        Destroy(gameObject);
    }
}
