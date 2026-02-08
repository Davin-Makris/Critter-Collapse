using UnityEngine;

public class Inventory : MonoBehaviour
{   // you can only  have one thing in the inventoy at a time
    static bool inventoryFull = false;
    [SerializeField] GameObject inInventory; // the obj currently in the inventory
    [SerializeField] GameObject inventory; // the player's inventory
    private GameObject lastObject = InteractableObject.lastObject; // the object that we last interacted with


    public void addToInventory()
    {
        if (inventoryFull) // if we already have something in the inventory, drop it
        {
            inInventory.transform.SetParent(null); // drop the object
            inventoryFull = false;
        }
        else // otherwise you can pick it up
        {
            inInventory.transform.SetParent(inventory.transform); // make the obj a child of the inventory
            inventoryFull = true;
        }
    }
}
