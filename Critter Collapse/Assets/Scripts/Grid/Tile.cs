using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor, offsetColor;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private GameObject highlight;

    public int xPos; // x coord of this tile
    public int yPos; // y coord of this tile

    // if this is an offset tile, change its color on init
    public void Init(bool isOffset)
    {
        renderer.color = isOffset ? offsetColor : baseColor; 
    }

    // when we hover over this tile, set the highlight color active
    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    // when we are not hovering over this tile, set the highlight color inactive
    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    // prints this tile's x and y pos for testing
    public void printPos()
    {
        Debug.Log("X: " + xPos + " Y: " + yPos);
    }

    // places the plant on THIS grid piece
    public void placePlantHere()
    {
        // if there is an object in our inventory
        if (Inventory.inInventory != null)
        {
            // and we have a plant in our inventory, we can place it in the bin at this grid's x and y
            if (Inventory.inInventory.GetComponent<PlantShape>() != null)
            {
                PlantShape plantObjInInventory = Inventory.inInventory.GetComponent<PlantShape>();
                plantObjInInventory.placeInBinRPC(xPos, yPos);
                //Debug.Log("PLANT ID: " + plantObjInInventory.myPlantID);
            }
            // otherwise we cannot
            else
            {
                Debug.Log("Cannot place a non-plant object in the plant bin");
            }
        }
        Debug.Log("GRID INTERACTED AT: " + xPos + ", " + yPos);
    }
}
