using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, Interactable
{
    [SerializeField] private string displayName = "Interact";
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private UnityEvent onInteract;
    // a flag that checks if we need to add this object to the player inventory during Interact()
    [SerializeField] private bool canAddToInventory = false; // false unless checked otherwise

    public string DisplayName => displayName; // connects to the Interactable interface values
    public bool CanInteract() => isEnabled;

    private Outline outline;
    //REMOVE STATIC- needs to be player specific
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

    // takes the player that did the interaction as a param
    public void Interact(GameObject player)
    {
        lastObject = gameObject; // update to the object this script is attached to
        if (canAddToInventory) // if we need to add this object to the player's inventory, do so
        {
            Inventory playerInventory = player.GetComponent<Inventory>(); // get the inventory component from this player
            playerInventory.addToInventory(); // add it
        }
        onInteract?.Invoke(); //then, do the rest of the interactions set in the editor using an event system: On_Interact()
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
