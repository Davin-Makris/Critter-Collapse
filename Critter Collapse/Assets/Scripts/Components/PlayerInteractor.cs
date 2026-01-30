using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float radius = 2f;
    [SerializeField] private LayerMask interactableLayers;

    private Collider2D[] buffer = new Collider2D[32]; // contains all objects around us
    private Interactable focused; // the object that we are currently focusing on

    void Update()
    {
        Interactable nearest = FindNearestInteractable();
        UpdateFocus(nearest);
        // if there is something we are focused on and we press E to interact with it
        if (focused != null && Input.GetKeyDown(KeyCode.E))
        {
            // if we can interact with it, do so
            if (focused.CanInteract()) focused.Interact();
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
                BestDistSq = DistSq; // update the distance
                nearest = interactable; // update the nearest object as well
            }
        }
        return nearest;
    }

    private Interactable OnTriggerEnter2D()
    {
        Debug.Log("OnTriggerEnter2D: ENTERED");
        // adds all nearby objects in the raduis to our buffer
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, radius, buffer, interactableLayers, -1, 0); //QueryTriggerInteraction.Collide
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
                BestDistSq = DistSq; // update the distance
                nearest = interactable; // update the nearest object as well
            }
        }
        Debug.Log("OnTriggerEnter2D: EXIT");
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
