using UnityEngine;
using Unity.Netcode;

public class Inventory : NetworkBehaviour
{   // you can only have one thing in the inventoy at a time
    public static bool inventoryFull = false;
    public static GameObject inInventory; // the obj currently in the inventory
    [SerializeField] GameObject inventory; // the player's inventory
    //private GameObject lastObject = InteractableObject.lastObject; // the object that we last interacted with


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
