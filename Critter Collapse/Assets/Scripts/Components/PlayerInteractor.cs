using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask interactableLayers;

    private Collider2D[] buffer = new Collider2D[32]; // contains all objects around us
    public Interactable focused; // the object that we are currently focusing on
    public GameObject focusedOnGameObj; // the gameOBj we are currently focused on

    void Update()
    {
        Interactable nearest = FindNearestInteractable();
        UpdateFocus(nearest);
    }

    //Called by inputSystem when player presses E
    void OnInteract(InputValue value) 
    {
        //Debug.Log("E pressed by player");
        // if there is something we are focused on and we press E to interact with it
        if (focused != null)
        {
            //Debug.Log("Interacting...");
            // if we can interact with it, do so
            Debug.Log("Name: " + gameObject.name + " inateacts with ");
            if (focused.CanInteract()) focused.Interact(gameObject); // gameObject refers to THIS player that is doing the interacting
        }
    }

    // finds the nearest interactable object and returns it
    private Interactable FindNearestInteractable()
    {
        // adds all nearby objects in the raduis to our buffer
        //int count = Physics.OverlapSphereNonAlloc(transform.position, radius, buffer, interactableLayers, QueryTriggerInteraction.Collide);
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, radius, buffer, interactableLayers, -1, 0);
        Interactable nearest = null;
        float BestDistSq = float.MaxValue;

        // go through each collider that is not null and try to get an interactable object that we can interact with
        for (int i = 0; i < count; i++)
        {
            Collider2D col = buffer[i]; // go through each collider in the buffer
            if (col == null) continue; // if they're null we can ignore it
            Interactable interactable = col.GetComponentInParent<Interactable>();
            if (interactable == null) continue; // if this is null too we can ignore it
            float DistSq = (col.transform.position - transform.position).sqrMagnitude;
            if (DistSq < BestDistSq) // if we have a better distance
            {
                focusedOnGameObj = col.gameObject; // get the game object this collider is attached to (what we're focused on)
                BestDistSq = DistSq; // update the distance
                nearest = interactable; // update the nearest object as well
            }
        }
        return nearest;
    }

    private void UpdateFocus(Interactable nearest)
    {
        // if the new focus is not equal to the current, change the focus
        if (ReferenceEquals(focused, nearest)) return;
        focused?.OnFocusLost();
        focused = nearest;
        focused?.OnFocusGained();
    }
}
