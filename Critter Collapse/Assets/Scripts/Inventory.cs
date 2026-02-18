using UnityEngine;
using Unity.Netcode;

public class Inventory : NetworkBehaviour
{   // you can only have one thing in this inventoy at a time
    public bool inventoryFull = false;
    //NOTE: REMOVE STATIC FROM HERE AND TILE.CS
    public static GameObject inInventory; // the obj currently in this inventory
    [SerializeField] GameObject inventory; // this player's inventory

    // on awake: find game object inventory and set it to the inventory var for this script
    void Awake()
    {
         inventory = gameObject.transform.Find("Inventory").gameObject; // get the inventory game object on this player object and set it
         //inventory.GetComponent<NetworkObject>().Spawn(); // spawn the inventory network object
         //AddToPlayerServerRpc(inventory.GetComponent<NetworkObject>().NetworkObjectId); // set the inventory back to the parent object
    }

    public void addToInventory()
    {
        if (!IsOwner) {return;}

        inInventory = InteractableObject.lastObject; // update our inInventory to the last object we interacted with
        NetworkObject inInventoryNO;
        if (inventoryFull) // if we already have something in the inventory, drop it
        {
            // if we're working with a networked object
            if (inInventoryNO = inInventory.GetComponent<NetworkObject>())
            {
                //Debug.Log("Removing Item: " + inInventoryNO.TryRemoveParent(true) + "\nParent: " + inInventory.transform.parent +"\nLast Object Parent: " + InteractableObject.lastObject.transform.parent);
                //inInventory.transform.SetParent(null);
                Debug.Log("Removing Networked object");
                inInventoryNO.TryRemoveParent(false);
                inInventoryNO.transform.position = gameObject.transform.position; // on drop, set the pos of the object to the player's pos
            }
            // otherwise
            else
            {
                Debug.Log("Removing regular object");
                inInventory.transform.SetParent(null); // drop the object
            }
            inInventory = null;
            inventoryFull = false;
        }
        else // otherwise pick it up
        {
            // if this is a networked object
            if (inInventoryNO = inInventory.GetComponent<NetworkObject>()) // GetComponentInParent
            {
                Debug.Log("Trying to pick up network Object");
                //Debug.Log("Object Picked Up: " + inInventoryNO.TrySetParent(inventory, false));
                AddToInventoryServerRpc(inInventoryNO.NetworkObjectId);
                //inInventory.transform.SetParent(inventory.transform);
                inventoryFull = true;
            }
            // otherwise
            else
            {
                inInventory.transform.SetParent(inventory.transform); // pickup the last object we interacted with
                inventoryFull = true;
            }
        }
    }

    // only the server has permissions to reparent items, so this does that work on the server side
    // adds the object as a child of the inventory
    [ServerRpc]
    private void AddToInventoryServerRpc(ulong objectId)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject netObj))
            return;

        if (inventoryFull)
            return;

        Debug.Log("Parent set to inventory: " + netObj.TrySetParent(inventory.transform, false));
    }

    // adds the object as a child of the player 
    [ServerRpc]
    private void AddToPlayerServerRpc(ulong objectId)
    {
        if (!NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(objectId, out NetworkObject netObj))
            return;

        Debug.Log("Parent set to player: " + netObj.TrySetParent(gameObject.transform, false));
    }
}
