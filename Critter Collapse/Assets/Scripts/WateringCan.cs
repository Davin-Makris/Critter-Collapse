using UnityEngine;

public class WateringCan : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint; // the spawn point for the can

    [HideInInspector]
    public bool holdingCan = false; // if the player is holding the watering can or not

    public void HoldWateringCan()
    {
        if (holdingCan)
        {
            //this.transform.SetParent(spawnPoint.transform); // make this a child of the spawn point
            holdingCan = false; // set holdingCan accordingly
        }
        else
        {
            //this.transform.SetParent(inventory.transform); // make this a child of the inventory
            holdingCan = true; // set holdingCan accordingly
        }
    }
}
