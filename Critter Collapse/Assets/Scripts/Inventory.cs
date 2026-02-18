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
         inventory.GetComponent<NetworkObject>().Spawn(); // spawn the inventory network object
         AddToPlayerServerRpc(inventory.GetComponent<NetworkObject>().NetworkObjectId); // set the inventory back to the parent object

    }

    public void addToInventory()
    {
        if (!IsOwner) {return;}

        inInventory = InteractableObject.lastObject; // update our inventory
        NetworkObject inInventoryNO;
        if (inventoryFull) // if we already have something in the inventory, drop it
        {
            if (inInventoryNO = inInventory.GetComponentInParent<NetworkObject>())
            {
                Debug.Log("Removing Item: " + inInventoryNO.TryRemoveParent(true) + "\nParent: " + inInventory.transform.parent +"\nLast Object Parent: " + InteractableObject.lastObject.transform.parent);
                inInventory.transform.SetParent(null);
            }
            else
            {
                inInventory.transform.SetParent(null); // drop the object
            }
            inInventory = null;
            inventoryFull = false;
        }
        else // otherwise pick it up
        {
            
            if (inInventoryNO = inInventory.GetComponentInParent<NetworkObject>())
            {
                Debug.Log("Trying to pick up network Object");
                //Debug.Log("Object Picked Up: " + inInventoryNO.TrySetParent(inventory, false));
                AddToInventoryServerRpc(inInventoryNO.NetworkObjectId);
                //inInventory.transform.SetParent(inventory.transform);
                inventoryFull = true;
            }
            else
            {
                inInventory.transform.SetParent(inventory.transform); // pickup the last object we interacted with
                inventoryFull = true;
            }
            //Debug.Log("Currently in inventory: " + inInventory);
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
