using UnityEngine;

public interface Interactable
{
    Transform transform { get; }

    string DisplayName { get; }

    bool CanInteract();

    void Interact(GameObject player);

    void OnFocusGained();

    void OnFocusLost();
}
