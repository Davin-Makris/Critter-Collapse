using UnityEngine;

public interface Interactable
{
    Transform transform { get; }

    string DisplayName { get; }

    bool CanInteract();

    void Interact();

    void OnFocusGained();

    void OnFocusLost();
}
