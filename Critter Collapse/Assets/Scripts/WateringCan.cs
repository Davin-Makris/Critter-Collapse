using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [SerializeField] GameObject inventory; // the player's inventory
    [SerializeField] GameObject spawnPoint; // the spawn point for the can

    public bool holdingCan = false; // if the player is holding the watering can or not

    // the On Interact for the watering can
    // if the player is not holding it, it will be added to the inventory
    // if the player is holding it, it will go back to the spawn point
    public void HoldWateringCan()
    {
        if (holdingCan)
        {
            this.transform.SetParent(spawnPoint.transform); // make this a child of the spawn point
            holdingCan = false; // set holdingCan accordingly
        }
        else
        {
            this.transform.SetParent(inventory.transform); // make this a child of the inventory
            holdingCan = true; // set holdingCan accordingly
        }
    }
}
