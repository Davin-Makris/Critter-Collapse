using UnityEngine;

public class GridBox : MonoBehaviour
{
    [SerializeField] short xCord; // x position, starting at 0
    [SerializeField] short yCord; // y postition, starting at 0
    //private PlantShape plantRef;
    private GameObject objInInventory; // the object currently in the player's inventory 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // places the plant on THIS grid piece
    public void placePlantHere()
    {
        //Debug.Log("GRID INTERACTED AT: " + xCord + ", " + yCord);
        //Debug.Log(Inventory.inInventory);
        // if there is an object in our inventory
        if (Inventory.inInventory != null)
        {
            // and we have a plant in our inventory, we can place it in the bin at this grid's x and y
            if (Inventory.inInventory.GetComponent<PlantShape>() != null)
            {
                PlantShape plantObjInInventory = Inventory.inInventory.GetComponent<PlantShape>();
                plantObjInInventory.placeInBinRPC(xCord, yCord);
                //Debug.Log("PLANT ID: " + plantObjInInventory.myPlantID);
            }
            // otherwise we cannot
            else
            {
                Debug.Log("Cannot place a non-plant object in the plant bin");
            }
        }
        Debug.Log("GRID INTERACTED AT: " + xCord + ", " + yCord);
    }
}
