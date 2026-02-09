using UnityEngine;

public class Inventory : MonoBehaviour
{   // you can only have one thing in the inventoy at a time
    static bool inventoryFull = false;
    private GameObject inInventory; // the obj currently in the inventory
    [SerializeField] GameObject inventory; // the player's inventory
    //private GameObject lastObject = InteractableObject.lastObject; // the object that we last interacted with


    public void addToInventory()
    {
        if (inventoryFull) // if we already have something in the inventory, drop it
        {
            inInventory.transform.SetParent(null); // drop the object
            inventoryFull = false;
        }
        else // otherwise pick it up
        {
            inInventory = InteractableObject.lastObject; // update our inventory
            inInventory.transform.SetParent(inventory.transform); // pickup the last object we interacted with 
            inventoryFull = true;
        }
    }
}
