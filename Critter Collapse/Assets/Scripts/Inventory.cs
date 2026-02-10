using UnityEngine;
using Unity.Netcode;

public class Inventory : NetworkBehaviour
{   // you can only have one thing in this inventoy at a time
    public bool inventoryFull = false;
    public static GameObject inInventory; // the obj currently in this inventory
    [SerializeField] GameObject inventory; // this player's inventory

    // on awake: find game object inventory and set it to the inventory var for this script
    public void onAwake()
    {
         inventory = gameObject.transform.Find("Inventory").gameObject; // get the inventory game object on this player object and set it
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
                inInventoryNO.TrySetParent(inventory);
            }
            else
            {
                inInventory.transform.SetParent(inventory.transform); // pickup the last object we interacted with 
            }
            inventoryFull = true;
            //Debug.Log("Currently in inventory: " + inInventory);
        }
    }
}
