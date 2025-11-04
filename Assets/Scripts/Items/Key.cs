using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    public void OnInteract(PlayerController player = null)
    {
        if (player == null) return;

        GameManager.Instance.SetHasKey(true);
        UIManager.Instance.ShowKeyUI(true);

        Destroy(gameObject);
    }
}
