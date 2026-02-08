using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, Interactable
{
    [SerializeField] private string displayName = "Interact";
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private UnityEvent onInteract;

    public string DisplayName => displayName; // connects to the Interactable interface values
    public bool CanInteract() => isEnabled;

    private Outline outline;
    public static GameObject lastObject; // a reference to the last object interacted with for when we add it to the inventory

    public void Awake()
    {
        // add an outline and give it a color and width
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineColor = Color.yellow;
        outline.OutlineWidth = 5f;
        outline.enabled = false; // disable it until we focus on it

        lastObject = gameObject; // update to the object this script is attached to
    }

    public void Interact()
    {
        lastObject = gameObject; // update to the object this script is attached to
        onInteract?.Invoke(); //using an event system on On_Interact()
    }

    public void OnFocusGained()
    {
        outline.enabled = true;
    }

    public void OnFocusLost()
    {
        outline.enabled = false;
    }
}
